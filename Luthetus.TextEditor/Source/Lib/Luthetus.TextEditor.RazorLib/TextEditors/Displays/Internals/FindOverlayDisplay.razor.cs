using Luthetus.Common.RazorLib.Keyboards.Models;
using Luthetus.Common.RazorLib.Reactives.Models;
using Luthetus.TextEditor.RazorLib.TextEditors.Models.Internals;
using Luthetus.TextEditor.RazorLib.TextEditors.Models.TextEditorServices;
using Luthetus.TextEditor.RazorLib.TextEditors.Models.TextEditorModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Collections.Immutable;
using Luthetus.TextEditor.RazorLib.Lexes.Models;

namespace Luthetus.TextEditor.RazorLib.TextEditors.Displays.Internals;

public partial class FindOverlayDisplay : ComponentBase
{
    [Inject]
    private ITextEditorService TextEditorService { get; set; } = null!;
    [Inject]
    private IJSRuntime JsRuntime { get; set; } = null!;

    [CascadingParameter]
    public TextEditorRenderBatch RenderBatch { get; set; } = null!;

    private bool _lastSeenShowFindOverlayValue = false;
    private string _inputValue = string.Empty;
    private int? _activeIndexMatchedTextSpan = null;

    private IThrottle _throttleInputValueChange = new Throttle(TimeSpan.FromMilliseconds(150));
    private TextEditorTextSpan? _decorationByteChangedTargetTextSpan;

