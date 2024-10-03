using System.Collections.Immutable;
using Microsoft.AspNetCore.Components.Web;
using Luthetus.Common.RazorLib.Keys.Models;
using Luthetus.TextEditor.RazorLib.Cursors.Models;
using Luthetus.TextEditor.RazorLib.JavaScriptObjects.Models;
using Luthetus.TextEditor.RazorLib.Lexers.Models;
using Luthetus.Common.RazorLib.Keymaps.Models;

namespace Luthetus.TextEditor.RazorLib.TextEditors.Models;

public interface ITextEditorViewModelApi
{
    #region CREATE_METHODS
    public void Register(
        Key<TextEditorViewModel> textEditorViewModelKey,
        ResourceUri resourceUri,
        Category category);
        
    public void Register(TextEditorViewModel viewModel);
    #endregion

    #region READ_METHODS
    /// <summary>
    /// Returns a shallow copy
	///
    /// One should store the result of invoking this method in a variable, then reference that variable.
    /// If one continually invokes this, there is no guarantee that the data had not changed
    /// since the previous invocation.
    /// </summary>
    public Dictionary<Key<TextEditorViewModel>, TextEditorViewModel> GetViewModels();
    public TextEditorModel? GetModelOrDefault(Key<TextEditorViewModel> viewModelKey);
    public Task<TextEditorDimensions> GetTextEditorMeasurementsAsync(string elementId);

    public Task<CharAndLineMeasurements> MeasureCharacterWidthAndLineHeightAsync(
        string measureCharacterWidthAndLineHeightElementId,
        int countOfTestCharacters);

    public TextEditorViewModel? GetOrDefault(Key<TextEditorViewModel> viewModelKey);
    public string? GetAllText(Key<TextEditorViewModel> viewModelKey);

    public void SetCursorShouldBlink(bool cursorShouldBlink);
    #endregion

    #region UPDATE_METHODS
    /// <summary>
    /// If a parameter is null the JavaScript will not modify that value
    /// </summary>
    public void SetScrollPosition(
        ITextEditorEditContext editContext,
        TextEditorViewModelModifier viewModelModifier,
        double? scrollLeftInPixels,
        double? scrollTopInPixels);

    public void ScrollIntoView(
        ITextEditorEditContext editContext,
        TextEditorModelModifier modelModifier,
        TextEditorViewModelModifier viewModelModifier,
        TextEditorTextSpan textSpan);

    public void MutateScrollVerticalPosition(
        ITextEditorEditContext editContext,
        TextEditorViewModelModifier viewModelModifier,
        double pixels);

    public void MutateScrollHorizontalPosition(
        ITextEditorEditContext editContext,
        TextEditorViewModelModifier viewModelModifier,
        double pixels);

    public Task FocusPrimaryCursorAsync(string primaryCursorContentId);

    public void MoveCursor(
    	KeymapArgs keymapArgs,
        ITextEditorEditContext editContext,
        TextEditorModelModifier modelModifier,
        TextEditorViewModelModifier viewModelModifier,
        CursorModifierBagTextEditor cursorModifierBag);

    /// <summary>
    /// If one wants to guarantee that the state is up to date use <see cref="MoveCursor"/>
    /// instead of this method. This is because, the <see cref="ITextEditorService"/> will provide
    /// you the latest instance of the given <see cref="TextEditorCursor"/>. As opposed to whatever
    /// instance of the <see cref="TextEditorCursorModifier"/> you have at time of enqueueing.
    /// <br/><br/>
    /// This method is needed however, because if one wants to arbitrarily create a cursor that does not
    /// map to the view model's cursors, then one would use this method. Since an attempt to map
    /// the cursor key would come back as the cursor not existing.
    /// </summary>
    public void MoveCursorUnsafe(
        KeymapArgs keymapArgs,
        ITextEditorEditContext editContext,
        TextEditorModelModifier modelModifier,
        TextEditorViewModelModifier viewModelModifier,
        CursorModifierBagTextEditor cursorModifierBag,
        TextEditorCursorModifier primaryCursor);

    public void CursorMovePageTop(
        ITextEditorEditContext editContext,
        TextEditorViewModelModifier viewModelModifier,
        CursorModifierBagTextEditor cursorModifierBag);

    /// <summary>
    /// If one wants to guarantee that the state is up to date use <see cref="CursorMovePageTopFactory"/>
    /// instead of this method. This is because, the <see cref="ITextEditorService"/> will provide
    /// you the latest instance of the given <see cref="TextEditorCursor"/>. As opposed to whatever
    /// instance of the <see cref="TextEditorCursorModifier"/> you have at time of enqueueing.
    /// <br/><br/>
    /// This method is needed however, because if one wants to arbitrarily create a cursor that does not
    /// map to the view model's cursors, then one would use this method. Since an attempt to map
    /// the cursor key would come back as the cursor not existing.
    /// </summary>
    public void CursorMovePageTopUnsafe(
    	ITextEditorEditContext editContext,
        TextEditorViewModelModifier viewModelModifier,
		CursorModifierBagTextEditor cursorModifierBag,
        TextEditorCursorModifier primaryCursor);

    public void CursorMovePageBottom(
        ITextEditorEditContext editContext,
        TextEditorModelModifier modelModifier,
        TextEditorViewModelModifier viewModelModifier,
        CursorModifierBagTextEditor cursorModifierBag);

    /// <summary>
    /// If one wants to guarantee that the state is up to date use <see cref="CursorMovePageBottomFactory"/>
    /// instead of this method. This is because, the <see cref="ITextEditorService"/> will provide
    /// you the latest instance of the given <see cref="TextEditorCursor"/>. As opposed to whatever
    /// instance of the <see cref="TextEditorCursorModifier"/> you have at time of enqueueing.
    /// <br/><br/>
    /// This method is needed however, because if one wants to arbitrarily create a cursor that does not
    /// map to the view model's cursors, then one would use this method. Since an attempt to map
    /// the cursor key would come back as the cursor not existing.
    /// </summary>
    public void CursorMovePageBottomUnsafe(
        ITextEditorEditContext editContext,
        TextEditorModelModifier modelModifier,
        TextEditorViewModelModifier viewModelModifier,
        CursorModifierBagTextEditor cursorModifierBag,
        TextEditorCursorModifier cursorModifier);
        
    public void RevealCursor(
        ITextEditorEditContext editContext,
        TextEditorModelModifier modelModifier,
        TextEditorViewModelModifier viewModelModifier,
        CursorModifierBagTextEditor cursorModifierBag,
        TextEditorCursorModifier cursorModifier);

    public void CalculateVirtualizationResult(
        ITextEditorEditContext editContext,
        TextEditorModelModifier modelModifier,
        TextEditorViewModelModifier viewModelModifier,
        CancellationToken cancellationToken);

    public Task RemeasureAsync(
        ITextEditorEditContext editContext,
        TextEditorViewModelModifier viewModelModifier,
        string measureCharacterWidthAndLineHeightElementId,
        int countOfTestCharacters,
        CancellationToken cancellationToken);

    public void ForceRender(
        ITextEditorEditContext editContext,
        TextEditorViewModelModifier viewModelModifier,
        CancellationToken cancellationToken);
    #endregion

    #region DELETE_METHODS
    public void Dispose(Key<TextEditorViewModel> viewModelKey);
    #endregion
    
    public bool CursorShouldBlink { get; }
    public event Action? CursorShouldBlinkChanged;
}
