using Luthetus.TextEditor.RazorLib.CompilerServices.Interfaces;
using Luthetus.TextEditor.RazorLib.CompilerServices.Syntax;
using Luthetus.TextEditor.RazorLib.CompilerServices.Syntax.Nodes;
using Luthetus.TextEditor.RazorLib.CompilerServices.Syntax.Nodes.Enums;
using Luthetus.TextEditor.RazorLib.CompilerServices.Syntax.Nodes.Interfaces;
using Luthetus.TextEditor.RazorLib.CompilerServices.Syntax.Tokens;
using Luthetus.CompilerServices.CSharp.Facts;

namespace Luthetus.CompilerServices.CSharp.ParserCase.Internals;

public static class ParseVariables
{
    public static void HandleVariableReference(
        IdentifierToken consumedIdentifierToken,
        CSharpCompilationUnit compilationUnit)
    {
    }

	/// <summary>Function invocation which uses the 'out' keyword.</summary>
    public static IVariableDeclarationNode? HandleVariableDeclarationExpression(
        TypeClauseNode consumedTypeClauseNode,
        IdentifierToken consumedIdentifierToken,
        VariableKind variableKind,
        CSharpCompilationUnit compilationUnit)
    {
    	IVariableDeclarationNode variableDeclarationNode;

		variableDeclarationNode = new VariableDeclarationNode(
	        consumedTypeClauseNode,
	        consumedIdentifierToken,
	        variableKind,
	        false);

        compilationUnit.ParserModel.Binder.BindVariableDeclarationNode(variableDeclarationNode, model);
        compilationUnit.ParserModel.CurrentCodeBlockBuilder.ChildList.Add(variableDeclarationNode);
        return variableDeclarationNode;
    }
    
    /// <summary>
    /// TODO: This method should return the 'VariableDeclarationNode?' just the same as <see cref="HandleVariableDeclarationExpression"/>
    /// </summary>
    public static void HandleVariableDeclarationStatement(
        TypeClauseNode consumedTypeClauseNode,
        IdentifierToken consumedIdentifierToken,
        VariableKind variableKind,
        CSharpCompilationUnit compilationUnit)
    {
    	var variableDeclarationNode = HandleVariableDeclarationExpression(
			consumedTypeClauseNode,
	        consumedIdentifierToken,
	        variableKind,
	        model);
	        
	    if (variableDeclarationNode is null)
	    	return;
	    	
	    // if (variableKind == VariableKind.Local || variableKind == VariableKind.Closure)

        if (compilationUnit.ParserModel.TokenWalker.Current.SyntaxKind == SyntaxKind.EqualsToken)
        {
            if (compilationUnit.ParserModel.TokenWalker.Peek(1).SyntaxKind == SyntaxKind.CloseAngleBracketToken)
            {
                HandlePropertyExpression(
                    variableDeclarationNode,
                    (EqualsToken)compilationUnit.ParserModel.TokenWalker.Consume(),
                    (CloseAngleBracketToken)compilationUnit.ParserModel.TokenWalker.Consume(),
                    (CSharpParserModel)model);
            }
            else
            {
                // Variable initialization occurs here.
                HandleVariableAssignment(
                    consumedIdentifierToken,
                    (EqualsToken)compilationUnit.ParserModel.TokenWalker.Consume(),
                    (CSharpParserModel)model);
            }
        }
        else
        {
            if (variableDeclarationNode.TypeClauseNode.TypeIdentifierToken.SyntaxKind ==
                SyntaxKind.VarTokenContextualKeyword)
            {
                compilationUnit.ParserModel.DiagnosticBag.ReportImplicitlyTypedVariablesMustBeInitialized(
                    consumedIdentifierToken.TextSpan);
            }
        }

        if (variableKind == VariableKind.Property &&
            compilationUnit.ParserModel.TokenWalker.Current.SyntaxKind == SyntaxKind.OpenBraceToken)
        {
            HandlePropertyDeclaration(
                variableDeclarationNode,
                (OpenBraceToken)compilationUnit.ParserModel.TokenWalker.Consume(),
                (CSharpParserModel)model);
        }
        else
        {
            _ = compilationUnit.ParserModel.TokenWalker.Match(SyntaxKind.StatementDelimiterToken);
        }
    }

    public static void HandlePropertyDeclaration(
        IVariableDeclarationNode consumedVariableDeclarationNode,
        OpenBraceToken consumedOpenBraceToken,
        CSharpCompilationUnit compilationUnit)
    {
    }

    public static void HandlePropertyExpression(
        IVariableDeclarationNode consumedVariableDeclarationNode,
        EqualsToken consumedEqualsToken,
        CloseAngleBracketToken consumedCloseAngleBracketToken,
        CSharpCompilationUnit compilationUnit)
    {
    }

    public static void HandleVariableAssignment(
        IdentifierToken consumedIdentifierToken,
        EqualsToken consumedEqualsToken,
        CSharpCompilationUnit compilationUnit)
    {
    }
}
