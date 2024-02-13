using Fluxor;
using Luthetus.Common.RazorLib.Themes.States;
using Luthetus.Common.RazorLib.Themes.Models;
using Luthetus.Common.RazorLib.FileSystems.Models;
using Luthetus.TextEditor.RazorLib.SearchEngines.States;
using Luthetus.TextEditor.RazorLib.Installations.Models;
using Luthetus.TextEditor.RazorLib.Options.States;
using Luthetus.TextEditor.RazorLib.TextEditors.Models.TextEditorServices;
using Luthetus.TextEditor.RazorLib.SearchEngines.Models;
using Microsoft.AspNetCore.Components;
using Luthetus.Common.RazorLib.BackgroundTasks.Models;
using Luthetus.Common.RazorLib.Installations.Displays;
using Luthetus.Common.RazorLib.Keys.Models;

namespace Luthetus.TextEditor.RazorLib.Installations.Displays;

public partial class LuthetusTextEditorInitializer : ComponentBase
{
    [Inject]
    private LuthetusTextEditorConfig TextEditorConfig { get; set; } = null!;
    [Inject]
    private IDispatcher Dispatcher { get; set; } = null!;
    [Inject]
    private IThemeService ThemeRecordsCollectionService { get; set; } = null!;
    [Inject]
    private ITextEditorService TextEditorService { get; set; } = null!;
	[Inject]
    private IFileSystemProvider FileSystemProvider { get; set; } = null!;
    [Inject]
    private IBackgroundTaskService BackgroundTaskService { get; set; } = null!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
		if (firstRender)
		{
            BackgroundTaskService.Enqueue(
                Key<BackgroundTask>.NewKey(),
                ContinuousBackgroundTaskWorker.GetQueueKey(),
                nameof(LuthetusCommonInitializer),
                async () =>
                {
                    if (TextEditorConfig.CustomThemeRecordList is not null)
                    {
                        foreach (var themeRecord in TextEditorConfig.CustomThemeRecordList)
                        {
                            Dispatcher.Dispatch(new ThemeState.RegisterAction(themeRecord));
                        }
                    }

                    var initialThemeRecord = ThemeRecordsCollectionService.ThemeStateWrap.Value.ThemeList.FirstOrDefault(
                        x => x.Key == TextEditorConfig.InitialThemeKey);

                    if (initialThemeRecord is not null)
                        Dispatcher.Dispatch(new TextEditorOptionsState.SetThemeAction(initialThemeRecord));

                    foreach (var searchEngine in TextEditorConfig.SearchEngineList)
                    {
                        Dispatcher.Dispatch(new TextEditorSearchEngineState.RegisterAction(searchEngine));
                    }

                    Dispatcher.Dispatch(new TextEditorSearchEngineState.RegisterAction(
                        new SearchEngineFileSystem(FileSystemProvider)));

                    await TextEditorService.OptionsApi.SetFromLocalStorageAsync().ConfigureAwait(false);
                });
		}
	
		await base.OnAfterRenderAsync(firstRender).ConfigureAwait(false);
    }
}