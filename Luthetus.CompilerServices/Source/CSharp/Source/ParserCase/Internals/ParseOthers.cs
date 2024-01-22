﻿using Luthetus.TextEditor.RazorLib.CompilerServices.Syntax.SyntaxTokens;
using Luthetus.TextEditor.RazorLib.CompilerServices.Syntax.SyntaxNodes;
using Luthetus.TextEditor.RazorLib.CompilerServices.GenericLexer.Decoration;
using Luthetus.TextEditor.RazorLib.CompilerServices.Syntax;
using Luthetus.TextEditor.RazorLib.CompilerServices.Syntax.SyntaxNodes.Expression;
using Luthetus.CompilerServices.Lang.CSharp.Facts;

namespace Luthetus.CompilerServices.Lang.CSharp.ParserCase.Internals;

public static class ParseOthers
{
    public static void HandleNamespaceReference(
        IdentifierToken consumedIdentifierToken,
        NamespaceGroupNode resolvedNamespaceGroupNode,
        ParserModel model)
    {
        model.Binder.BindNamespaceReference(consumedIdentifierToken);

        if (SyntaxKind.MemberAccessToken == model.TokenWalker.Current.SyntaxKind)
        {
            var memberAccessToken = model.TokenWalker.Consume();
            var memberIdentifierToken = (IdentifierToken)model.TokenWalker.Match(SyntaxKind.IdentifierToken);

            if (memberIdentifierToken.IsFabricated)
            {
                model.DiagnosticBag.ReportUnexpectedToken(
                    model.TokenWalker.Current.TextSpan,
                    model.TokenWalker.Current.SyntaxKind.ToString(),
                    SyntaxKind.IdentifierToken.ToString());
            }

            // Check all the TypeDefinitionNodes that are in the namespace
            var typeDefinitionNodes = resolvedNamespaceGroupNode.GetTopLevelTypeDefinitionNodes();

            var typeDefinitionNode = typeDefinitionNodes.SingleOrDefault(td =>
                td.TypeIdentifier.TextSpan.GetText() == memberIdentifierToken.TextSpan.GetText());

            if (typeDefinitionNode is null)
            {
                model.DiagnosticBag.ReportNotDefinedInContext(
                    model.TokenWalker.Current.TextSpan,
                    consumedIdentifierToken.TextSpan.GetText());
            }
            else
            {
                ParseTypes.HandleTypeReference(
                    memberIdentifierToken,
                    typeDefinitionNode,
                    model);
            }
        }
        else
        {
            // TODO: (2023-05-28) Report an error diagnostic for 'namespaces are not statements'. Something like this I'm not sure.
            model.TokenWalker.Consume();
        }
    }

    public static void HandleNamespaceIdentifier(ParserModel model)
    {
        var combineNamespaceIdentifierIntoOne = new List<ISyntaxToken>();

        while (!model.TokenWalker.IsEof)
        {
            if (combineNamespaceIdentifierIntoOne.Count % 2 == 0)
            {
                var matchedToken = model.TokenWalker.Match(SyntaxKind.IdentifierToken);
                combineNamespaceIdentifierIntoOne.Add(matchedToken);

                if (matchedToken.IsFabricated)
                    break;
            }
            else
            {
                if (SyntaxKind.MemberAccessToken == model.TokenWalker.Current.SyntaxKind)
                    combineNamespaceIdentifierIntoOne.Add(model.TokenWalker.Consume());
                else
                    break;
            }
        }

        if (combineNamespaceIdentifierIntoOne.Count == 0)
        {
            model.SyntaxStack.Push(new EmptyNode());
            return;
        }

        var identifierTextSpan = combineNamespaceIdentifierIntoOne.First().TextSpan with
        {
            EndingIndexExclusive = combineNamespaceIdentifierIntoOne.Last().TextSpan.EndingIndexExclusive
        };

        model.SyntaxStack.Push(new IdentifierToken(identifierTextSpan));
    }

