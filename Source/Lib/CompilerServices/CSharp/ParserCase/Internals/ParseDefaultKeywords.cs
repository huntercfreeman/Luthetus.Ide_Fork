using System.Collections.Immutable;
using Luthetus.TextEditor.RazorLib.CompilerServices.Syntax;
using Luthetus.TextEditor.RazorLib.CompilerServices.Syntax.Nodes;
using Luthetus.TextEditor.RazorLib.CompilerServices.Syntax.Nodes.Enums;
using Luthetus.TextEditor.RazorLib.CompilerServices.Syntax.Nodes.Interfaces;
using Luthetus.TextEditor.RazorLib.CompilerServices.Syntax.Tokens;
using Luthetus.CompilerServices.CSharp.Facts;

namespace Luthetus.CompilerServices.CSharp.ParserCase.Internals;

public class ParseDefaultKeywords
{
    public static void HandleAsTokenKeyword(CSharpParserModel model)
    {
    	var keywordToken = model.TokenWalker.Consume();
        // TODO: Implement this method
    }

    public static void HandleBaseTokenKeyword(CSharpParserModel model)
    {
    	var keywordToken = model.TokenWalker.Consume();
        // TODO: Implement this method
    }

    public static void HandleBoolTokenKeyword(CSharpParserModel model)
    {
        HandleTypeIdentifierKeyword(model);
    }

    public static void HandleBreakTokenKeyword(CSharpParserModel model)
    {
    	var keywordToken = model.TokenWalker.Consume();
        // TODO: Implement this method
    }

    public static void HandleByteTokenKeyword(CSharpParserModel model)
    {
        HandleTypeIdentifierKeyword(model);
    }

    public static void HandleCaseTokenKeyword(CSharpParserModel model)
    {
    	model.ExpressionList.Add((SyntaxKind.ColonToken, null));
		var expressionNode = ParseOthers.ParseExpression(model);
	    var colonToken = (ColonToken)model.TokenWalker.Match(SyntaxKind.ColonToken);
    }

    public static void HandleCatchTokenKeyword(CSharpParserModel model)
    {
    	var catchKeywordToken = (KeywordToken)model.TokenWalker.Consume();
    }

    public static void HandleCharTokenKeyword(CSharpParserModel model)
    {
        HandleTypeIdentifierKeyword(model);
    }

    public static void HandleCheckedTokenKeyword(CSharpParserModel model)
    {
    	var keywordToken = model.TokenWalker.Consume();
        // TODO: Implement this method
    }

    public static void HandleConstTokenKeyword(CSharpParserModel model)
    {
    	var keywordToken = model.TokenWalker.Consume();
        // TODO: Implement this method
    }

    public static void HandleContinueTokenKeyword(CSharpParserModel model)
    {
    	var keywordToken = model.TokenWalker.Consume();
        // TODO: Implement this method
    }

    public static void HandleDecimalTokenKeyword(CSharpParserModel model)
    {
        HandleTypeIdentifierKeyword(model);
    }

    public static void HandleDefaultTokenKeyword(CSharpParserModel model)
    {
    	// Switch statement default case.
        if (model.TokenWalker.Next.SyntaxKind == SyntaxKind.ColonToken)
        	_ = model.TokenWalker.Consume();
    }

    public static void HandleDelegateTokenKeyword(CSharpParserModel model)
    {
        HandleTypeIdentifierKeyword(model);
    }

    public static void HandleDoTokenKeyword(CSharpParserModel model)
    {
    	var doKeywordToken = (KeywordToken)model.TokenWalker.Consume();
    }

    public static void HandleDoubleTokenKeyword(CSharpParserModel model)
    {
        HandleTypeIdentifierKeyword(model);
    }

    public static void HandleElseTokenKeyword(CSharpParserModel model)
    {
    	var keywordToken = model.TokenWalker.Consume();
        // TODO: Implement this method
    }

    public static void HandleEnumTokenKeyword(CSharpParserModel model)
    {
        HandleStorageModifierTokenKeyword(model);

        // Why was this method invocation here? (2024-01-23)
        //
        // HandleTypeIdentifierKeyword(model);
    }

