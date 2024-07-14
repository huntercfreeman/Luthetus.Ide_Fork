using Fluxor;
using Luthetus.Common.RazorLib.Commands.Models;
using Luthetus.Common.RazorLib.TreeViews.Models;
using Luthetus.Common.RazorLib.BackgroundTasks.Models;
using Luthetus.Common.RazorLib.ComponentRenderers.Models;
using Luthetus.Common.RazorLib.Notifications.Models;
using Luthetus.Common.RazorLib.Keys.Models;
using Luthetus.TextEditor.RazorLib;
using Luthetus.TextEditor.RazorLib.TextEditors.Models;
using Luthetus.TextEditor.RazorLib.CompilerServices.Interfaces;
using Luthetus.TextEditor.RazorLib.BackgroundTasks.Models;
using Luthetus.TextEditor.RazorLib.Lexes.Models;
using Luthetus.TextEditor.RazorLib.Installations.Models;
using Luthetus.TextEditor.RazorLib.Groups.Models;
using Luthetus.CompilerServices.Lang.CSharp.BinderCase;
using Luthetus.Ide.RazorLib.BackgroundTasks.Models;
using Luthetus.Ide.RazorLib.Namespaces.Models;
using Luthetus.Ide.RazorLib.TestExplorers.Models;
using Luthetus.Ide.RazorLib.CompilerServices.Models;

namespace Luthetus.Ide.RazorLib.TestExplorers.Models;

