using System.Collections.Immutable;
using Luthetus.TextEditor.RazorLib.CompilerServices.GenericLexer.SyntaxEnums;
using Luthetus.TextEditor.RazorLib.Lexers.Models;

namespace Luthetus.TextEditor.RazorLib.CompilerServices.GenericLexer.SyntaxObjects;

public class GenericStringSyntax : IGenericSyntax
{
    public GenericStringSyntax(TextEditorTextSpan textSpan)
    {
        TextSpan = textSpan;
    }

    public TextEditorTextSpan TextSpan { get; }
    public ImmutableArray<IGenericSyntax> ChildList => ImmutableArray<IGenericSyntax>.Empty;
    public GenericSyntaxKind GenericSyntaxKind => GenericSyntaxKind.StringLiteral;
}