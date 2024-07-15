using System.Collections.Immutable;
using Luthetus.TextEditor.RazorLib.Lexes.Models;
using Luthetus.CompilerServices.Json.Json.SyntaxEnums;

namespace Luthetus.CompilerServices.Json.Json;

public interface IJsonSyntax
{
    public JsonSyntaxKind JsonSyntaxKind { get; }
    public TextEditorTextSpan TextEditorTextSpan { get; }
    public ImmutableArray<IJsonSyntax> ChildJsonSyntaxes { get; }
}