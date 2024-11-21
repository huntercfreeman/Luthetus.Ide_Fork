using Microsoft.AspNetCore.Components;
using Luthetus.Common.RazorLib.Keys.Models;
using Luthetus.TextEditor.RazorLib;
using Luthetus.TextEditor.RazorLib.TextEditors.Models;
using Luthetus.TextEditor.RazorLib.TextEditors.Models.Internals;
using Luthetus.TextEditor.RazorLib.Lexers.Models;
using Luthetus.TextEditor.RazorLib.CompilerServices.Syntax;
using Luthetus.TextEditor.RazorLib.CompilerServices.Syntax.Nodes;
using Luthetus.TextEditor.RazorLib.CompilerServices.Syntax.Symbols;

namespace Luthetus.TextEditor.RazorLib.TextEditors.Displays.Internals.Symbols;

public partial class TypeSyntaxDisplay : ComponentBase
{
	[Inject]
	private ITextEditorService TextEditorService { get; set; } = null!;
	
	[Parameter, EditorRequired]
	public SyntaxViewModel SyntaxViewModel { get; set; } = default!;
	
	private Task HandleOnClick(SyntaxViewModel syntaxViewModelLocal)
	{
		if (syntaxViewModelLocal.DefinitionNode is null ||
			syntaxViewModelLocal.DefinitionNode.SyntaxKind != SyntaxKind.TypeDefinitionNode)
		{
			return Task.CompletedTask;
		}
		
		var typeDefinitionNode = (TypeDefinitionNode)syntaxViewModelLocal.DefinitionNode;
	
		return TextEditorService.OpenInEditorAsync(
			typeDefinitionNode.TypeIdentifierToken.TextSpan.ResourceUri.Value,
			true,
			typeDefinitionNode.TypeIdentifierToken.TextSpan.StartingIndexInclusive,
			new Category("main"),
			Key<TextEditorViewModel>.NewKey());
	}
}