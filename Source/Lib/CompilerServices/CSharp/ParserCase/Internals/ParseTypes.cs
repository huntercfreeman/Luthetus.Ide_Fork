using System.Collections.Immutable;
using Luthetus.TextEditor.RazorLib.CompilerServices.Syntax;
using Luthetus.TextEditor.RazorLib.CompilerServices.Syntax.Nodes;
using Luthetus.TextEditor.RazorLib.CompilerServices.Syntax.Tokens;

namespace Luthetus.CompilerServices.CSharp.ParserCase.Internals;

public static class ParseTypes
{
    public static void HandleStaticClassIdentifier(
        IdentifierToken consumedIdentifierToken,
        CSharpParserModel model)
    {
        // The identifier token was already consumed, so a Backtrack() is needed.
        model.TokenWalker.Backtrack();
        model.SyntaxStack.Push(MatchTypeClause(model));
    }

    public static void HandleUndefinedTypeOrNamespaceReference(
        IdentifierToken consumedIdentifierToken,
        CSharpParserModel model)
    {
        var identifierReferenceNode = new AmbiguousIdentifierNode(consumedIdentifierToken);

        model.Binder.BindTypeIdentifier(consumedIdentifierToken, model);

        model.DiagnosticBag.ReportUndefinedTypeOrNamespace(
            consumedIdentifierToken.TextSpan,
            consumedIdentifierToken.TextSpan.GetText());

        model.SyntaxStack.Push(identifierReferenceNode);
    }

    /// <summary>
    /// This method is used for generic type definition such as, 'class List&lt;T&gt; { ... }'
    /// </summary>
    public static void HandleGenericArguments(
        OpenAngleBracketToken consumedOpenAngleBracketToken,
        CSharpParserModel model)
    {
        if (SyntaxKind.CloseAngleBracketToken == model.TokenWalker.Current.SyntaxKind)
        {
            model.SyntaxStack.Push(new GenericArgumentsListingNode(
                consumedOpenAngleBracketToken,
                ImmutableArray<GenericArgumentEntryNode>.Empty,
                (CloseAngleBracketToken)model.TokenWalker.Consume()));

            return;
        }

        var mutableGenericArgumentsListing = new List<GenericArgumentEntryNode>();

        while (true)
        {
            // TypeClause
            var typeClauseNode = MatchTypeClause(model);

            if (typeClauseNode.IsFabricated)
                break;

            var genericArgumentEntryNode = new GenericArgumentEntryNode(typeClauseNode);
            mutableGenericArgumentsListing.Add(genericArgumentEntryNode);

            if (SyntaxKind.CommaToken == model.TokenWalker.Current.SyntaxKind)
            {
                var commaToken = (CommaToken)model.TokenWalker.Consume();

                // TODO: Track comma tokens?
                //
                // functionArgumentListing.Add(commaToken);
            }
            else
            {
                break;
            }
        }

        var closeAngleBracketToken = (CloseAngleBracketToken)model.TokenWalker.Match(SyntaxKind.CloseAngleBracketToken);

        model.SyntaxStack.Push(new GenericArgumentsListingNode(
            consumedOpenAngleBracketToken,
            mutableGenericArgumentsListing.ToImmutableArray(),
            closeAngleBracketToken));
    }

    public static void HandleAttribute(
        OpenSquareBracketToken consumedOpenSquareBracketToken,
        CSharpParserModel model)
    {
        // Suppress unused variable warning
        _ = consumedOpenSquareBracketToken;

        if (SyntaxKind.CloseSquareBracketToken == model.TokenWalker.Current.SyntaxKind)
        {
            var closeSquareBracketToken = (CloseSquareBracketToken)model.TokenWalker.Consume();

            model.DiagnosticBag.ReportTodoException(
                closeSquareBracketToken.TextSpan,
                "An identifier was expected.");

            return;
        }

        while (true)
        {
            var identifierToken = (IdentifierToken)model.TokenWalker.Match(SyntaxKind.IdentifierToken);
            model.Binder.BindTypeIdentifier(identifierToken, model);

            if (identifierToken.IsFabricated && SyntaxKind.CommaToken != model.TokenWalker.Current.SyntaxKind)
                break;

            if (model.TokenWalker.Current.SyntaxKind == SyntaxKind.CommaToken)
            {
                var commaToken = (CommaToken)model.TokenWalker.Consume();
                // TODO: Track comma tokens?
            }
            else
            {
                break;
            }
        }
    }