public static class TestExplorerHelper
{
	/// <summary>
    /// TODO: D.R.Y.: This method is copy and pasted, then altered a bit, from
    /// <see cref="Luthetus.TextEditor.RazorLib.Commands.Models.Defaults.TextEditorCommandDefaultFunctions.GoToDefinitionFactory"/>.
    /// </summary>
    public static TextEditorEdit ShowTestInEditorFactory(
		string className,
		ICommonComponentRenderers commonComponentRenderers,
        IDispatcher dispatcher,
        ICompilerServiceRegistry compilerServiceRegistry,
        ITextEditorService textEditorService,
        IServiceProvider serviceProvider)
    {
    	return (IEditContext editContext) =>
        {
			var wordTextSpan = TextEditorTextSpan.FabricateTextSpan(className);
			
			if (compilerServiceRegistry is not CompilerServiceRegistry)
			{
				NotificationHelper.DispatchInformative(
			        nameof(TestExplorerTreeViewMouseEventHandler),
			        $"Could not open in editor because compilerServiceRegistry was not the type: 'CompilerServiceRegistry'; it was '{compilerServiceRegistry.GetType().Name}'",
			        commonComponentRenderers,
			        dispatcher,
			        TimeSpan.FromSeconds(5));
			
				return Task.CompletedTask;
			}
			
			var cSharpCompilerService = ((CompilerServiceRegistry)compilerServiceRegistry).CSharpCompilerService;
			var cSharpBinder = (CSharpBinder)cSharpCompilerService.Binder;
			
			// ImmutableDictionary<NamespaceAndTypeIdentifiers, TypeDefinitionNode>
			var allTypeDefinitions = cSharpBinder.AllTypeDefinitions;
			
			var typeDefinitionNodeList = allTypeDefinitions.Where(kvp => kvp.Key.TypeIdentifier == className)
				.ToList();
				
			if (typeDefinitionNodeList.Count == 0)
			{
				NotificationHelper.DispatchInformative(
			        nameof(TestExplorerTreeViewMouseEventHandler),
			        $"Could not open in editor because typeDefinitionNodeList.Count == 0",
			        commonComponentRenderers,
			        dispatcher,
			        TimeSpan.FromSeconds(5));
            
                return Task.CompletedTask;
			}

            var definitionTextSpan = typeDefinitionNodeList.First().Value.TypeIdentifierToken.TextSpan;

            if (definitionTextSpan is null)
            {
            	NotificationHelper.DispatchInformative(
			        nameof(TestExplorerTreeViewMouseEventHandler),
			        $"Could not open in editor because definitionTextSpan was null",
			        commonComponentRenderers,
			        dispatcher,
			        TimeSpan.FromSeconds(5));
            
                return Task.CompletedTask;
            }

            var definitionModel = textEditorService.ModelApi.GetOrDefault(definitionTextSpan.ResourceUri);

            if (definitionModel is null)
            {
                if (textEditorService.TextEditorConfig.RegisterModelFunc is not null)
                {
                    textEditorService.TextEditorConfig.RegisterModelFunc.Invoke(
                        new RegisterModelArgs(definitionTextSpan.ResourceUri, serviceProvider));

                    var definitionModelModifier = editContext.GetModelModifier(definitionTextSpan.ResourceUri);

                    if (definitionModel is null)
                    {
                    	NotificationHelper.DispatchInformative(
					        nameof(TestExplorerTreeViewMouseEventHandler),
					        $"Could not open in editor because definitionModel was null",
					        commonComponentRenderers,
					        dispatcher,
					        TimeSpan.FromSeconds(5));
                    
                        return Task.CompletedTask;
                    }
                }
                else
                {
                	NotificationHelper.DispatchInformative(
				        nameof(TestExplorerTreeViewMouseEventHandler),
				        $"Could not open in editor because textEditorService.TextEditorConfig.RegisterModelFunc was null",
				        commonComponentRenderers,
				        dispatcher,
				        TimeSpan.FromSeconds(5));
                
                    return Task.CompletedTask;
                }
            }

            var definitionViewModels = textEditorService.ModelApi.GetViewModelsOrEmpty(definitionTextSpan.ResourceUri);

            if (!definitionViewModels.Any())
            {
                if (textEditorService.TextEditorConfig.TryRegisterViewModelFunc is not null)
                {
                    textEditorService.TextEditorConfig.TryRegisterViewModelFunc.Invoke(new TryRegisterViewModelArgs(
                        Key<TextEditorViewModel>.NewKey(),
                        definitionTextSpan.ResourceUri,
                        new Category("main"),
                        true,
                        serviceProvider));

                    definitionViewModels = textEditorService.ModelApi.GetViewModelsOrEmpty(definitionTextSpan.ResourceUri);

                    if (!definitionViewModels.Any())
                    {
                    	NotificationHelper.DispatchInformative(
					        nameof(TestExplorerTreeViewMouseEventHandler),
					        $"Could not open in editor because !definitionViewModels.Any()",
					        commonComponentRenderers,
					        dispatcher,
					        TimeSpan.FromSeconds(5));
					        
                        return Task.CompletedTask;
                    }
                }
                else
                {
                	NotificationHelper.DispatchInformative(
				        nameof(TestExplorerTreeViewMouseEventHandler),
				        $"Could not open in editor because textEditorService.TextEditorConfig.TryRegisterViewModelFunc was null",
				        commonComponentRenderers,
				        dispatcher,
				        TimeSpan.FromSeconds(5));
                
                    return Task.CompletedTask;
                }
            }

            var definitionViewModelKey = definitionViewModels.First().ViewModelKey;
            
            var definitionViewModelModifier = editContext.GetViewModelModifier(definitionViewModelKey);
            var definitionCursorModifierBag = editContext.GetCursorModifierBag(definitionViewModelModifier?.ViewModel);
            var definitionPrimaryCursorModifier = editContext.GetPrimaryCursorModifier(definitionCursorModifierBag);

            if (definitionViewModelModifier is null || definitionCursorModifierBag is null || definitionPrimaryCursorModifier is null)
            {
            	NotificationHelper.DispatchInformative(
			        nameof(TestExplorerTreeViewMouseEventHandler),
			        $"Could not open in editor because definitionViewModelModifier was null || definitionCursorModifierBag was null || definitionPrimaryCursorModifier was null",
			        commonComponentRenderers,
			        dispatcher,
			        TimeSpan.FromSeconds(5));
				        
                return Task.CompletedTask;
            }

            var rowData = definitionModel.GetLineInformationFromPositionIndex(definitionTextSpan.StartingIndexInclusive);
            var columnIndex = definitionTextSpan.StartingIndexInclusive - rowData.StartPositionIndexInclusive;

            definitionPrimaryCursorModifier.SelectionAnchorPositionIndex = null;
            definitionPrimaryCursorModifier.LineIndex = rowData.Index;
            definitionPrimaryCursorModifier.ColumnIndex = columnIndex;
            definitionPrimaryCursorModifier.PreferredColumnIndex = columnIndex;

            if (textEditorService.TextEditorConfig.TryShowViewModelFunc is not null)
            {
                textEditorService.TextEditorConfig.TryShowViewModelFunc.Invoke(new TryShowViewModelArgs(
                    definitionViewModelKey,
                    Key<TextEditorGroup>.Empty,
                    serviceProvider));
            }
            else
            {
            	NotificationHelper.DispatchInformative(
			        nameof(TestExplorerTreeViewMouseEventHandler),
			        $"Could not open in editor because textEditorService.TextEditorConfig.TryShowViewModelFunc was null",
			        commonComponentRenderers,
			        dispatcher,
			        TimeSpan.FromSeconds(5));
            }

            return Task.CompletedTask;
        };
    }
}
