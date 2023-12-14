using Xunit;
using Luthetus.TextEditor.RazorLib.TextEditors.Models;
using Fluxor;
using Luthetus.TextEditor.Tests.Basis.TextEditors.Models.TextEditorServices;

namespace Luthetus.TextEditor.Tests.Basis.TextEditors.Models;

/// <summary>
/// <see cref="TextEditorService"/>
/// </summary>
public class TextEditorServiceTests
{
	/// <summary>
	/// <see cref="TextEditorService(IState{RazorLib.TextEditors.States.TextEditorModelState}, IState{RazorLib.TextEditors.States.TextEditorViewModelState}, IState{RazorLib.Groups.States.TextEditorGroupState}, IState{RazorLib.Diffs.States.TextEditorDiffState}, IState{Common.RazorLib.Themes.States.ThemeState}, IState{RazorLib.Options.States.TextEditorOptionsState}, IState{RazorLib.Finds.States.TextEditorSearchEngineState}, RazorLib.Installations.Models.LuthetusTextEditorOptions, Common.RazorLib.Storages.Models.IStorageService, Microsoft.JSInterop.IJSRuntime, Common.RazorLib.Storages.States.StorageSync, IDispatcher)"/>
	/// <br/>----<br/>
	/// <see cref="TextEditorService.ModelStateWrap"/>
	/// <see cref="TextEditorService.ViewModelStateWrap"/>
	/// <see cref="TextEditorService.GroupStateWrap"/>
	/// <see cref="TextEditorService.DiffStateWrap"/>
	/// <see cref="TextEditorService.ThemeStateWrap"/>
	/// <see cref="TextEditorService.OptionsStateWrap"/>
	/// <see cref="TextEditorService.SearchEngineStateWrap"/>
	/// <see cref="TextEditorService.ThemeCssClassString"/>
	/// <see cref="TextEditorService.ModelApi"/>
	/// <see cref="TextEditorService.ViewModelApi"/>
	/// <see cref="TextEditorService.GroupApi"/>
	/// <see cref="TextEditorService.DiffApi"/>
	/// <see cref="TextEditorService.OptionsApi"/>
	/// <see cref="TextEditorService.SearchEngineApi"/>
	/// </summary>
	[Fact]
	public void Constructor()
	{
        TextEditorServicesTestsHelper.InitializeTextEditorServiceTests(out var textEditorService);

		Assert.NotNull(textEditorService);
		Assert.NotNull(textEditorService.ModelStateWrap);
		Assert.NotNull(textEditorService.ViewModelStateWrap);
		Assert.NotNull(textEditorService.GroupStateWrap);
		Assert.NotNull(textEditorService.DiffStateWrap);
		Assert.NotNull(textEditorService.ThemeStateWrap);
		Assert.NotNull(textEditorService.OptionsStateWrap);
		Assert.NotNull(textEditorService.SearchEngineStateWrap);
		Assert.NotNull(textEditorService.ThemeCssClassString);
		Assert.NotNull(textEditorService.ModelApi);
		Assert.NotNull(textEditorService.ViewModelApi);
		Assert.NotNull(textEditorService.GroupApi);
		Assert.NotNull(textEditorService.DiffApi);
		Assert.NotNull(textEditorService.OptionsApi);
		Assert.NotNull(textEditorService.SearchEngineApi);
	}
}