    public static void HandleEventTokenKeyword(CSharpParserModel model)
    {
    	var keywordToken = model.TokenWalker.Consume();
        // TODO: Implement this method
    }

    public static void HandleExplicitTokenKeyword(CSharpParserModel model)
    {
    	var keywordToken = model.TokenWalker.Consume();
        // TODO: Implement this method
    }

    public static void HandleExternTokenKeyword(CSharpParserModel model)
    {
    	var keywordToken = model.TokenWalker.Consume();
        // TODO: Implement this method
    }

    public static void HandleFalseTokenKeyword(CSharpParserModel model)
    {
    	var expressionNode = ParseOthers.ParseExpression(model);
    	model.StatementBuilderStack.Push(expressionNode);
    }

    public static void HandleFinallyTokenKeyword(CSharpParserModel model)
    {
    	var finallyKeywordToken = (KeywordToken)model.TokenWalker.Consume();
    }

    public static void HandleFixedTokenKeyword(CSharpParserModel model)
    {
    	var keywordToken = model.TokenWalker.Consume();
        // TODO: Implement this method
    }

    public static void HandleFloatTokenKeyword(CSharpParserModel model)
    {
        HandleTypeIdentifierKeyword(model);
    }

    public static void HandleForTokenKeyword(CSharpParserModel model)
    {
    	var forKeywordToken = (KeywordToken)model.TokenWalker.Consume();
    }

    public static void HandleForeachTokenKeyword(CSharpParserModel model)
    {
    	var foreachKeywordToken = (KeywordToken)model.TokenWalker.Consume();
    }

    public static void HandleGotoTokenKeyword(CSharpParserModel model)
    {
    	var keywordToken = model.TokenWalker.Consume();
        // TODO: Implement this method
    }

    public static void HandleImplicitTokenKeyword(CSharpParserModel model)
    {
    	var keywordToken = model.TokenWalker.Consume();
        // TODO: Implement this method
    }

    public static void HandleInTokenKeyword(CSharpParserModel model)
    {
    	var keywordToken = model.TokenWalker.Consume();
        // TODO: Implement this method
    }

    public static void HandleIntTokenKeyword(CSharpParserModel model)
    {
        HandleTypeIdentifierKeyword(model);
    }

    public static void HandleIsTokenKeyword(CSharpParserModel model)
    {
    	var keywordToken = model.TokenWalker.Consume();
        // TODO: Implement this method
    }

    public static void HandleLockTokenKeyword(CSharpParserModel model)
    {
    	var lockKeywordToken = (KeywordToken)model.TokenWalker.Consume();
    }

    public static void HandleLongTokenKeyword(CSharpParserModel model)
    {
        HandleTypeIdentifierKeyword(model);
    }

    public static void HandleNullTokenKeyword(CSharpParserModel model)
    {
        HandleTypeIdentifierKeyword(model);
    }

    public static void HandleObjectTokenKeyword(CSharpParserModel model)
    {
        HandleTypeIdentifierKeyword(model);
    }

    public static void HandleOperatorTokenKeyword(CSharpParserModel model)
    {
    	var keywordToken = model.TokenWalker.Consume();
        // TODO: Implement this method
    }

    public static void HandleOutTokenKeyword(CSharpParserModel model)
    {
    	var keywordToken = model.TokenWalker.Consume();
        // TODO: Implement this method
    }

    public static void HandleParamsTokenKeyword(CSharpParserModel model)
    {
    	var keywordToken = model.TokenWalker.Consume();
        // TODO: Implement this method
    }

    public static void HandleProtectedTokenKeyword(CSharpParserModel model)
    {
    	var protectedTokenKeyword = (KeywordToken)model.TokenWalker.Consume();
        model.StatementBuilderStack.Push(protectedTokenKeyword);
    }

    public static void HandleReadonlyTokenKeyword(CSharpParserModel model)
    {
    	var keywordToken = model.TokenWalker.Consume();
        // TODO: Implement this method
    }

    public static void HandleRefTokenKeyword(CSharpParserModel model)
    {
    	var keywordToken = model.TokenWalker.Consume();
        // TODO: Implement this method
    }

