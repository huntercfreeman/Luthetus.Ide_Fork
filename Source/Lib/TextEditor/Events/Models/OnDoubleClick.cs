using Luthetus.Common.RazorLib.Keys.Models;
using Luthetus.Common.RazorLib.BackgroundTasks.Models;
using Luthetus.TextEditor.RazorLib.Cursors.Models;
using Luthetus.TextEditor.RazorLib.Lexes.Models;
using Luthetus.TextEditor.RazorLib.TextEditors.Displays;
using Microsoft.AspNetCore.Components.Web;
using Luthetus.TextEditor.RazorLib.TextEditors.Models;
using Luthetus.TextEditor.RazorLib.BackgroundTasks.Models;

namespace Luthetus.TextEditor.RazorLib.Events.Models;

public class OnDoubleClick : ITextEditorTask
{
    private readonly TextEditorViewModelDisplay.TextEditorEvents _events;

    public OnDoubleClick(
        MouseEventArgs mouseEventArgs,
        TextEditorViewModelDisplay.TextEditorEvents events,
        ResourceUri resourceUri,
        Key<TextEditorViewModel> viewModelKey)
    {
        _events = events;

        MouseEventArgs = mouseEventArgs;
        ResourceUri = resourceUri;
        ViewModelKey = viewModelKey;
    }

    public Key<BackgroundTask> BackgroundTaskKey { get; } = Key<BackgroundTask>.NewKey();
    public Key<BackgroundTaskQueue> QueueKey { get; } = ContinuousBackgroundTaskWorker.GetQueueKey();
    public string Name { get; } = nameof(OnDoubleClick);
    public Task? WorkProgress { get; }
    public MouseEventArgs MouseEventArgs { get; }
    public ResourceUri ResourceUri { get; }
    public Key<TextEditorViewModel> ViewModelKey { get; }

	public IEditContext EditContext { get; set; }

    public TimeSpan ThrottleTimeSpan => TextEditorViewModelDisplay.TextEditorEvents.ThrottleDelayDefault;

    public IBackgroundTask? BatchOrDefault(IBackgroundTask oldEvent)
    {
		if (oldEvent is OnDoubleClick)
		{
			// Replace the upstream event with this one,
			// because unhandled-consecutive events of this type are redundant.
			return this;
		}
        
		// Keep both events, because they are not able to be batched.
		return null;
    }

    public async Task HandleEvent(CancellationToken cancellationToken)
    {
		try
		{
            var modelModifier = EditContext.GetModelModifier(ResourceUri, true);
            var viewModelModifier = EditContext.GetViewModelModifier(ViewModelKey);
            var cursorModifierBag = EditContext.GetCursorModifierBag(viewModelModifier?.ViewModel);
            var primaryCursorModifier = EditContext.GetPrimaryCursorModifier(cursorModifierBag);

            if (modelModifier is null || viewModelModifier is null || cursorModifierBag is null || primaryCursorModifier is null)
                return;

            var hasSelectedText = TextEditorSelectionHelper.HasSelectedText(primaryCursorModifier);

            if ((MouseEventArgs.Buttons & 1) != 1 && hasSelectedText)
                return; // Not pressing the left mouse button so assume ContextMenu is desired result.

            if (MouseEventArgs.ShiftKey)
                return; // Do not expand selection if user is holding shift

			// Labeling any IEditContext -> JavaScript interop or Blazor StateHasChanged.
			// Reason being, these are likely to be huge optimizations (2024-05-29).
            var rowAndColumnIndex = await _events.CalculateRowAndColumnIndex(MouseEventArgs).ConfigureAwait(false);

            var lowerColumnIndexExpansion = modelModifier.GetColumnIndexOfCharacterWithDifferingKind(
                rowAndColumnIndex.rowIndex,
                rowAndColumnIndex.columnIndex,
                true);

            lowerColumnIndexExpansion = lowerColumnIndexExpansion == -1
                ? 0
                : lowerColumnIndexExpansion;

            var higherColumnIndexExpansion = modelModifier.GetColumnIndexOfCharacterWithDifferingKind(
                rowAndColumnIndex.rowIndex,
                rowAndColumnIndex.columnIndex,
                false);

            higherColumnIndexExpansion = higherColumnIndexExpansion == -1
                ? modelModifier.GetLineLength(rowAndColumnIndex.rowIndex)
                : higherColumnIndexExpansion;

            // Move user's cursor position to the higher expansion
            {
                primaryCursorModifier.LineIndex = rowAndColumnIndex.rowIndex;
                primaryCursorModifier.ColumnIndex = higherColumnIndexExpansion;
                primaryCursorModifier.PreferredColumnIndex = rowAndColumnIndex.columnIndex;
            }

            // Set text selection ending to higher expansion
            {
                var cursorPositionOfHigherExpansion = modelModifier.GetPositionIndex(
                    rowAndColumnIndex.rowIndex,
                    higherColumnIndexExpansion);

                primaryCursorModifier.SelectionEndingPositionIndex = cursorPositionOfHigherExpansion;
            }

            // Set text selection anchor to lower expansion
            {
                var cursorPositionOfLowerExpansion = modelModifier.GetPositionIndex(
                    rowAndColumnIndex.rowIndex,
                    lowerColumnIndexExpansion);

                primaryCursorModifier.SelectionAnchorPositionIndex = cursorPositionOfLowerExpansion;
            }
		}
		finally
		{
			await EditContext.TextEditorService.FinalizePost(EditContext);
		}
    }
}