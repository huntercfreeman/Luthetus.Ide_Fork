﻿using Luthetus.Common.RazorLib.Keys.Models;
using Luthetus.TextEditor.RazorLib.TextEditors.Models;
using System.Collections.Immutable;

namespace Luthetus.TextEditor.RazorLib.Groups.Models;

public interface ITextEditorGroupApi
{
    public void AddViewModel(Key<TextEditorGroup> groupKey, Key<TextEditorViewModel> viewModelKey);
    public TextEditorGroup? GetOrDefault(Key<TextEditorGroup> groupKey);
    public void Register(Key<TextEditorGroup> groupKey);
    public void Dispose(Key<TextEditorGroup> groupKey);
    public void RemoveViewModel(Key<TextEditorGroup> groupKey, Key<TextEditorViewModel> viewModelKey);
    public void SetActiveViewModel(Key<TextEditorGroup> groupKey, Key<TextEditorViewModel> viewModelKey);

    /// <summary>
    /// One should store the result of invoking this method in a variable, then reference that variable.
    /// If one continually invokes this, there is no guarantee that the data had not changed
    /// since the previous invocation.
    /// </summary>
    public ImmutableList<TextEditorGroup> GetGroups();
}
