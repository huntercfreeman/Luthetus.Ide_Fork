using System.Text;
using CliWrap.EventStream;

namespace Luthetus.Ide.RazorLib.Terminals.Models.NewCode;

public class TerminalOutputTextEditor : ITerminalOutput
{
	private readonly ITerminal _terminal;

	public TerminalOutputTextEditor(ITerminal terminal)
	{
		_terminal = terminal;
		_terminal.TerminalInteractive.WorkingDirectoryChanged += OnWorkingDirectoryChanged;
	}
	
	private string _output = string.Empty;
	
	public string OutputRaw
	{
		get => _output;
		private set
		{
			_output = value;
			OnWriteOutput?.Invoke();
		}
	}
	
	public StringBuilder OutputBuilder { get; } = new();
	
	public event Action? OnWriteOutput;

	public void OnWorkingDirectoryChanged()
	{
	}
	
	public void OnHandleCommandStarting()
	{
	}
	
	public void WriteOutput(TerminalCommandParsed terminalCommandParsed, CommandEvent commandEvent)
	{
		var output = (string?)null;

		switch (commandEvent)
		{
			case StartedCommandEvent started:
				OutputBuilder.Append($"TEXTEDITOR{terminalCommandParsed.SourceTerminalCommandRequest.CommandText}\n");
				break;
			case StandardOutputCommandEvent stdOut:
				OutputBuilder.Append($"{stdOut.Text}\n");
				break;
			case StandardErrorCommandEvent stdErr:
				OutputBuilder.Append($"{stdErr.Text}\n");
				break;
			case ExitedCommandEvent exited:
				OutputBuilder.Append($"Process exited; Code: {exited.ExitCode}\n");
				break;
		}
		
		OutputRaw = OutputBuilder.ToString();
	}
	
	public void Dispose()
	{
		_terminal.TerminalInteractive.WorkingDirectoryChanged -= OnWorkingDirectoryChanged;
	}
}
