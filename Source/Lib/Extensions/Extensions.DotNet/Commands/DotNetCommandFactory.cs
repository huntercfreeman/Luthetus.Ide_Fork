using Microsoft.JSInterop;
using Fluxor;
using Luthetus.Common.RazorLib.Contexts.Models;
using Luthetus.Common.RazorLib.Keymaps.Models;
using Luthetus.Common.RazorLib.Keys.Models;
using Luthetus.Common.RazorLib.Commands.Models;
using Luthetus.Common.RazorLib.TreeViews.Models;
using Luthetus.Common.RazorLib.FileSystems.Models;
using Luthetus.Common.RazorLib.JsRuntimes.Models;
using Luthetus.TextEditor.RazorLib.TextEditors.Models;
using Luthetus.TextEditor.RazorLib;
using Luthetus.Ide.RazorLib.Editors.Models;
using Luthetus.Ide.RazorLib.Commands;
using Luthetus.Extensions.DotNet.DotNetSolutions.States;
using Luthetus.Extensions.DotNet.Namespaces.Models;

namespace Luthetus.Extensions.DotNet.Commands;

public class DotNetCommandFactory : IDotNetCommandFactory
{
	private readonly ITextEditorService _textEditorService;
	private readonly ITreeViewService _treeViewService;
	private readonly IEnvironmentProvider _environmentProvider;
	private readonly IJSRuntime _jsRuntime;
	private readonly IDispatcher _dispatcher;

	public DotNetCommandFactory(
        ITextEditorService textEditorService,
        ITreeViewService treeViewService,
		IEnvironmentProvider environmentProvider,
		IJSRuntime jsRuntime,
		IDispatcher dispatcher)
	{
		_textEditorService = textEditorService;
        _treeViewService = treeViewService;
		_environmentProvider = environmentProvider;
		_jsRuntime = jsRuntime;
		_dispatcher = dispatcher;
    }

	private List<TreeViewNoType> _nodeList = new();
    private TreeViewNamespacePath? _nodeOfViewModel = null;
    
    private LuthetusCommonJavaScriptInteropApi? _jsRuntimeCommonApi;
    
    private LuthetusCommonJavaScriptInteropApi JsRuntimeCommonApi =>
    	_jsRuntimeCommonApi ??= _jsRuntime.GetLuthetusCommonApi();

	public void Initialize()
	{
		// NuGetPackageManagerContext
		{
			_ = ContextFacts.GlobalContext.Keymap.TryRegister(
				new KeymapArgs
				{
					Key = "n",
					Code = "KeyN",
					ShiftKey = false,
					CtrlKey = true,
					AltKey = true,
					MetaKey = false,
					LayerKey = Key<KeymapLayer>.Empty,
				},
				ContextHelper.ConstructFocusContextElementCommand(
					ContextFacts.NuGetPackageManagerContext, "Focus: NuGetPackageManager", "focus-nu-get-package-manager", JsRuntimeCommonApi, _dispatcher));
		}
		// CSharpReplContext
		{
			_ = ContextFacts.GlobalContext.Keymap.TryRegister(
				new KeymapArgs
				{
					Key = "r",
					Code = "KeyR",
					ShiftKey = false,
					CtrlKey = true,
					AltKey = true,
					MetaKey = false,
					LayerKey = Key<KeymapLayer>.Empty,
				},
				ContextHelper.ConstructFocusContextElementCommand(
					ContextFacts.SolutionExplorerContext, "Focus: C# REPL", "focus-c-sharp-repl", JsRuntimeCommonApi, _dispatcher));
		}
		// SolutionExplorerContext
		{
			var focusSolutionExplorerCommand = ContextHelper.ConstructFocusContextElementCommand(
				ContextFacts.SolutionExplorerContext, "Focus: SolutionExplorer", "focus-solution-explorer", JsRuntimeCommonApi, _dispatcher);

			_ = ContextFacts.GlobalContext.Keymap.TryRegister(
					new KeymapArgs
					{
						Key = "s",
						Code = "KeyS",
						ShiftKey = false,
						CtrlKey = true,
						AltKey = true,
						MetaKey = false,
						LayerKey = Key<KeymapLayer>.Empty,
					},
					focusSolutionExplorerCommand);

			// Set active solution explorer tree view node to be the
			// active text editor view model and,
			// Set focus to the solution explorer;
			{
				var focusTextEditorCommand = new CommonCommand(
					"Focus: SolutionExplorer (with text editor view model)", "focus-solution-explorer_with-text-editor-view-model", false,
					async commandArgs =>
					{
						await PerformGetFlattenedTree().ConfigureAwait(false);

						var localNodeOfViewModel = _nodeOfViewModel;

						if (localNodeOfViewModel is null)
							return;

						_treeViewService.SetActiveNode(
							DotNetSolutionState.TreeViewSolutionExplorerStateKey,
							localNodeOfViewModel,
							false,
							false);

						var elementId = _treeViewService.GetNodeElementId(localNodeOfViewModel);

						await focusSolutionExplorerCommand.CommandFunc
							.Invoke(commandArgs)
							.ConfigureAwait(false);
					});

				_ = ContextFacts.GlobalContext.Keymap.TryRegister(
						new KeymapArgs
						{
							Key = "S",
							Code = "KeyS",
							CtrlKey = true,
							ShiftKey = true,
							AltKey = true,
							MetaKey = false,
							LayerKey = Key<KeymapLayer>.Empty,
						},
						focusTextEditorCommand);
			}
		}
	}

    private async Task PerformGetFlattenedTree()
    {
		_nodeList.Clear();

		var group = _textEditorService.GroupApi.GetOrDefault(EditorIdeApi.EditorTextEditorGroupKey);

		if (group is not null)
		{
			var textEditorViewModel = _textEditorService.ViewModelApi.GetOrDefault(group.ActiveViewModelKey);

			if (textEditorViewModel is not null)
			{
				if (_treeViewService.TryGetTreeViewContainer(
						DotNetSolutionState.TreeViewSolutionExplorerStateKey,
						out var treeViewContainer) &&
                    treeViewContainer is not null)
				{
					await RecursiveGetFlattenedTree(treeViewContainer.RootNode, textEditorViewModel).ConfigureAwait(false);
				}
			}
		}
    }

    private async Task RecursiveGetFlattenedTree(
        TreeViewNoType treeViewNoType,
        TextEditorViewModel textEditorViewModel)
    {
        _nodeList.Add(treeViewNoType);

        if (treeViewNoType is TreeViewNamespacePath treeViewNamespacePath)
        {
            if (textEditorViewModel is not null)
            {
                var viewModelAbsolutePath = _environmentProvider.AbsolutePathFactory(
                    textEditorViewModel.ResourceUri.Value,
                    false);

                if (viewModelAbsolutePath.Value ==
                        treeViewNamespacePath.Item.AbsolutePath.Value)
                {
                    _nodeOfViewModel = treeViewNamespacePath;
                }
            }

            switch (treeViewNamespacePath.Item.AbsolutePath.ExtensionNoPeriod)
            {
                case ExtensionNoPeriodFacts.C_SHARP_PROJECT:
                    await treeViewNamespacePath.LoadChildListAsync().ConfigureAwait(false);
                    break;
            }
        }

        await treeViewNoType.LoadChildListAsync().ConfigureAwait(false);

        foreach (var node in treeViewNoType.ChildList)
        {
            await RecursiveGetFlattenedTree(node, textEditorViewModel).ConfigureAwait(false);
        }
    }
}
