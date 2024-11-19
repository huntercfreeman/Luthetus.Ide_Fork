using System.Collections.Immutable;
using Luthetus.TextEditor.RazorLib.CompilerServices.Syntax.Nodes.Interfaces;

namespace Luthetus.TextEditor.RazorLib.CompilerServices.Syntax.Nodes;

/// <summary>
/// If the parser cannot parse an expression, then replace
/// the expression result with an instance of this type.
///
/// The <see cref="SyntaxList"/> contains the 'primaryExpression'
/// that was attempted to have merge with the 'token' or 'secondaryExpression'.
/// As well it contains either the 'token' or the 'secondaryExpression'.
/// And then as well any other syntax that was parsed up until
/// the expression ended.
/// </summary>
public sealed class BadExpressionNode : IExpressionNode
{
    public BadExpressionNode(TypeClauseNode resultTypeClauseNode, List<ISyntax> syntaxList)
    {
    	ResultTypeClauseNode = resultTypeClauseNode;
        SyntaxList = syntaxList;
    }
    
    public BadExpressionNode(TypeClauseNode resultTypeClauseNode, ISyntax syntaxPrimary, ISyntax syntaxSecondary)
    	: this(resultTypeClauseNode, new List<ISyntax> { syntaxPrimary, syntaxSecondary })
    {
    }

	private ISyntax[] _childList = Array.Empty<ISyntax>();
	private bool _childListIsDirty = true;

    public List<ISyntax> SyntaxList { get; }
    public TypeClauseNode ResultTypeClauseNode { get; }

    public bool IsFabricated { get; init; }
    public SyntaxKind SyntaxKind => SyntaxKind.BadExpressionNode;
    
    public int GetStartInclusiveIndex()
    {
    }
    
    public int GetEndExclusiveIndex()
    {
    }
    
    public ISyntax[] GetChildList()
    {
    	if (!_childListIsDirty)
    		return _childList;
    	
    	_childList = SyntaxList.ToArray();
        
    	_childListIsDirty = false;
    	return _childList;
    }
}