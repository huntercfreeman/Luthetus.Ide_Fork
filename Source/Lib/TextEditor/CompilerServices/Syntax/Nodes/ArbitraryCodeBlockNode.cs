using System.Collections.Immutable;
using Luthetus.TextEditor.RazorLib.CompilerServices.Syntax.Nodes.Enums;
using Luthetus.TextEditor.RazorLib.CompilerServices.Syntax.Nodes.Interfaces;
using Luthetus.TextEditor.RazorLib.CompilerServices.Syntax.Tokens;
using Luthetus.TextEditor.RazorLib.CompilerServices.Interfaces;

namespace Luthetus.TextEditor.RazorLib.CompilerServices.Syntax.Nodes;

public sealed class ArbitraryCodeBlockNode : ICodeBlockOwner
{
    public ArbitraryCodeBlockNode(ICodeBlockOwner? parentCodeBlockOwner)
    {
        ParentCodeBlockOwner = parentCodeBlockOwner;
        Parent = ParentCodeBlockOwner;
        
        SetChildList();
    }

    public ICodeBlockOwner? ParentCodeBlockOwner { get; }
    public OpenBraceToken OpenBraceToken { get; private set; }
	public CodeBlockNode? CodeBlockNode { get; private set; }

	public ScopeDirectionKind ScopeDirectionKind => ParentCodeBlockOwner.ScopeDirectionKind;

    public ISyntax[] ChildList { get; private set; }
    public ISyntaxNode? Parent { get; }

    public bool IsFabricated { get; init; }
    public SyntaxKind SyntaxKind => SyntaxKind.ArbitraryCodeBlockNode;
    
    public TypeClauseNode? GetReturnTypeClauseNode()
    {
    	return ParentCodeBlockOwner?.GetReturnTypeClauseNode();
    }
    
    public ICodeBlockOwner SetCodeBlockNode(OpenBraceToken openBraceToken, CodeBlockNode codeBlockNode)
    {
    	OpenBraceToken = openBraceToken;
    	CodeBlockNode = codeBlockNode;
    	SetChildList();
    	return this;
    }
    
    public void OnBoundScopeCreatedAndSetAsCurrent(IParserModel parserModel)
    {
    	// Do nothing.
    	return;
    }
    
    public void SetChildList()
    {
    	var childCount = 0;
        if (OpenBraceToken.ConstructorWasInvoked)
    		childCount++;
    	if (CodeBlockNode is not null)
    		childCount++;
            
        var childList = new ISyntax[childCount];
		var i = 0;

		if (OpenBraceToken.ConstructorWasInvoked)
    		childList[i++] = OpenBraceToken;
    	if (CodeBlockNode is not null)
    		childList[i++] = CodeBlockNode;
            
        ChildList = childList;
    }
}
