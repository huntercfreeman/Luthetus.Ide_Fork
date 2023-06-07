﻿namespace Luthetus.Ide.ClassLib.CompilerServices.Common.Syntax;

/// <summary>
/// In order to share identical logic with C and CSharp code analysis I need to have them share the SyntaxKind enum. I don't like this because some enum members are used in one language but not the other.
/// </summary>
public enum SyntaxKind
{
    // Tokens
    CommentMultiLineToken,
    CommentSingleLineToken,
    IdentifierToken,
    KeywordToken,
    KeywordContextualToken,
    NumericLiteralToken,
    StringLiteralToken,
    TriviaToken,
    PreprocessorDirectiveToken,
    LibraryReferenceToken,
    PlusToken,
    PlusPlusToken,
    MinusToken,
    MinusMinusToken,
    EqualsToken,
    EqualsEqualsToken,
    QuestionMarkToken,
    QuestionMarkQuestionMarkToken,
    BangToken,
    StatementDelimiterToken,
    OpenParenthesisToken,
    CloseParenthesisToken,
    OpenBraceToken,
    CloseBraceToken,
    OpenAngleBracketToken,
    CloseAngleBracketToken,
    OpenSquareBracketToken,
    CloseSquareBracketToken,
    DollarSignToken,
    ColonToken,
    MemberAccessToken,
    CommaToken,
    BadToken,
    EndOfFileToken,

    // Nodes
    CompilationUnitNode,
    LiteralExpressionNode,
    BoundLiteralExpressionNode,
    BoundBinaryOperatorNode,
    BoundBinaryExpressionNode,
    PreprocessorLibraryReferenceStatementNode,
    BoundFunctionDefinitionNode,
    BoundIfStatementNode,
    BoundVariableDeclarationStatementNode,
    BoundVariableAssignmentStatementNode,
    BoundFunctionInvocationNode,
    BoundReturnStatementNode,
    BoundNamespaceStatementNode,
    BoundClassDefinitionNode,
    BoundClassReferenceNode,
    BoundInheritanceStatementNode,
    BoundUsingStatementNode,
    BoundIdentifierReferenceNode,
    BoundAttributeNode,
    BoundGenericArgumentsNode,
    BoundFunctionArgumentsNode,
    BoundFunctionParametersNode,
    BoundConstructorInvocationNode,
    BoundObjectInitializationNode,
    BoundNamespaceEntryNode,

    // Symbols
    TypeSymbol,
    FunctionSymbol,
    VariableSymbol,
    PropertySymbol,
    StringInterpolationSymbol,
}