using Luthetus.TextEditor.RazorLib.CompilerServices.Syntax.Nodes.Interfaces;
using Luthetus.TextEditor.RazorLib.CompilerServices.Syntax.Nodes.Enums;
using Luthetus.TextEditor.RazorLib.CompilerServices.Syntax.Tokens;
using System.Collections.Immutable;

namespace Luthetus.TextEditor.RazorLib.CompilerServices.Syntax.Nodes;

public sealed record TryStatementFinallyNode : ICodeBlockOwner
{
	public TryStatementFinallyNode(
		TryStatementNode? parent,
        KeywordToken keywordToken,
        CodeBlockNode? codeBlockNode)
    {
    	Parent = parent;
        KeywordToken = keywordToken;
        CodeBlockNode = codeBlockNode;

        SetChildList();
    }

    public KeywordToken KeywordToken { get; }
    public OpenBraceToken? OpenBraceToken { get; private set; }
    public CodeBlockNode? CodeBlockNode { get; private set; }

	public ScopeDirectionKind ScopeDirectionKind => ScopeDirectionKind.Down;

    public ImmutableArray<ISyntax> ChildList { get; private set; }
    public ISyntaxNode? Parent { get; }

    public bool IsFabricated { get; init; }
    public SyntaxKind SyntaxKind => SyntaxKind.TryStatementFinallyNode;
    
    public TypeClauseNode? GetReturnTypeClauseNode()
    {
    	return null;
    }
    
    public ICodeBlockOwner WithCodeBlockNode(OpenBraceToken openBraceToken, CodeBlockNode codeBlockNode)
    {
    	OpenBraceToken = openBraceToken;
    	CodeBlockNode = codeBlockNode;
    	return this;
    }
    
    public void SetChildList()
    {
    	var childrenList = new List<ISyntax>();

        if (KeywordToken is not null)
            childrenList.Add(KeywordToken);
            
        if (OpenBraceToken is not null)
            childrenList.Add(OpenBraceToken);
            
        if (CodeBlockNode is not null)
            childrenList.Add(CodeBlockNode);

        ChildList = childrenList.ToImmutableArray();
    }
}
