using Luthetus.Common.RazorLib.Keys.Models;
using Luthetus.Common.RazorLib.BackgroundTasks.Models;
using Luthetus.TextEditor.RazorLib.TextEditors.Displays;
using Microsoft.AspNetCore.Components.Web;
using Luthetus.TextEditor.RazorLib.TextEditors.Models;
using Luthetus.TextEditor.RazorLib.BackgroundTasks.Models;

namespace Luthetus.TextEditor.RazorLib.Events.Models;

public class OnWheelBatch : ITextEditorTask
{
    private readonly TextEditorViewModelDisplay.TextEditorEvents _events;

    public OnWheelBatch(
        List<WheelEventArgs> wheelEventArgsList,
        TextEditorViewModelDisplay.TextEditorEvents events,
        Key<TextEditorViewModel> viewModelKey)
    {
        _events = events;

        WheelEventArgsList = wheelEventArgsList;
        ViewModelKey = viewModelKey;
    }

    public Key<BackgroundTask> BackgroundTaskKey { get; } = Key<BackgroundTask>.NewKey();
    public Key<BackgroundTaskQueue> QueueKey { get; } = ContinuousBackgroundTaskWorker.GetQueueKey();
    public string Name { get; private set; } = nameof(OnWheelBatch);
    public Task? WorkProgress { get; }
    public List<WheelEventArgs> WheelEventArgsList { get; }
    public Key<TextEditorViewModel> ViewModelKey { get; }

	public IEditContext EditContext { get; set; }

    public TimeSpan ThrottleTimeSpan => TextEditorViewModelDisplay.TextEditorEvents.ThrottleDelayDefault;

    public IBackgroundTask? BatchOrDefault(IBackgroundTask oldEvent)
    {
        return null;
    }

    public async Task HandleEvent(CancellationToken cancellationToken)
    {
		try
		{
			Name += $"_{WheelEventArgsList.Count}";

            var viewModelModifier = EditContext.GetViewModelModifier(ViewModelKey);

            if (viewModelModifier is null)
                return;

            double? horizontalMutateScrollPositionByPixels = null;
            double? verticalMutateScrollPositionByPixels = null;

            foreach (var wheelEventArgs in WheelEventArgsList)
            {
                if (wheelEventArgs.ShiftKey)
                {
                    horizontalMutateScrollPositionByPixels ??= 0;
                    horizontalMutateScrollPositionByPixels += wheelEventArgs.DeltaY;
                }
                else
                {
                    verticalMutateScrollPositionByPixels ??= 0;
                    verticalMutateScrollPositionByPixels += wheelEventArgs.DeltaY;
                }
            }

            if (horizontalMutateScrollPositionByPixels is not null)
            {
                await _events.TextEditorService.ViewModelApi.MutateScrollHorizontalPositionFactory(
                        viewModelModifier.ViewModel.ViewModelKey,
                        horizontalMutateScrollPositionByPixels.Value)
                    .Invoke(EditContext)
                    .ConfigureAwait(false);
            }

            if (verticalMutateScrollPositionByPixels is not null)
            {
                await _events.TextEditorService.ViewModelApi.MutateScrollVerticalPositionFactory(
                        viewModelModifier.ViewModel.ViewModelKey,
                        verticalMutateScrollPositionByPixels.Value)
                    .Invoke(EditContext)
                    .ConfigureAwait(false);
            }
		}
		finally
		{
			await EditContext.TextEditorService.FinalizePost(EditContext);
		}
    }
}