    private string InputValue
    {
        get => _inputValue;
        set
        {
            _inputValue = value;

            _ = Task.Run(async () =>
            {
                await _throttleInputValueChange.FireAsync(_ =>
                {
                    TextEditorService.Post(
                        nameof(FindOverlayDisplay),
                        async editContext =>
                        {
                            var viewModelModifier = editContext.GetViewModelModifier(RenderBatch.ViewModel!.ViewModelKey);

                            if (viewModelModifier is null)
                                return;

                            var localInputValue = _inputValue;

                            viewModelModifier.ViewModel = viewModelModifier.ViewModel with
                            {
                                FindOverlayValue = localInputValue,
                            };

                            var modelModifier = editContext.GetModelModifier(RenderBatch.Model!.ResourceUri);

                            if (modelModifier is null)
                                return;

                            var textSpanMatches = modelModifier.FindMatches(localInputValue);

                            await TextEditorService.ModelApi.CalculatePresentationModelFactory(
                                modelModifier.ResourceUri,
                                FindOverlayPresentationFacts.PresentationKey)
                                .Invoke(editContext)
                                .ConfigureAwait(false);

                            var pendingCalculation = modelModifier.PresentationModelsList.FirstOrDefault(x =>
                                x.TextEditorPresentationKey == FindOverlayPresentationFacts.PresentationKey)
                                ?.PendingCalculation;

                            if (pendingCalculation is null)
                                pendingCalculation = new(modelModifier.GetAllText());

                            var presentationModel = modelModifier.PresentationModelsList.FirstOrDefault(x =>
                                x.TextEditorPresentationKey == FindOverlayPresentationFacts.PresentationKey);

                            if (presentationModel?.PendingCalculation is not null)
                            {
                                presentationModel.PendingCalculation.TextSpanList = textSpanMatches;

                                (presentationModel.CompletedCalculation, presentationModel.PendingCalculation) =
                                    (presentationModel.PendingCalculation, presentationModel.CompletedCalculation);
                            }
                        });

                    return Task.CompletedTask;
                }).ConfigureAwait(false);
            });
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (_lastSeenShowFindOverlayValue != RenderBatch.ViewModel!.ShowFindOverlay)
        {
            _lastSeenShowFindOverlayValue = RenderBatch.ViewModel!.ShowFindOverlay;

            // If it changes from 'false' to 'true', focus the input element
            if (_lastSeenShowFindOverlayValue)
            {
                _ = await JsRuntime.InvokeAsync<bool>(
                        "luthetusIde.tryFocusHtmlElementById",
                        RenderBatch.ViewModel!.FindOverlayId)
                    .ConfigureAwait(false);
            }
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task HandleOnKeyDownAsync(KeyboardEventArgs keyboardEventArgs)
    {
        if (keyboardEventArgs.Key == KeyboardKeyFacts.MetaKeys.ESCAPE)
        {
            await JsRuntime.InvokeVoidAsync(
                    "luthetusTextEditor.focusHtmlElementById",
                    RenderBatch.ViewModel!.PrimaryCursorContentId)
            .ConfigureAwait(false);

            TextEditorService.Post(
                nameof(FindOverlayDisplay),
                async editContext =>
                {
                    var viewModelModifier = editContext.GetViewModelModifier(RenderBatch.ViewModel!.ViewModelKey);

                    if (viewModelModifier is null)
                        return;

                    viewModelModifier.ViewModel = viewModelModifier.ViewModel with
                    {
                        ShowFindOverlay = false,
                    };

                    var modelModifier = editContext.GetModelModifier(RenderBatch.Model!.ResourceUri);

                    if (modelModifier is null)
                        return;

                    await TextEditorService.ModelApi.CalculatePresentationModelFactory(
                        modelModifier.ResourceUri,
                        FindOverlayPresentationFacts.PresentationKey)
                        .Invoke(editContext)
                        .ConfigureAwait(false);

                    var pendingCalculation = modelModifier.PresentationModelsList.FirstOrDefault(x =>
                        x.TextEditorPresentationKey == FindOverlayPresentationFacts.PresentationKey)
                        ?.PendingCalculation;

                    if (pendingCalculation is null)
                        pendingCalculation = new(modelModifier.GetAllText());

                    var presentationModel = modelModifier.PresentationModelsList.FirstOrDefault(x =>
                        x.TextEditorPresentationKey == FindOverlayPresentationFacts.PresentationKey);

                    if (presentationModel?.PendingCalculation is not null)
                    {
                        presentationModel.PendingCalculation.TextSpanList = ImmutableArray<TextEditorTextSpan>.Empty;

                        (presentationModel.CompletedCalculation, presentationModel.PendingCalculation) =
                            (presentationModel.PendingCalculation, presentationModel.CompletedCalculation);
                    }
                });
        }
    }

    private void MoveActiveIndexMatchedTextSpanUp()
    {
        var findOverlayPresentationModel = RenderBatch.Model!.PresentationModelsList.FirstOrDefault(
            x => x.TextEditorPresentationKey == FindOverlayPresentationFacts.PresentationKey);

        if (findOverlayPresentationModel is null)
            return;

        var completedCalculation = findOverlayPresentationModel.CompletedCalculation;

        if (completedCalculation is null)
            return;

        if (_activeIndexMatchedTextSpan is null)
        {
            _activeIndexMatchedTextSpan = completedCalculation.TextSpanList.Length - 1;
        }
        else
        {
            _activeIndexMatchedTextSpan--;

            if (_activeIndexMatchedTextSpan == -1)
                _activeIndexMatchedTextSpan = completedCalculation.TextSpanList.Length - 1;
        }

        HandleActiveIndexMatchedTextSpanChanged();
    }

    private void MoveActiveIndexMatchedTextSpanDown()
    {
        var findOverlayPresentationModel = RenderBatch.Model!.PresentationModelsList.FirstOrDefault(
            x => x.TextEditorPresentationKey == FindOverlayPresentationFacts.PresentationKey);

        if (findOverlayPresentationModel is null)
            return;

        var completedCalculation = findOverlayPresentationModel.CompletedCalculation;

        if (completedCalculation is null)
            return;

        if (_activeIndexMatchedTextSpan is null)
        {
            _activeIndexMatchedTextSpan = 0;
        }
        else
        {
            _activeIndexMatchedTextSpan++;

            if (_activeIndexMatchedTextSpan == completedCalculation.TextSpanList.Length)
                _activeIndexMatchedTextSpan = 0;
        }

        HandleActiveIndexMatchedTextSpanChanged();
    }

    private void HandleActiveIndexMatchedTextSpanChanged()
    {
        TextEditorService.Post(
            nameof(HandleActiveIndexMatchedTextSpanChanged),
            editContext =>
            {
                var localActiveIndexMatchedTextSpan = _activeIndexMatchedTextSpan;

                if (localActiveIndexMatchedTextSpan is null)
                    return Task.CompletedTask;

                var viewModelModifier = editContext.GetViewModelModifier(RenderBatch.ViewModel!.ViewModelKey);

                if (viewModelModifier is null)
                    return Task.CompletedTask;
                
                var modelModifier = editContext.GetModelModifier(RenderBatch.Model!.ResourceUri);

                if (modelModifier is null)
                    return Task.CompletedTask;

                var presentationModel = modelModifier.PresentationModelsList.FirstOrDefault(x =>
                    x.TextEditorPresentationKey == FindOverlayPresentationFacts.PresentationKey);

                if (presentationModel?.CompletedCalculation is not null)
                {
                    if (_decorationByteChangedTargetTextSpan is not null)
                    {
                        var needsColorResetSinceNoLongerActive = presentationModel.CompletedCalculation.TextSpanList.FirstOrDefault(x =>
                            x.StartingIndexInclusive == _decorationByteChangedTargetTextSpan.StartingIndexInclusive &&
                            x.EndingIndexExclusive == _decorationByteChangedTargetTextSpan.EndingIndexExclusive &&
                            x.ResourceUri == _decorationByteChangedTargetTextSpan.ResourceUri &&
                            x.GetText() == _decorationByteChangedTargetTextSpan.GetText());

                        if (needsColorResetSinceNoLongerActive is not null)
                        {
                            presentationModel.CompletedCalculation.TextSpanList = presentationModel.CompletedCalculation.TextSpanList.Replace(needsColorResetSinceNoLongerActive, needsColorResetSinceNoLongerActive with
                            {
                                DecorationByte = _decorationByteChangedTargetTextSpan.DecorationByte
                            });
                        }
                    }

                    var targetTextSpan = presentationModel.CompletedCalculation.TextSpanList[localActiveIndexMatchedTextSpan.Value];
                    _decorationByteChangedTargetTextSpan = targetTextSpan;

                    presentationModel.CompletedCalculation.TextSpanList =
                        presentationModel.CompletedCalculation.TextSpanList.Replace(targetTextSpan, targetTextSpan with
                        {
                            DecorationByte = (byte)TextEditorFindOverlayDecorationKind.Insertion,
                        });

                    
                }

                return Task.CompletedTask;
            });
    }
}