    public static void HandleSbyteTokenKeyword(CSharpParserModel model)
    {
        HandleTypeIdentifierKeyword(model);
    }

    public static void HandleShortTokenKeyword(CSharpParserModel model)
    {
        HandleTypeIdentifierKeyword(model);
    }

    public static void HandleSizeofTokenKeyword(CSharpParserModel model)
    {
    	var keywordToken = model.TokenWalker.Consume();
        // TODO: Implement this method
    }

    public static void HandleStackallocTokenKeyword(CSharpParserModel model)
    {
    	var keywordToken = model.TokenWalker.Consume();
        // TODO: Implement this method
    }

    public static void HandleStringTokenKeyword(CSharpParserModel model)
    {
        HandleTypeIdentifierKeyword(model);
    }

    public static void HandleStructTokenKeyword(CSharpParserModel model)
    {
        HandleStorageModifierTokenKeyword(model);
    }

    public static void HandleSwitchTokenKeyword(CSharpParserModel model)
    {
    	var switchKeywordToken = (KeywordToken)model.TokenWalker.Consume();
    }

    public static void HandleThisTokenKeyword(CSharpParserModel model)
    {
    	var keywordToken = model.TokenWalker.Consume();
        // TODO: Implement this method
    }

    public static void HandleThrowTokenKeyword(CSharpParserModel model)
    {
    	var keywordToken = model.TokenWalker.Consume();
        // TODO: Implement this method
    }

    public static void HandleTrueTokenKeyword(CSharpParserModel model)
    {
    	var expressionNode = ParseOthers.ParseExpression(model);
    	model.StatementBuilderStack.Push(expressionNode);
    }

    public static void HandleTryTokenKeyword(CSharpParserModel model)
    {
    	var tryKeywordToken = (KeywordToken)model.TokenWalker.Consume();
    }

    public static void HandleTypeofTokenKeyword(CSharpParserModel model)
    {
    	var keywordToken = model.TokenWalker.Consume();
        // TODO: Implement this method
    }

    public static void HandleUintTokenKeyword(CSharpParserModel model)
    {
        HandleTypeIdentifierKeyword(model);
    }

    public static void HandleUlongTokenKeyword(CSharpParserModel model)
    {
        HandleTypeIdentifierKeyword(model);
    }

    public static void HandleUncheckedTokenKeyword(CSharpParserModel model)
    {
    	var keywordToken = model.TokenWalker.Consume();
        // TODO: Implement this method
    }

    public static void HandleUnsafeTokenKeyword(CSharpParserModel model)
    {
    	var keywordToken = model.TokenWalker.Consume();
        // TODO: Implement this method
    }

    public static void HandleUshortTokenKeyword(CSharpParserModel model)
    {
        HandleTypeIdentifierKeyword(model);
    }

    public static void HandleVoidTokenKeyword(CSharpParserModel model)
    {
        HandleTypeIdentifierKeyword(model);
    }

    public static void HandleVolatileTokenKeyword(CSharpParserModel model)
    {
    	var keywordToken = model.TokenWalker.Consume();
        // TODO: Implement this method
    }

    public static void HandleWhileTokenKeyword(CSharpParserModel model)
    {
    	var whileKeywordToken = (KeywordToken)model.TokenWalker.Consume();
    }

    public static void HandleUnrecognizedTokenKeyword(CSharpParserModel model)
    {
    	var keywordToken = model.TokenWalker.Consume();
        // TODO: Implement this method
    }

	/// <summary>The 'Default' of this method name is confusing.
	/// It seems to refer to the 'default' of switch statement rather than the 'default' keyword itself?
	/// </summary>
    public static void HandleDefault(CSharpParserModel model)
    {
    	var defaultKeywordToken = (KeywordToken)model.TokenWalker.Consume();
    }

    public static void HandleTypeIdentifierKeyword(CSharpParserModel model)
    {
    	var typeIdentifierKeywordToken = (KeywordToken)model.TokenWalker.Consume();
    }

    public static void HandleNewTokenKeyword(CSharpParserModel model)
    {
    	var newKeywordToken = (KeywordToken)model.TokenWalker.Consume();
    }

