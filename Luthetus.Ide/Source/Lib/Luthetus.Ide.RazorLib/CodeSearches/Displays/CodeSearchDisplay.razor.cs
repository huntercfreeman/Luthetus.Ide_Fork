using Fluxor;
using Fluxor.Blazor.Web.Components;
using Luthetus.Ide.RazorLib.CodeSearches.Models;
using Luthetus.Ide.RazorLib.DotNetSolutions.States;
using Luthetus.Ide.RazorLib.CodeSearches.States;
using Luthetus.TextEditor.RazorLib.Installations.Models;
using Microsoft.AspNetCore.Components;
using Luthetus.TextEditor.RazorLib.Lexes.Models;
using Luthetus.TextEditor.RazorLib.TextEditors.Models;
using Luthetus.Common.RazorLib.Keys.Models;
using Luthetus.TextEditor.RazorLib.TextEditors.Models.TextEditorServices;
using Luthetus.Common.RazorLib.Dimensions.Models;
using Luthetus.Common.RazorLib.Resizes.Displays;
using Luthetus.TextEditor.RazorLib.TextEditors.Models.Internals;

namespace Luthetus.Ide.RazorLib.CodeSearches.Displays;

public partial class CodeSearchDisplay : FluxorComponent
{
	[Inject]
	private IState<CodeSearchState> CodeSearchStateWrap { get; set; } = null!;
	[Inject]
	private IState<DotNetSolutionState> DotNetSolutionStateWrap { get; set; } = null!;
	[Inject]
	private IDispatcher Dispatcher { get; set; } = null!;
	[Inject]
	private LuthetusTextEditorConfig TextEditorConfig { get; set; } = null!;
	[Inject]
	private ITextEditorService TextEditorService { get; set; } = null!;
	[Inject]
	private IServiceProvider ServiceProvider { get; set; } = null!;

	private readonly TextEditorViewModelDisplayOptions _textEditorViewModelDisplayOptions = new()
	{
		IncludeHeaderHelperComponent = false,
	};

    private ElementDimensions _topContentElementDimensions = new();
	private ElementDimensions _bottomContentElementDimensions = new();

    private string InputValue
	{
		get => CodeSearchStateWrap.Value.Query;
		set
		{
			if (value is null)
				value = string.Empty;

			Dispatcher.Dispatch(new CodeSearchState.WithAction(inState => inState with
			{
				Query = value,
			}));

			Dispatcher.Dispatch(new CodeSearchState.SearchEffect());
		}
	}

    protected override void OnInitialized()
    {
        // topContentHeight
        {
			var topContentHeight = _topContentElementDimensions.DimensionAttributeList.Single(
				da => da.DimensionAttributeKind == DimensionAttributeKind.Height);

			topContentHeight.DimensionUnitList.AddRange(new[]
			{
				new DimensionUnit
				{
					Value = 40,
					DimensionUnitKind = DimensionUnitKind.Percentage
				},
				new DimensionUnit
				{
					Value = ResizableRow.RESIZE_HANDLE_HEIGHT_IN_PIXELS / 2,
					DimensionUnitKind = DimensionUnitKind.Pixels,
					DimensionOperatorKind = DimensionOperatorKind.Subtract
				},
			});
        }

        // bottomContentHeight
        {
            var bottomContentHeight = _bottomContentElementDimensions.DimensionAttributeList.Single(
				da => da.DimensionAttributeKind == DimensionAttributeKind.Height);

				bottomContentHeight.DimensionUnitList.AddRange(new[]
				{
				new DimensionUnit
				{
					Value = 60,
					DimensionUnitKind = DimensionUnitKind.Percentage
				},
				new DimensionUnit
				{
					Value = ResizableRow.RESIZE_HANDLE_HEIGHT_IN_PIXELS / 2,
					DimensionUnitKind = DimensionUnitKind.Pixels,
					DimensionOperatorKind = DimensionOperatorKind.Subtract
				},
			});
        }

        base.OnInitialized();
    }

    protected override Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			var dotNetSolutionState = DotNetSolutionStateWrap.Value;
			var dotNetSolutionModel = dotNetSolutionState.DotNetSolutionModel;

			if (dotNetSolutionModel is not null)
			{
				var parentDirectory = dotNetSolutionModel.AbsolutePath.ParentDirectory;

				if (parentDirectory is not null)
				{
					Dispatcher.Dispatch(new CodeSearchState.WithAction(inState => inState with
					{
						StartingAbsolutePathForSearch = parentDirectory.Path
					}));
				}
			}
		}

		return base.OnAfterRenderAsync(firstRender);
	}

	private string GetIsActiveCssClass(CodeSearchFilterKind codeSearchFilterKind)
	{
		return CodeSearchStateWrap.Value.CodeSearchFilterKind == codeSearchFilterKind
			? "luth_active"
			: string.Empty;
	}

	private async Task HandleOnClick(string filePath)
	{
		var inPreviewViewModelKey = CodeSearchStateWrap.Value.PreviewViewModelKey;
		var outPreviewViewModelKey = Key<TextEditorViewModel>.NewKey();

        if (TextEditorConfig.RegisterViewModelFunc is null)
            return;

        await TextEditorConfig.RegisterViewModelFunc.Invoke(
            outPreviewViewModelKey,
            new ResourceUri(filePath),
            ServiceProvider);

        Dispatcher.Dispatch(new CodeSearchState.WithAction(inState => inState with
		{
			PreviewFilePath = filePath,
            PreviewViewModelKey = outPreviewViewModelKey,
        }));

		if (inPreviewViewModelKey != Key<TextEditorViewModel>.Empty)
			TextEditorService.ViewModelApi.Dispose(inPreviewViewModelKey);
    }
	
	private async Task HandleOnDoubleClick(string filePath)
	{
		if (TextEditorConfig.OpenInEditorAsyncFunc is null)
			return;

		await TextEditorConfig.OpenInEditorAsyncFunc
			.Invoke(filePath, ServiceProvider)
			.ConfigureAwait(false);
	}

	private async Task HandleResizableRowReRenderAsync()
	{
		await InvokeAsync(StateHasChanged).ConfigureAwait(false);
	}
}