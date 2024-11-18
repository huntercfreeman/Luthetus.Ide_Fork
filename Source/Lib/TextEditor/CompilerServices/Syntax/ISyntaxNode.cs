using System.Collections.Immutable;

namespace Luthetus.TextEditor.RazorLib.CompilerServices.Syntax;

public interface ISyntaxNode : ISyntax
{
    public ISyntax[] GetChildList();
}