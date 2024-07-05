using Luthetus.Common.RazorLib.Dimensions.Models;
using Luthetus.Common.RazorLib.Dynamics.Models;
using Luthetus.Common.RazorLib.Keys.Models;
using Luthetus.Common.RazorLib.Contexts.Models;

namespace Luthetus.Common.RazorLib.Dialogs.Models;

public record DialogViewModel : IDialog
{
	public DialogViewModel(
		Key<IDynamicViewModel> dynamicViewModelKey,
		string title,
		Type componentType,
		Dictionary<string, object?>? componentParameterMap,
		string? cssClass,
		bool isResizable,
		string? setFocusOnCloseElementId)
	{
		DynamicViewModelKey = dynamicViewModelKey;
		Title = title;
        ComponentType = componentType;
        ComponentParameterMap = componentParameterMap;
        DialogCssClass = cssClass;
		DialogIsResizable = isResizable;

		DialogFocusPointHtmlElementId = $"luth_dialog-focus-point_{DynamicViewModelKey.Guid}";
		SetFocusOnCloseElementId = setFocusOnCloseElementId;
	}

	public Key<IDynamicViewModel> DynamicViewModelKey { get; }
	public ElementDimensions DialogElementDimensions { get; set; } = DialogHelper.ConstructDefaultElementDimensions();
    public Type ComponentType { get; }
    public Dictionary<string, object?>? ComponentParameterMap { get; init; }
	public string Title { get; set; }
	public string TitleVerbose => Title;
    public string DialogFocusPointHtmlElementId { get; init; }
    public string? DialogCssClass { get; set; }
    public string? DialogCssStyle { get; set; }
	public bool DialogIsMinimized { get; set; }
    public bool DialogIsMaximized { get; set; }
    public bool DialogIsResizable { get; set; }
    public string? SetFocusOnCloseElementId { get; set; }

	public IDialog SetTitle(string title)
	{
		return this with { Title = title };
	}

	public IDialog SetIsMinimized(bool isMinimized)
	{
		return this with { DialogIsMinimized = isMinimized };
	}

	public IDialog SetDialogIsMaximized(bool isMaximized)
	{
		return this with { DialogIsMaximized = isMaximized };
	}

	public IDialog SetIsResizable(bool isResizable)
	{
		return this with { DialogIsResizable = isResizable };
	}

	public IDialog SetCssClassString(string cssClassString)
	{
		return this with { DialogCssClass = cssClassString };
	}
}
