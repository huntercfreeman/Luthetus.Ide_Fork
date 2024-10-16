using System.Collections.Immutable;

namespace Luthetus.TextEditor.RazorLib.CompilerServices.Syntax.Nodes;

/// <summary>
/// Used when defining a syntax which contains a generic type.
/// </summary>
public sealed class GenericArgumentEntryNode : ISyntaxNode
{
    public GenericArgumentEntryNode(TypeClauseNode typeClauseNode)
    {
        TypeClauseNode = typeClauseNode;

        SetChildList();
    }

    public TypeClauseNode TypeClauseNode { get; }

    public ISyntax[] ChildList { get; private set; }
    public ISyntaxNode? Parent { get; }

    public bool IsFabricated { get; init; }
    public SyntaxKind SyntaxKind => SyntaxKind.GenericArgumentEntryNode;
    
    public void SetChildList()
    {
    	var children = new List<ISyntax>
        {
            TypeClauseNode
        };

        ChildList = children.ToImmutableArray();
    	throw new NotImplementedException();
    }
}