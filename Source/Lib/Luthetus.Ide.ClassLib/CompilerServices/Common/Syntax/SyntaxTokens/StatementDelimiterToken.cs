﻿using Luthetus.TextEditor.RazorLib.Lexing;

namespace Luthetus.Ide.ClassLib.CompilerServices.Common.Syntax.SyntaxTokens;

public class StatementDelimiterToken : ISyntaxToken
{
    public StatementDelimiterToken(
        TextEditorTextSpan textEditorTextSpan)
    {
        TextSpan = textEditorTextSpan;
    }

    public TextEditorTextSpan TextSpan { get; }
    public SyntaxKind SyntaxKind => SyntaxKind.StatementDelimiterToken;
}