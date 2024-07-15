using System.Collections.Immutable;
using Luthetus.TextEditor.RazorLib.Lexes.Models;
using Luthetus.CompilerServices.Css.Css.SyntaxEnums;

namespace Luthetus.CompilerServices.Css.Css.SyntaxObjects;

public class CssPropertyValueSyntax : ICssSyntax
{
    public CssPropertyValueSyntax(
        TextEditorTextSpan textEditorTextSpan,
        ImmutableArray<ICssSyntax> childCssSyntaxes)
    {
        ChildCssSyntaxes = childCssSyntaxes;
        TextEditorTextSpan = textEditorTextSpan;
    }

    public TextEditorTextSpan TextEditorTextSpan { get; }
    public ImmutableArray<ICssSyntax> ChildCssSyntaxes { get; }

    public CssSyntaxKind CssSyntaxKind => CssSyntaxKind.PropertyValue;
}