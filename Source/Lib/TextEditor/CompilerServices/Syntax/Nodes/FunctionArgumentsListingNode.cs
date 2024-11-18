using System.Collections.Immutable;
using Luthetus.TextEditor.RazorLib.CompilerServices.Syntax.Tokens;
using Luthetus.TextEditor.RazorLib.CompilerServices.Syntax.Nodes.Interfaces;

namespace Luthetus.TextEditor.RazorLib.CompilerServices.Syntax.Nodes;

/// <summary>
/// Used when defining a function.
/// </summary>
public sealed class FunctionArgumentsListingNode : IExpressionNode
{
    public FunctionArgumentsListingNode(
        OpenParenthesisToken openParenthesisToken,
        List<FunctionArgumentEntryNode> functionArgumentEntryNodeList,
        CloseParenthesisToken closeParenthesisToken)
    {
        OpenParenthesisToken = openParenthesisToken;
        FunctionArgumentEntryNodeList = functionArgumentEntryNodeList;
        CloseParenthesisToken = closeParenthesisToken;
    }

	private ISyntax[] _childList = Array.Empty<ISyntax>();
	private bool _childListIsDirty = true;

    public OpenParenthesisToken OpenParenthesisToken { get; }
    public List<FunctionArgumentEntryNode> FunctionArgumentEntryNodeList { get; }
    public CloseParenthesisToken CloseParenthesisToken { get; }
    TypeClauseNode IExpressionNode.ResultTypeClauseNode => TypeFacts.Pseudo.ToTypeClause();

    public bool IsFabricated { get; init; }
    public SyntaxKind SyntaxKind => SyntaxKind.FunctionArgumentsListingNode;
    
    public ISyntax[] GetChildList()
    {
    	if (!_childListIsDirty)
    		return _childList;
    	
    	// OpenParenthesisToken, FunctionArgumentEntryNodeList.Count, CloseParenthesisToken,
    	var childCount = 
    		1 +                                    // OpenParenthesisToken,
    		FunctionArgumentEntryNodeList.Count + // FunctionArgumentEntryNodeList.Count,
    		1;                                     // CloseParenthesisToken,
            
        var childList = new ISyntax[childCount];
		var i = 0;

		childList[i++] = OpenParenthesisToken;
		foreach (var item in FunctionArgumentEntryNodeList)
		{
			childList[i++] = item;
		}
		childList[i++] = CloseParenthesisToken;
            
        _childList = childList;
        
    	_childListIsDirty = false;
    	return _childList;
    }
}