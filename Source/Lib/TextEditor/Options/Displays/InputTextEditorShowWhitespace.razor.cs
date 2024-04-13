using Fluxor;
using Fluxor.Blazor.Web.Components;
using Luthetus.TextEditor.RazorLib.Options.States;
using Microsoft.AspNetCore.Components;

namespace Luthetus.TextEditor.RazorLib.Options.Displays;

public partial class InputTextEditorShowWhitespace : FluxorComponent
{
    [Inject]
    private IState<TextEditorOptionsState> TextEditorOptionsStateWrap { get; set; } = null!;
    [Inject]
    private ITextEditorService TextEditorService { get; set; } = null!;

    [CascadingParameter(Name = "InputElementCssClass")]
    public string CascadingInputElementCssClass { get; set; } = string.Empty;

    [Parameter]
    public string TopLevelDivElementCssClassString { get; set; } = string.Empty;
    [Parameter]
    public string InputElementCssClassString { get; set; } = string.Empty;
    [Parameter]
    public string LabelElementCssClassString { get; set; } = string.Empty;

    public bool GlobalShowWhitespace
    {
        get => TextEditorOptionsStateWrap.Value.Options.ShowWhitespace;
        set => TextEditorService.OptionsApi.SetShowWhitespace(value);
    }
}