    public static void HandlePublicTokenKeyword(CSharpParserModel model)
    {
    	var publicKeywordToken = (KeywordToken)model.TokenWalker.Consume();
        model.StatementBuilderStack.Push(publicKeywordToken);
    }

    public static void HandleInternalTokenKeyword(CSharpParserModel model)
    {
    	var internalTokenKeyword = (KeywordToken)model.TokenWalker.Consume();
        model.StatementBuilderStack.Push(internalTokenKeyword);
    }

    public static void HandlePrivateTokenKeyword(CSharpParserModel model)
    {
    	var privateTokenKeyword = (KeywordToken)model.TokenWalker.Consume();
        model.StatementBuilderStack.Push(privateTokenKeyword);
    }

    public static void HandleStaticTokenKeyword(CSharpParserModel model)
    {
    	var keywordToken = model.TokenWalker.Consume();
        // TODO: Implement this method
    }

    public static void HandleOverrideTokenKeyword(CSharpParserModel model)
    {
    	var keywordToken = model.TokenWalker.Consume();
        // TODO: Implement this method
    }

    public static void HandleVirtualTokenKeyword(CSharpParserModel model)
    {
    	var keywordToken = model.TokenWalker.Consume();
        // TODO: Implement this method
    }

    public static void HandleAbstractTokenKeyword(CSharpParserModel model)
    {
    	var keywordToken = model.TokenWalker.Consume();
        // TODO: Implement this method
    }

    public static void HandleSealedTokenKeyword(CSharpParserModel model)
    {
    	var keywordToken = model.TokenWalker.Consume();
        // TODO: Implement this method
    }

    public static void HandleIfTokenKeyword(CSharpParserModel model)
    {
    	var ifTokenKeyword = (KeywordToken)model.TokenWalker.Consume();
    }

    public static void HandleUsingTokenKeyword(CSharpParserModel model)
    {
    	var usingTokenKeyword = (KeywordToken)model.TokenWalker.Consume();
    }

    public static void HandleInterfaceTokenKeyword(CSharpParserModel model)
    {
        ParseDefaultKeywords.HandleStorageModifierTokenKeyword(model);
    }

