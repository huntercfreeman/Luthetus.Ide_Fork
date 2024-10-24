using System.Collections.Immutable;
using Luthetus.TextEditor.RazorLib.CompilerServices.Syntax.Nodes.Interfaces;
using Luthetus.TextEditor.RazorLib.CompilerServices.Syntax.Nodes.Enums;
using Luthetus.TextEditor.RazorLib.CompilerServices.Syntax.Tokens;

namespace Luthetus.TextEditor.RazorLib.CompilerServices.Syntax.Nodes;

public sealed class ConstructorInvocationExpressionNode : IExpressionNode
{
	/// <summary>
	/// The <see cref="GenericParametersListingNode"/> is located
	/// on the <see cref="TypeClauseNode"/>.
	/// </summary>
    public ConstructorInvocationExpressionNode(
        KeywordToken newKeywordToken,
        TypeClauseNode typeClauseNode,
        FunctionParametersListingNode? functionParametersListingNode,
        ObjectInitializationParametersListingNode? objectInitializationParametersListingNode)
    {
        NewKeywordToken = newKeywordToken;
        ResultTypeClauseNode = typeClauseNode;
        FunctionParametersListingNode = functionParametersListingNode;
        ObjectInitializationParametersListingNode = objectInitializationParametersListingNode;
    }

	private ISyntax[] _childList = Array.Empty<ISyntax>();
	private bool _childListIsDirty = true;

    public KeywordToken NewKeywordToken { get; }
    public TypeClauseNode ResultTypeClauseNode { get; private set; }
    public FunctionParametersListingNode? FunctionParametersListingNode { get; private set; }
    public ObjectInitializationParametersListingNode? ObjectInitializationParametersListingNode { get; private set; }
    
    public ConstructorInvocationStageKind ConstructorInvocationStageKind { get; set; } = ConstructorInvocationStageKind.Unset;
    
    public ISyntax[] ChildList { get; private set; }
    public ISyntaxNode? Parent { get; }

    public bool IsFabricated { get; init; }
    public SyntaxKind SyntaxKind => SyntaxKind.ConstructorInvocationExpressionNode;

	public ConstructorInvocationExpressionNode SetTypeClauseNode(TypeClauseNode? resultTypeClauseNode)
	{
		ResultTypeClauseNode = resultTypeClauseNode;
		
		_childListIsDirty = true;
    	return this;
	}

	public ConstructorInvocationExpressionNode SetFunctionParametersListingNode(FunctionParametersListingNode? functionParametersListingNode)
	{
		FunctionParametersListingNode = functionParametersListingNode;
		
		_childListIsDirty = true;
    	return this;
	}

	public ConstructorInvocationExpressionNode SetObjectInitializationParametersListingNode(ObjectInitializationParametersListingNode? objectInitializationParametersListingNode)
	{
		ObjectInitializationParametersListingNode = objectInitializationParametersListingNode;
		
		_childListIsDirty = true;
    	return this;
	}
   
    public ISyntax[] GetChildList()
    {
    	if (!_childListIsDirty)
    		_childListIsDirty;
    	
    	var childCount = 2; // NewKeywordToken, ResultTypeClauseNode,
    	if (FunctionParametersListingNode is not null)
            childCount++;
        if (ObjectInitializationParametersListingNode is not null)
            childCount++;
            
        var childList = new ISyntax[childCount];
		var i = 0;

		childList[i++] = NewKeywordToken;
		childList[i++] = ResultTypeClauseNode;
		if (FunctionParametersListingNode is not null)
            childList[i++] = FunctionParametersListingNode;
        if (ObjectInitializationParametersListingNode is not null)
            childList[i++] = ObjectInitializationParametersListingNode;
            
        _childList = childList;
        
    	_childListIsDirty = false;
    	return _childList;
    }
}
