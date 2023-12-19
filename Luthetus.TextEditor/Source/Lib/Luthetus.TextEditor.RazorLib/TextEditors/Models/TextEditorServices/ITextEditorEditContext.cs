﻿using Luthetus.TextEditor.RazorLib.Commands.Models;
using Luthetus.TextEditor.RazorLib.Cursors.Models;
using Luthetus.TextEditor.RazorLib.TextEditors.Models.TextEditorModels;
using static Luthetus.TextEditor.RazorLib.TextEditors.Models.TextEditorService;

namespace Luthetus.TextEditor.RazorLib.TextEditors.Models;

public interface ITextEditorEditContext
{
    public TextEditorCommandArgs CommandArgs { get; set; }
    public TextEditorModel Model { get; set; }
    public TextEditorViewModel ViewModel { get; set; }
    public RefreshCursorsRequest RefreshCursorsRequest { get; set; }
    public TextEditorCursorModifier PrimaryCursor { get; set; }
}

