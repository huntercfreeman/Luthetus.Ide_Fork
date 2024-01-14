﻿using System.Collections.Immutable;
using Luthetus.TextEditor.RazorLib.CompilerServices.GenericLexer.SyntaxEnums;
using Luthetus.TextEditor.RazorLib.Lexes.Models;

namespace Luthetus.TextEditor.RazorLib.CompilerServices.GenericLexer.SyntaxObjects;

public class GenericPreprocessorDirectiveSyntax : IGenericSyntax
{
    public GenericPreprocessorDirectiveSyntax(
        TextEditorTextSpan textSpan,
        ImmutableArray<IGenericSyntax> childList)
    {
        TextSpan = textSpan;
        ChildList = childList;
    }

    public TextEditorTextSpan TextSpan { get; }
    public ImmutableArray<IGenericSyntax> ChildList { get; }
    public GenericSyntaxKind GenericSyntaxKind => GenericSyntaxKind.PreprocessorDirective;
}