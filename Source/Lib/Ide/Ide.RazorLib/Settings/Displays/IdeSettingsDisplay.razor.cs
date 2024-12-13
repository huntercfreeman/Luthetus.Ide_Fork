using Microsoft.AspNetCore.Components;
using Luthetus.Common.RazorLib.FileSystems.Models;
using Luthetus.TextEditor.RazorLib.CompilerServices;
using Luthetus.Ide.RazorLib.AppDatas.Models;

namespace Luthetus.Ide.RazorLib.Settings.Displays;

public partial class IdeSettingsDisplay : ComponentBase
{
	[Inject]
	private IEnvironmentProvider EnvironmentProvider { get; set; } = null!;
	[Inject]
	private IAppDataService AppDataService { get; set; } = null!;
	
	private void WriteLuthetusDebugSomethingToConsole()
	{
		#if DEBUG
		Console.WriteLine(LuthetusDebugSomething.CreateText());
		#else
		Console.WriteLine($"Must run in debug mode to see {nameof(WriteLuthetusDebugSomethingToConsole)}");
		#endif
	}
}