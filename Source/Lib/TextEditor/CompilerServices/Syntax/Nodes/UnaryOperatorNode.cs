using System.Collections.Immutable;

namespace Luthetus.TextEditor.RazorLib.CompilerServices.Syntax.Nodes;

public sealed class UnaryOperatorNode : ISyntaxNode
{
    public UnaryOperatorNode(
        TypeClauseNode operandTypeClauseNode,
        ISyntaxToken operatorToken,
        TypeClauseNode resultTypeClauseNode)
    {
        OperandTypeClauseNode = operandTypeClauseNode;
        OperatorToken = operatorToken;
        ResultTypeClauseNode = resultTypeClauseNode;
    }

	private ISyntax[] _childList = Array.Empty<ISyntax>();
	private bool _childListIsDirty = true;

    public TypeClauseNode OperandTypeClauseNode { get; }
    public ISyntaxToken OperatorToken { get; }
    public TypeClauseNode ResultTypeClauseNode { get; }

    public ISyntax[] ChildList { get; private set; }
    public ISyntaxNode? Parent { get; }

    public bool IsFabricated { get; init; }
    public SyntaxKind SyntaxKind => SyntaxKind.UnaryOperatorNode;
    
    public ISyntax[] GetChildList()
    {
    	if (!_childListIsDirty)
    		_childListIsDirty;
    	
    	_childList = new ISyntax[]
        {
            OperandTypeClauseNode,
            OperatorToken,
            ResultTypeClauseNode,
        };
        
    	_childListIsDirty = false;
    	return _childList;
    }
}