	/// <summary>
	/// This method should only be used to disambiguate syntax.
	/// If it is known that there has to be a TypeClauseNode at the current position,
	/// then use <see cref="MatchTypeClause"/>.
	///
	/// Example: 'someMethod(out var e);'
	///           In this example the 'out' can continue into a variable reference or declaration.
	///           Therefore an invocation to this method is performed to determine if it is a declaration.
	/// 
	/// Furthermore, because there is a need to disambiguate, more checks are performed in this method
	/// than in <see cref="MatchTypeClause"/>.
	/// </summary>
	public static bool IsPossibleTypeClause(ISyntaxToken syntaxToken, CSharpParserModel model)
	{
		if (UtilityApi.IsKeywordSyntaxKind(syntaxToken.SyntaxKind) &&
                (UtilityApi.IsTypeIdentifierKeywordSyntaxKind(syntaxToken.SyntaxKind) ||
                UtilityApi.IsVarContextualKeyword(model, syntaxToken.SyntaxKind)))
        {
            return true;
        }
        
        if (syntaxToken.SyntaxKind == SyntaxKind.IdentifierToken)
        {
        	var text = syntaxToken.TextSpan.GetText();
        
        	if (model.Binder.TryGetVariableDeclarationHierarchically(
	        		model,
	                model.BinderSession.ResourceUri,
	                model.BinderSession.CurrentScopeIndexKey,
	                text,
	                out var variableDeclarationNode)
                && variableDeclarationNode is not null)
            {
            	return false;
            }
            
            if (model.Binder.TryGetFunctionHierarchically(
	        		model,
	                model.BinderSession.ResourceUri,
	                model.BinderSession.CurrentScopeIndexKey,
	                text,
	                out var functionDefinitionNode)
                && functionDefinitionNode is not null)
            {
            	return false;
            }
            
        	return true;
        }
        
        return false;
	}

    public static TypeClauseNode MatchTypeClause(CSharpParserModel model)
    {
        ISyntaxToken syntaxToken;
		
		if (UtilityApi.IsKeywordSyntaxKind(model.TokenWalker.Current.SyntaxKind) &&
                (UtilityApi.IsTypeIdentifierKeywordSyntaxKind(model.TokenWalker.Current.SyntaxKind) ||
                UtilityApi.IsVarContextualKeyword(model, model.TokenWalker.Current.SyntaxKind)))
		{
            syntaxToken = model.TokenWalker.Consume();
        }
        else
        {
            syntaxToken = model.TokenWalker.Match(SyntaxKind.IdentifierToken);
        }

        var typeClauseNode = new TypeClauseNode(
            syntaxToken,
            null,
            null);

        model.Binder.BindTypeClauseNode(typeClauseNode, model);

        if (model.TokenWalker.Current.SyntaxKind == SyntaxKind.OpenAngleBracketToken)
        {
            Console.WriteLine("model.TokenWalker.Current.SyntaxKind == SyntaxKind.OpenAngleBracketToken");
        }
        
        if (model.TokenWalker.Current.SyntaxKind == SyntaxKind.QuestionMarkToken)
        {
        	typeClauseNode.HasQuestionMark = true;
        	_ = model.TokenWalker.Consume();
		}
        
        while (model.TokenWalker.Current.SyntaxKind == SyntaxKind.OpenSquareBracketToken)
        {
            var openSquareBracketToken = model.TokenWalker.Consume();
            var closeSquareBracketToken = model.TokenWalker.Match(SyntaxKind.CloseSquareBracketToken);

            var arraySyntaxTokenTextSpan = syntaxToken.TextSpan with
            {
                EndingIndexExclusive = closeSquareBracketToken.TextSpan.EndingIndexExclusive
            };

            var arraySyntaxToken = new ArraySyntaxToken(arraySyntaxTokenTextSpan);
            var genericParameterEntryNode = new GenericParameterEntryNode(typeClauseNode);

            var genericParametersListingNode = new GenericParametersListingNode(
                new OpenAngleBracketToken(openSquareBracketToken.TextSpan)
                {
                    IsFabricated = true
                },
                new List<GenericParameterEntryNode> { genericParameterEntryNode },
                new CloseAngleBracketToken(closeSquareBracketToken.TextSpan)
                {
                    IsFabricated = true
                });

            return new TypeClauseNode(
                arraySyntaxToken,
                null,
                genericParametersListingNode);

            // TODO: Implement multidimensional arrays. This array logic always returns after finding the first array syntax.
        }

        return typeClauseNode;
    }

    public static void HandlePrimaryConstructorDefinition(
        TypeDefinitionNode typeDefinitionNode,
        OpenParenthesisToken consumedOpenParenthesisToken,
        CSharpParserModel model)
    {
        ParseFunctions.HandleFunctionArguments(consumedOpenParenthesisToken, model);
        var functionArgumentsListingNode = (FunctionArgumentsListingNode)model.SyntaxStack.Pop();

        typeDefinitionNode.SetPrimaryConstructorFunctionArgumentsListingNode(functionArgumentsListingNode);

		model.SyntaxStack.Push(typeDefinitionNode);
		model.CurrentCodeBlockBuilder.PendingChild = typeDefinitionNode;
    }
}