    public static void HandleExpression(
        IExpressionNode? topMostExpressionNode,
        IExpressionNode? previousInvocationExpressionNode,
        IExpressionNode? leftExpressionNode,
        ISyntaxToken? operatorToken,
        IExpressionNode? rightExpressionNode,
        ExpressionDelimiter[]? extraExpressionDeliminaters, ParserModel model)
    {
        while (!model.TokenWalker.IsEof)
        {
            var tokenCurrent = model.TokenWalker.Consume();

            if (tokenCurrent.SyntaxKind == SyntaxKind.EndOfFileToken || tokenCurrent.SyntaxKind == SyntaxKind.StatementDelimiterToken)
            {
                model.TokenWalker.Backtrack();
                break;
            }

            ExpressionDelimiter? closeExtraExpressionDelimiterEncountered =
                extraExpressionDeliminaters?.FirstOrDefault(x => x.CloseSyntaxKind == tokenCurrent.SyntaxKind);

            if (closeExtraExpressionDelimiterEncountered is not null)
            {
                if (tokenCurrent.SyntaxKind == SyntaxKind.CloseParenthesisToken)
                {
                    if (closeExtraExpressionDelimiterEncountered?.OpenSyntaxToken is not null)
                    {
                        ParenthesizedExpressionNode parenthesizedExpression;

                        if (previousInvocationExpressionNode is not null)
                        {
                            parenthesizedExpression = new ParenthesizedExpressionNode(
                                (OpenParenthesisToken)closeExtraExpressionDelimiterEncountered.OpenSyntaxToken,
                                previousInvocationExpressionNode,
                                (CloseParenthesisToken)tokenCurrent);
                        }
                        else
                        {
                            parenthesizedExpression = new ParenthesizedExpressionNode(
                                (OpenParenthesisToken)closeExtraExpressionDelimiterEncountered.OpenSyntaxToken,
                                new EmptyExpressionNode(CSharpFacts.Types.Void.ToTypeClause()),
                                (CloseParenthesisToken)tokenCurrent);
                        }

                        model.SyntaxStack.Push(parenthesizedExpression);
                        return;
                    }
                    else
                    {
                        // If one provides 'CloseParenthesisToken' as a closing delimiter,
                        // but does not provide the corresponding open delimiter (it is null)
                        // then a function invocation started the initial invocation
                        // of this method.
                        model.TokenWalker.Backtrack();
                        break;
                    }
                }
                else if (tokenCurrent.SyntaxKind == SyntaxKind.CommaToken ||
                         tokenCurrent.SyntaxKind == SyntaxKind.CloseBraceToken)
                {
                    model.TokenWalker.Backtrack();
                    break;
                }
            }

            switch (tokenCurrent.SyntaxKind)
            {
                case SyntaxKind.TrueTokenKeyword:
                case SyntaxKind.FalseTokenKeyword:
                    var booleanLiteralExpressionNode = new LiteralExpressionNode(tokenCurrent, CSharpFacts.Types.Bool.ToTypeClause());

                    previousInvocationExpressionNode = booleanLiteralExpressionNode;

                    if (topMostExpressionNode is null)
                    {
                        topMostExpressionNode = booleanLiteralExpressionNode;
                    }
                    else if (leftExpressionNode is null)
                    {
                        if (topMostExpressionNode.SyntaxKind != SyntaxKind.LiteralExpressionNode)
                            leftExpressionNode = booleanLiteralExpressionNode;
                    }
                    else if (rightExpressionNode is null)
                    {
                        if (topMostExpressionNode.SyntaxKind != SyntaxKind.LiteralExpressionNode)
                            rightExpressionNode = booleanLiteralExpressionNode;
                    }
                    else
                    {
                        throw new ApplicationException("TODO: Why would this occur?");
                    }

                    break;
                case SyntaxKind.NumericLiteralToken:
                    var numericLiteralExpressionNode = new LiteralExpressionNode(tokenCurrent, CSharpFacts.Types.Int.ToTypeClause());

                    previousInvocationExpressionNode = numericLiteralExpressionNode;

                    if (topMostExpressionNode is null)
                    {
                        topMostExpressionNode = numericLiteralExpressionNode;
                    }
                    else if (leftExpressionNode is null)
                    {
                        if (topMostExpressionNode.SyntaxKind != SyntaxKind.LiteralExpressionNode)
                            leftExpressionNode = numericLiteralExpressionNode;
                    }
                    else if (rightExpressionNode is null)
                    {
                        if (topMostExpressionNode.SyntaxKind != SyntaxKind.LiteralExpressionNode)
                            rightExpressionNode = numericLiteralExpressionNode;
                    }
                    else
                    {
                        throw new ApplicationException("TODO: Why would this occur?");
                    }

                    break;
                case SyntaxKind.StringLiteralToken:
                    var stringLiteralExpressionNode = new LiteralExpressionNode(tokenCurrent, CSharpFacts.Types.String.ToTypeClause());

                    previousInvocationExpressionNode = stringLiteralExpressionNode;

                    if (topMostExpressionNode is null)
                    {
                        topMostExpressionNode = stringLiteralExpressionNode;
                    }
                    else if (leftExpressionNode is null)
                    {
                        if (topMostExpressionNode.SyntaxKind != SyntaxKind.LiteralExpressionNode)
                            leftExpressionNode = stringLiteralExpressionNode;
                    }
                    else if (rightExpressionNode is null)
                    {
                        if (topMostExpressionNode.SyntaxKind != SyntaxKind.LiteralExpressionNode)
                            rightExpressionNode = stringLiteralExpressionNode;
                    }
                    else
                    {
                        throw new ApplicationException("TODO: Why would this occur?");
                    }

                    break;
                case SyntaxKind.IdentifierToken:
                    var variableReferenceNode = new VariableReferenceNode(
                        (IdentifierToken)tokenCurrent,
                        // TODO: Don't pass null here
                        null);

                    model.Binder.BindVariableReferenceNode(variableReferenceNode);
                    break;
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                case SyntaxKind.StarToken:
                case SyntaxKind.DivisionToken:
                    if (leftExpressionNode is null && previousInvocationExpressionNode is not null)
                        leftExpressionNode = previousInvocationExpressionNode;

                    if (previousInvocationExpressionNode is BinaryExpressionNode previousBinaryExpressionNode)
                    {
                        var previousOperatorPrecedence = UtilityApi.GetOperatorPrecedence(previousBinaryExpressionNode.BinaryOperatorNode.OperatorToken.SyntaxKind);
                        var currentOperatorPrecedence = UtilityApi.GetOperatorPrecedence(tokenCurrent.SyntaxKind);

                        if (currentOperatorPrecedence > previousOperatorPrecedence)
                        {
                            // Take the right node from the previous expression.
                            // Make it the new expression's left node.
                            //
                            // Then replace the previous expression's right node with the
                            // newly formed expression.

                            HandleExpression(
                                topMostExpressionNode,
                                null,
                                previousBinaryExpressionNode.RightExpressionNode,
                                tokenCurrent,
                                null,
                                extraExpressionDeliminaters,
                                model);

                            var modifiedRightExpressionNode = (IExpressionNode)model.SyntaxStack.Pop();

                            topMostExpressionNode = new BinaryExpressionNode(
                                previousBinaryExpressionNode.LeftExpressionNode,
                                previousBinaryExpressionNode.BinaryOperatorNode,
                                modifiedRightExpressionNode);
                        }
                    }

                    if (operatorToken is null)
                        operatorToken = tokenCurrent;
                    else
                        throw new ApplicationException("TODO: Why would this occur?");

                    break;
                case SyntaxKind.OpenParenthesisToken:
                    var copyExtraExpressionDeliminaters = new List<ExpressionDelimiter>(extraExpressionDeliminaters ?? Array.Empty<ExpressionDelimiter>());

                    copyExtraExpressionDeliminaters.Insert(0, new ExpressionDelimiter(
                        SyntaxKind.OpenParenthesisToken,
                        SyntaxKind.CloseParenthesisToken,
                        tokenCurrent,
                        null));

                    HandleExpression(
                        null,
                        null,
                        null,
                        null,
                        null,
                        copyExtraExpressionDeliminaters.ToArray(),
                        model);

                    var parenthesizedExpression = (IExpressionNode)model.SyntaxStack.Pop();

                    previousInvocationExpressionNode = parenthesizedExpression;

                    if (topMostExpressionNode is null)
                        topMostExpressionNode = parenthesizedExpression;
                    else if (leftExpressionNode is null)
                        leftExpressionNode = parenthesizedExpression;
                    else if (rightExpressionNode is null)
                        rightExpressionNode = parenthesizedExpression;
                    else
                        throw new ApplicationException("TODO: Why would this occur?");
                    break;
                default:
                    if (tokenCurrent.SyntaxKind == SyntaxKind.DollarSignToken)
                    {
                        // TODO: Convert DollarSignToken to a function signature...
                        // ...Then read in the parameters...
                        // ...Any function invocation logic also would be done here

                        model.Binder.BindStringInterpolationExpression((DollarSignToken)tokenCurrent);
                    }

                    break;
            }

            if (leftExpressionNode is not null && operatorToken is not null && rightExpressionNode is not null)
            {
                var binaryOperatorNode = model.Binder.BindBinaryOperatorNode(
                    leftExpressionNode,
                    operatorToken,
                    rightExpressionNode);

                var binaryExpressionNode = new BinaryExpressionNode(
                    leftExpressionNode,
                    binaryOperatorNode,
                    rightExpressionNode);

                topMostExpressionNode = binaryExpressionNode;
                previousInvocationExpressionNode = binaryExpressionNode;

                leftExpressionNode = null;
                operatorToken = null;
                rightExpressionNode = null;

                HandleExpression(
                    topMostExpressionNode,
                    previousInvocationExpressionNode,
                    leftExpressionNode,
                    operatorToken,
                    rightExpressionNode,
                    extraExpressionDeliminaters,
                    model);

                return;
            }
        }

        var fallbackExpressionNode = new LiteralExpressionNode(
            new EndOfFileToken(new(0, 0, (byte)GenericDecorationKind.None, new(string.Empty), string.Empty)),
            CSharpFacts.Types.Void.ToTypeClause());

        model.SyntaxStack.Push(topMostExpressionNode ?? fallbackExpressionNode);
    }
}