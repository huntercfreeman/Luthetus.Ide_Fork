using System.Collections.Immutable;
using Luthetus.TextEditor.RazorLib.CompilerServices.Syntax.Tokens;
using Luthetus.TextEditor.RazorLib.CompilerServices.Syntax.Nodes.Interfaces;
using Luthetus.TextEditor.RazorLib.CompilerServices.Syntax.Nodes.Enums;
using Luthetus.TextEditor.RazorLib.CompilerServices.Interfaces;

namespace Luthetus.TextEditor.RazorLib.CompilerServices.Syntax.Nodes;

public sealed record ConstructorDefinitionNode : ICodeBlockOwner
{
    public ConstructorDefinitionNode(
        TypeClauseNode returnTypeClauseNode,
        IdentifierToken functionIdentifier,
        GenericArgumentsListingNode? genericArgumentsListingNode,
        FunctionArgumentsListingNode functionArgumentsListingNode,
        CodeBlockNode? codeBlockNode,
        ConstraintNode? constraintNode)
    {
        ReturnTypeClauseNode = returnTypeClauseNode;
        FunctionIdentifier = functionIdentifier;
        GenericArgumentsListingNode = genericArgumentsListingNode;
        FunctionArgumentsListingNode = functionArgumentsListingNode;
        CodeBlockNode = codeBlockNode;
        ConstraintNode = constraintNode;

        SetChildList();
    }

    public TypeClauseNode ReturnTypeClauseNode { get; }
    public IdentifierToken FunctionIdentifier { get; }
    public GenericArgumentsListingNode? GenericArgumentsListingNode { get; }
    public FunctionArgumentsListingNode FunctionArgumentsListingNode { get; }
    public CodeBlockNode? CodeBlockNode { get; private set; }
    public ConstraintNode? ConstraintNode { get; }
    public OpenBraceToken OpenBraceToken { get; private set; }

	public ScopeDirectionKind ScopeDirectionKind => ScopeDirectionKind.Down;

    public ImmutableArray<ISyntax> ChildList { get; private set; }
    public ISyntaxNode? Parent { get; }

    public bool IsFabricated { get; init; }
    public SyntaxKind SyntaxKind => SyntaxKind.ConstructorDefinitionNode;
    
    public TypeClauseNode? GetReturnTypeClauseNode()
    {
    	return ReturnTypeClauseNode;
    }
    
    public ICodeBlockOwner WithCodeBlockNode(OpenBraceToken openBraceToken, CodeBlockNode codeBlockNode)
    {
    	OpenBraceToken = openBraceToken;
    	CodeBlockNode = codeBlockNode;
    	return this;
    }
    
    public void OnBoundScopeCreatedAndSetAsCurrent(IParserModel parserModel)
    {
    	// Do nothing.
    	return;
    }
    
    public void SetChildList()
    {
    	var children = new List<ISyntax>
        {
            ReturnTypeClauseNode,
            FunctionIdentifier,
        };

        if (GenericArgumentsListingNode is not null)
            children.Add(GenericArgumentsListingNode);

        children.Add(FunctionArgumentsListingNode);

        if (CodeBlockNode is not null)
            children.Add(CodeBlockNode);
        
        if (ConstraintNode is not null)
            children.Add(ConstraintNode);

        ChildList = children.ToImmutableArray();
    }
}
