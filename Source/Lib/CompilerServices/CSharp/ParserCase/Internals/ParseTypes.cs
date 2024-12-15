using System.Collections.Immutable;
using Luthetus.TextEditor.RazorLib.CompilerServices;
using Luthetus.TextEditor.RazorLib.CompilerServices.Syntax;
using Luthetus.TextEditor.RazorLib.CompilerServices.Syntax.Nodes;
using Luthetus.TextEditor.RazorLib.CompilerServices.Syntax.Tokens;
using Luthetus.CompilerServices.CSharp.CompilerServiceCase;

namespace Luthetus.CompilerServices.CSharp.ParserCase.Internals;

public static class ParseTypes
{
    public static void HandleStaticClassIdentifier(
        IdentifierToken consumedIdentifierToken,
        CSharpCompilationUnit compilationUnit,
        ref CSharpParserModel parserModel)
    {
    }

    public static void HandleUndefinedTypeOrNamespaceReference(
        IdentifierToken consumedIdentifierToken,
        CSharpCompilationUnit compilationUnit,
        ref CSharpParserModel parserModel)
    {
    }

    /// <summary>
    /// This method is used for generic type definition such as, 'class List&lt;T&gt; { ... }'
    /// </summary>
    public static GenericArgumentsListingNode HandleGenericArguments(CSharpCompilationUnit compilationUnit, ref CSharpParserModel parserModel)
    {
    	var openAngleBracketToken = (OpenAngleBracketToken)parserModel.TokenWalker.Consume();
    
    	if (SyntaxKind.CloseAngleBracketToken == parserModel.TokenWalker.Current.SyntaxKind)
        {
            return new GenericArgumentsListingNode(
                openAngleBracketToken,
                ImmutableArray<GenericArgumentEntryNode>.Empty,
                (CloseAngleBracketToken)parserModel.TokenWalker.Consume());
        }

        var mutableGenericArgumentsListing = new List<GenericArgumentEntryNode>();

        while (true)
        {
            // TypeClause
            var typeClauseNode = MatchTypeClause(compilationUnit, ref parserModel);

            if (typeClauseNode.IsFabricated)
                break;

            var genericArgumentEntryNode = new GenericArgumentEntryNode(typeClauseNode);
            mutableGenericArgumentsListing.Add(genericArgumentEntryNode);

            if (SyntaxKind.CommaToken == parserModel.TokenWalker.Current.SyntaxKind)
            {
                var commaToken = (CommaToken)parserModel.TokenWalker.Consume();

                // TODO: Track comma tokens?
                //
                // functionArgumentListing.Add(commaToken);
            }
            else
            {
                break;
            }
        }

        var closeAngleBracketToken = (CloseAngleBracketToken)parserModel.TokenWalker.Match(SyntaxKind.CloseAngleBracketToken);

        return new GenericArgumentsListingNode(
            openAngleBracketToken,
            mutableGenericArgumentsListing.ToImmutableArray(),
            closeAngleBracketToken);
    }

    public static void HandleAttribute(
        OpenSquareBracketToken consumedOpenSquareBracketToken,
        CSharpCompilationUnit compilationUnit,
        ref CSharpParserModel parserModel)
    {
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
	public static bool IsPossibleTypeClause(ISyntaxToken syntaxToken, CSharpCompilationUnit compilationUnit, ref CSharpParserModel parserModel)
	{
		return false;
	}

    public static TypeClauseNode MatchTypeClause(CSharpCompilationUnit compilationUnit, ref CSharpParserModel parserModel)
    {
    	if (ParseOthers.TryParseExpression(SyntaxKind.TypeClauseNode, compilationUnit, ref parserModel, out var expressionNode))
    	{
    		return (TypeClauseNode)expressionNode;
    	}
    	else
    	{
    		var syntaxToken = (IdentifierToken)parserModel.TokenWalker.Match(SyntaxKind.IdentifierToken);
    		
    		return new TypeClauseNode(
	            syntaxToken,
	            null,
	            null);
    	}
    	
        /*ISyntaxToken syntaxToken;
		
		if (UtilityApi.IsKeywordSyntaxKind(parserModel.TokenWalker.Current.SyntaxKind) &&
                (UtilityApi.IsTypeIdentifierKeywordSyntaxKind(parserModel.TokenWalker.Current.SyntaxKind) ||
                UtilityApi.IsVarContextualKeyword(compilationUnit, parserModel.TokenWalker.Current.SyntaxKind)))
		{
            syntaxToken = parserModel.TokenWalker.Consume();
        }
        else
        {
            syntaxToken = parserModel.TokenWalker.Match(SyntaxKind.IdentifierToken);
        }

        var typeClauseNode = new TypeClauseNode(
            syntaxToken,
            null,
            null);

        parserModel.Binder.BindTypeClauseNode(typeClauseNode, compilationUnit);

        if (parserModel.TokenWalker.Current.SyntaxKind == SyntaxKind.OpenAngleBracketToken)
        {
        	var genericParametersListingNode = (GenericParametersListingNode)ParseOthers.Force_ParseExpression(
        		SyntaxKind.GenericParametersListingNode,
        		compilationUnit);
        		
            typeClauseNode.SetGenericParametersListingNode(genericParametersListingNode);
        }
        
        if (parserModel.TokenWalker.Current.SyntaxKind == SyntaxKind.QuestionMarkToken)
        {
        	typeClauseNode.HasQuestionMark = true;
        	_ = parserModel.TokenWalker.Consume();
		}
        
        while (parserModel.TokenWalker.Current.SyntaxKind == SyntaxKind.OpenSquareBracketToken)
        {
            var openSquareBracketToken = parserModel.TokenWalker.Consume();
            var closeSquareBracketToken = parserModel.TokenWalker.Match(SyntaxKind.CloseSquareBracketToken);

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
        */
    }

    public static void HandlePrimaryConstructorDefinition(
        TypeDefinitionNode typeDefinitionNode,
        OpenParenthesisToken consumedOpenParenthesisToken,
        CSharpCompilationUnit compilationUnit,
        ref CSharpParserModel parserModel)
    {
    }
}