	/// <summary>
	/// Example:
	/// public class MyClass { }
	///              ^
	///
	/// Given the example the 'MyClass' is the next token
	/// upon invocation of this method.
	///
	/// Invocation of this method implies the current token was
	/// class, interface, struct, etc...
	/// </summary>
    public static void HandleStorageModifierTokenKeyword(CSharpParserModel model)
    {
    	var storageModifierToken = model.TokenWalker.Consume();
    
    	// Given: public partial class MyClass { }
		// Then: partial
        var hasPartialModifier = false;
        if (model.StatementBuilderStack.TryPeek(out var syntax) && syntax is ISyntaxToken syntaxToken)
        {
            if (syntaxToken.SyntaxKind == SyntaxKind.PartialTokenContextualKeyword)
            {
                _ = model.StatementBuilderStack.Pop();
                hasPartialModifier = true;
            }
        }
    
    	// TODO: Fix; the code that parses the accessModifierKind is a mess
		//
		// Given: public class MyClass { }
		// Then: public
		var accessModifierKind = AccessModifierKind.Public;
        if (model.StatementBuilderStack.TryPeek(out syntax) && syntax is ISyntaxToken firstSyntaxToken)
        {
            var firstOutput = UtilityApi.GetAccessModifierKindFromToken(firstSyntaxToken);

            if (firstOutput is not null)
            {
                _ = model.StatementBuilderStack.Pop();
                accessModifierKind = firstOutput.Value;

				// Given: protected internal class MyClass { }
				// Then: protected internal
                if (model.StatementBuilderStack.TryPeek(out syntax) && syntax is ISyntaxToken secondSyntaxToken)
                {
                    var secondOutput = UtilityApi.GetAccessModifierKindFromToken(secondSyntaxToken);

                    if (secondOutput is not null)
                    {
                        _ = model.StatementBuilderStack.Pop();

                        if ((firstOutput.Value.ToString().ToLower() == "protected" &&
                                secondOutput.Value.ToString().ToLower() == "internal") ||
                            (firstOutput.Value.ToString().ToLower() == "internal" &&
                                secondOutput.Value.ToString().ToLower() == "protected"))
                        {
                            accessModifierKind = AccessModifierKind.ProtectedInternal;
                        }
                        else if ((firstOutput.Value.ToString().ToLower() == "private" &&
                                    secondOutput.Value.ToString().ToLower() == "protected") ||
                                (firstOutput.Value.ToString().ToLower() == "protected" &&
                                    secondOutput.Value.ToString().ToLower() == "private"))
                        {
                            accessModifierKind = AccessModifierKind.PrivateProtected;
                        }
                        // else use the firstOutput.
                    }
                }
            }
        }
    
    	// TODO: Fix nullability spaghetti code
        var storageModifierKind = UtilityApi.GetStorageModifierKindFromToken(storageModifierToken);
        if (storageModifierKind is null)
            return;
        if (storageModifierKind == StorageModifierKind.Record &&
        	model.TokenWalker.Current.SyntaxKind == SyntaxKind.StructTokenKeyword)
        {
        	var structKeywordToken = (KeywordToken)model.TokenWalker.Consume();
        	storageModifierKind = StorageModifierKind.RecordStruct;
        }
    
		// Given: public class MyClass<T> { }
		// Then: MyClass
		IdentifierToken identifierToken;
		// Retrospective: What is the purpose of this 'if (contextualKeyword) logic'?
		// Response: maybe it is because 'var' contextual keyword is allowed to be a class name?
        if (UtilityApi.IsContextualKeywordSyntaxKind(model.TokenWalker.Current.SyntaxKind))
        {
            var contextualKeywordToken = (KeywordContextualToken)model.TokenWalker.Consume();
            // Take the contextual keyword as an identifier
            identifierToken = new IdentifierToken(contextualKeywordToken.TextSpan);
        }
        else
        {
            identifierToken = (IdentifierToken)model.TokenWalker.Match(SyntaxKind.IdentifierToken);
        }

		// Given: public class MyClass<T> { }
		// Then: <T>
        GenericArgumentsListingNode? genericArgumentsListingNode = null;
        if (model.TokenWalker.Current.SyntaxKind == SyntaxKind.OpenAngleBracketToken)
        {
            ParseTypes.HandleGenericArguments(
                (OpenAngleBracketToken)model.TokenWalker.Consume(),
                model);

            genericArgumentsListingNode = (GenericArgumentsListingNode?)model.StatementBuilderStack.Pop();
        }

        var typeDefinitionNode = new TypeDefinitionNode(
            accessModifierKind,
            hasPartialModifier,
            storageModifierKind.Value,
            identifierToken,
            valueType: null,
            genericArgumentsListingNode,
            primaryConstructorFunctionArgumentsListingNode: null,
            inheritedTypeClauseNode: null,
            openBraceToken: default,
            codeBlockNode: null);

        model.Binder.BindTypeDefinitionNode(typeDefinitionNode, model);
        model.Binder.BindTypeIdentifier(identifierToken, model);
        model.StatementBuilderStack.Push(typeDefinitionNode);
        model.CurrentCodeBlockBuilder.PendingChild = typeDefinitionNode;
        
        if (model.TokenWalker.Current.SyntaxKind == SyntaxKind.WhereTokenContextualKeyword)
        {
        	while (!model.TokenWalker.IsEof)
        	{
        		if (model.TokenWalker.Current.SyntaxKind == SyntaxKind.OpenBraceToken ||
        			model.TokenWalker.Current.SyntaxKind == SyntaxKind.StatementDelimiterToken)
        		{
        			break;
        		}
        		
        		_ = model.TokenWalker.Consume();
        	}
        }
    }

    public static void HandleClassTokenKeyword(CSharpParserModel model)
    {
        HandleStorageModifierTokenKeyword(model);
    }

    public static void HandleNamespaceTokenKeyword(CSharpParserModel model)
    {
    	var namespaceTokenKeyword = (KeywordToken)model.TokenWalker.Consume();
    }

    public static void HandleReturnTokenKeyword(CSharpParserModel model)
    {
    	var returnTokenKeyword = (KeywordToken)model.TokenWalker.Consume();
    }
}
