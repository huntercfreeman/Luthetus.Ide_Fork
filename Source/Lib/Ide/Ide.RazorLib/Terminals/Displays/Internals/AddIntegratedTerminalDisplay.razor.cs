using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Fluxor;
using Luthetus.Common.RazorLib.BackgroundTasks.Models;
using Luthetus.Common.RazorLib.ComponentRenderers.Models;
using Luthetus.Common.RazorLib.Dialogs.Models;
using Luthetus.Common.RazorLib.FileSystems.Models;
using Luthetus.Common.RazorLib.Keys.Models;
using Luthetus.Common.RazorLib.Dynamics.Models;
using Luthetus.TextEditor.RazorLib;
using Luthetus.TextEditor.RazorLib.CompilerServices.Interfaces;
using Luthetus.Ide.RazorLib.Terminals.Models;
using Luthetus.Ide.RazorLib.Terminals.States;

namespace Luthetus.Ide.RazorLib.Terminals.Displays.Internals;

public partial class AddIntegratedTerminalDisplay : ComponentBase
{
	[Inject]
	private ITextEditorService TextEditorService { get; set; } = null!;
	[Inject]
	private ICompilerServiceRegistry CompilerServiceRegistry { get; set; } = null!;
	[Inject]
	private IDialogService DialogService { get; set; } = null!;
	[Inject]
	private IJSRuntime JsRuntime { get; set; } = null!;
	[Inject]
	private IDispatcher Dispatcher { get; set; } = null!;
	[Inject]
	private IBackgroundTaskService BackgroundTaskService { get; set; } = null!;
	[Inject]
	private ICommonComponentRenderers CommonComponentRenderers { get; set; } = null!;
	[Inject]
	private IEnvironmentProvider EnvironmentProvider { get; set; } = null!;
	
	[CascadingParameter]
	public IDialog Dialog { get; set; } = null!;

	private string _pathToShellExecutable = string.Empty;
	private string _integratedTerminalDisplayName = string.Empty;
	
	private void SubmitOnClick()
	{
		var pathToShellExecutableLocal = _pathToShellExecutable;
		var integratedTerminalDisplayNameLocal = _integratedTerminalDisplayName;
		
		Dispatcher.Dispatch(new TerminalState.RegisterAction(
	    	new TerminalIntegrated(
				_integratedTerminalDisplayName,
				terminal => new TerminalInteractive(terminal),
				terminal => new TerminalInputStringBuilder(terminal),
				terminal => new TerminalOutput(
					terminal,
					new TerminalOutputFormatterExpand(
						terminal,
						TextEditorService,
						CompilerServiceRegistry,
						DialogService,
					    JsRuntime,
						Dispatcher)),
				BackgroundTaskService,
				CommonComponentRenderers,
				Dispatcher,
				EnvironmentProvider,
				_pathToShellExecutable)
			{
				Key = Key<ITerminal>.NewKey()
			}));
			
		DialogService.DisposeDialogRecord(Dialog.DynamicViewModelKey);
	}
}