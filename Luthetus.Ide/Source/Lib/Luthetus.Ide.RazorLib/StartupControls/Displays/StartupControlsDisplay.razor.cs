﻿using Fluxor;
using Fluxor.Blazor.Web.Components;
using Luthetus.Common.RazorLib.Keys.Models;
using Luthetus.Ide.RazorLib.CommandLines.Models;
using Luthetus.Ide.RazorLib.ProgramExecutions.States;
using Luthetus.Ide.RazorLib.Terminals.States;
using Luthetus.Ide.RazorLib.Terminals.Models;
using Microsoft.AspNetCore.Components;
using Luthetus.Common.RazorLib.Dialogs.Models;
using Luthetus.Common.RazorLib.Dialogs.States;
using Luthetus.Ide.RazorLib.Debugger;

namespace Luthetus.Ide.RazorLib.StartupControls.Displays;

public partial class StartupControlsDisplay : FluxorComponent
{
    [Inject]
    private IState<ProgramExecutionState> ProgramExecutionStateWrap { get; set; } = null!;
    [Inject]
    private IState<TerminalSessionState> TerminalSessionStateWrap { get; set; } = null!;
    [Inject]
    private IDispatcher Dispatcher { get; set; } = null!;

    private readonly Key<TerminalCommand> _newDotNetSolutionTerminalCommandKey = Key<TerminalCommand>.NewKey();
    private readonly CancellationTokenSource _newDotNetSolutionCancellationTokenSource = new();

    private TerminalCommand? GetStartProgramTerminalCommand()
    {
        var programExecutionState = ProgramExecutionStateWrap.Value;

        if (programExecutionState.StartupProjectAbsolutePath is null)
            return null;

        var ancestorDirectory = programExecutionState.StartupProjectAbsolutePath.ParentDirectory;

        if (ancestorDirectory is null)
            return null;

        var formattedCommand = DotNetCliCommandFormatter.FormatStartProjectWithoutDebugging(
            programExecutionState.StartupProjectAbsolutePath);

        return new TerminalCommand(
            _newDotNetSolutionTerminalCommandKey,
            formattedCommand,
            ancestorDirectory.Path,
            _newDotNetSolutionCancellationTokenSource.Token);
    }

    private async Task StartProgramWithoutDebuggingOnClick()
    {
        var startProgramTerminalCommand = GetStartProgramTerminalCommand();

        if (startProgramTerminalCommand is null)
            return;

        var executionTerminalSession = TerminalSessionStateWrap.Value.TerminalSessionMap[
            TerminalSessionFacts.EXECUTION_TERMINAL_SESSION_KEY];

        await executionTerminalSession
            .EnqueueCommandAsync(startProgramTerminalCommand)
            .ConfigureAwait(false);
    }
    
    private async Task StartProgramWithDebuggingOnClick()
    {
        var startProgramTerminalCommand = GetStartProgramTerminalCommand();

        if (startProgramTerminalCommand is null)
            return;

        var executionTerminalSession = TerminalSessionStateWrap.Value.TerminalSessionMap[
            TerminalSessionFacts.EXECUTION_TERMINAL_SESSION_KEY];

        await executionTerminalSession
            .EnqueueCommandAsync(startProgramTerminalCommand)
            .ConfigureAwait(false);

        // Show debug dialog
        {
            var dialogRecord = new DialogRecord(
                Key<DialogRecord>.NewKey(),
                "Debug",
                typeof(DebuggerDisplay),
                null,
                null)
                {
                    IsResizable = true
                };

            Dispatcher.Dispatch(new DialogState.RegisterAction(dialogRecord));
        }
    }
}