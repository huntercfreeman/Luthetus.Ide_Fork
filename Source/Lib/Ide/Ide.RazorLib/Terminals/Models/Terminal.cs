using System.Reactive.Linq;
using CliWrap;
using CliWrap.EventStream;
using Fluxor;
using Luthetus.Common.RazorLib.BackgroundTasks.Models;
using Luthetus.Common.RazorLib.ComponentRenderers.Models;
using Luthetus.Common.RazorLib.Keys.Models;
using Luthetus.Common.RazorLib.Notifications.Models;

namespace Luthetus.Ide.RazorLib.Terminals.Models;

/// <summary>
/// This implementation of <see cref="ITerminal"/> is a "blank slate".
/// </summary>
public class Terminal : ITerminal
{
	private readonly IBackgroundTaskService _backgroundTaskService;
	private readonly ICommonComponentRenderers _commonComponentRenderers;
	private readonly IDispatcher _dispatcher;

	public Terminal(
		string displayName,
		Func<Terminal, ITerminalInteractive> terminalInteractiveFactory,
		Func<Terminal, ITerminalInput> terminalInputFactory,
		Func<Terminal, ITerminalOutput> terminalOutputFactory,
		IBackgroundTaskService backgroundTaskService,
		ICommonComponentRenderers commonComponentRenderers,
		IDispatcher dispatcher)
	{
		DisplayName = displayName;
		TerminalInteractive = terminalInteractiveFactory.Invoke(this);
		TerminalInput = terminalInputFactory.Invoke(this);
		TerminalOutput = terminalOutputFactory.Invoke(this);
		
		_backgroundTaskService = backgroundTaskService;
		_commonComponentRenderers = commonComponentRenderers;
		_dispatcher = dispatcher;
	}

	public string DisplayName { get; }
	public ITerminalInteractive TerminalInteractive { get; }
	public ITerminalInput TerminalInput { get; }
	public ITerminalOutput TerminalOutput { get; }

	private CancellationTokenSource _commandCancellationTokenSource = new();

    public Key<ITerminal> Key { get; init; } = Key<ITerminal>.NewKey();
    public TerminalCommandParsed? ActiveTerminalCommandParsed { get; private set; }

	/// <summary>NOTE: the following did not work => _process?.HasExited ?? false;</summary>
    public bool HasExecutingProcess { get; private set; }

    public void EnqueueCommand(TerminalCommandRequest terminalCommandRequest)
    {
		_backgroundTaskService.Enqueue(
			Key<IBackgroundTask>.NewKey(),
			BlockingBackgroundTaskWorker.GetQueueKey(),
			"Enqueue Command",
			() => HandleCommand(terminalCommandRequest));
    }
    
    public Task EnqueueCommandAsync(TerminalCommandRequest terminalCommandRequest)
    {
		return _backgroundTaskService.EnqueueAsync(
			Key<IBackgroundTask>.NewKey(),
			BlockingBackgroundTaskWorker.GetQueueKey(),
			"Enqueue Command",
			() => HandleCommand(terminalCommandRequest));
    }
    
    public void EnqueueClear()
    {
    	EnqueueCommand(new TerminalCommandRequest("clear", null));
    }

    private async Task HandleCommand(TerminalCommandRequest terminalCommandRequest)
    {
    	if (TerminalOutput.GetParsedCommandListCount() > 10)
    	{
            TerminalOutput.ClearOutput();
        }
    
    	var parsedCommand = await TerminalInteractive.TryHandleCommand(terminalCommandRequest);
    	ActiveTerminalCommandParsed = parsedCommand;

		if (parsedCommand is null)
			return;
    	
    	var cliWrapCommand = Cli.Wrap(parsedCommand.TargetFileName);

		cliWrapCommand = cliWrapCommand.WithWorkingDirectory(TerminalInteractive.WorkingDirectory);

		if (!string.IsNullOrWhiteSpace(parsedCommand.Arguments))
			cliWrapCommand = cliWrapCommand.WithArguments(parsedCommand.Arguments);
		
		// TODO: Decide where to put invocation of 'parsedCommand.SourceTerminalCommandRequest.BeginWithFunc'...
		//       ...and invocation of 'cliWrapCommand'
		//       and invocation of 'parsedCommand.SourceTerminalCommandRequest.ContinueWithFunc'
		//       |
		// 	  This comment is referring to the 'try/catch' block logic.
		//       If the 'BeginWithFunc' throws an exception should the 'cliWrapCommand' run?
		//       If the 'cliWrapCommand' throws an exception should the 'ContinueWithFunc' run?
		
		try
		{
			HasExecutingProcess = true;
		
			if (parsedCommand.SourceTerminalCommandRequest.BeginWithFunc is not null)
			{
				await parsedCommand.SourceTerminalCommandRequest.BeginWithFunc
					.Invoke(parsedCommand)
					.ConfigureAwait(false);
			}
			
			await cliWrapCommand
				.Observe(_commandCancellationTokenSource.Token)
				.ForEachAsync(HandleOutput)
				.ConfigureAwait(false);
		}
		catch (Exception e)
		{
			// TODO: This will erroneously write 'StartedCommandEvent' out twice...
			//       ...unless a check is added to see WHEN the exception was thrown.
			TerminalOutput.WriteOutput(
				parsedCommand,
				new StartedCommandEvent(-1));
		
			TerminalOutput.WriteOutput(
				parsedCommand,
				new StandardErrorCommandEvent(
					parsedCommand.SourceTerminalCommandRequest.CommandText +
					" threw an exception" +
					"\n"));
		
			NotificationHelper.DispatchError("Terminal Exception", e.ToString(), _commonComponentRenderers, _dispatcher, TimeSpan.FromSeconds(14));
		}
		finally
		{
			if (parsedCommand.SourceTerminalCommandRequest.ContinueWithFunc is not null)
			{
				await parsedCommand.SourceTerminalCommandRequest.ContinueWithFunc
					.Invoke(parsedCommand)
					.ConfigureAwait(false);
			}
		
			HasExecutingProcess = false;
		}
	}
	
	private void HandleOutput(CommandEvent commandEvent)
	{
		TerminalOutput.WriteOutput(ActiveTerminalCommandParsed, commandEvent);
	}

	public void KillProcess()
    {
        _commandCancellationTokenSource.Cancel();
        _commandCancellationTokenSource = new();
        DispatchNewStateKey();
    }

	private void DispatchNewStateKey()
    {
        // _dispatcher.Dispatch(new TerminalState.NotifyStateChangedAction(Key));
    }
    
    public void Dispose()
    {
    	TerminalInput.Dispose();
		TerminalOutput.Dispose();
		// Input and output are dependent on 'TerminalInteractive'.
		// Therefore, dispose it last.
		TerminalInteractive.Dispose();
    }
}
