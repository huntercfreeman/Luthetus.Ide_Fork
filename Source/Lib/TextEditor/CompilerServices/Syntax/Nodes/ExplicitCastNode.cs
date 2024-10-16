using System.Collections.Immutable;
using Luthetus.TextEditor.RazorLib.CompilerServices.Syntax.Nodes.Interfaces;
using Luthetus.TextEditor.RazorLib.CompilerServices.Syntax.Tokens;

namespace Luthetus.TextEditor.RazorLib.CompilerServices.Syntax.Nodes;

public sealed class ExplicitCastNode : ISyntaxNode
{
	public ExplicitCastNode(
        OpenParenthesisToken openParenthesisToken,
        TypeClauseNode typeClauseNode,
        CloseParenthesisToken closeParenthesisToken,
        IExpressionNode expressionNode)
    {
        OpenParenthesisToken = openParenthesisToken;
        TypeClauseNode = typeClauseNode;
        CloseParenthesisToken = closeParenthesisToken;
        ExpressionNode = expressionNode;

        SetChildList();
    }

    public OpenParenthesisToken OpenParenthesisToken { get; }
    public TypeClauseNode TypeClauseNode { get; }
    public CloseParenthesisToken CloseParenthesisToken { get; }
    public IExpressionNode ExpressionNode { get; }

    public ISyntax[] ChildList { get; private set; }
    public ISyntaxNode? Parent { get; }

    public bool IsFabricated { get; init; }
    public SyntaxKind SyntaxKind => SyntaxKind.ExplicitCastNode;
    
    public void SetChildList()
    {
    	ChildList = new ISyntax[]
        {
            OpenParenthesisToken,
            TypeClauseNode,
            CloseParenthesisToken,
            ExpressionNode,
        }.ToImmutableArray();
    	throw new NotImplementedException();
    }
}
