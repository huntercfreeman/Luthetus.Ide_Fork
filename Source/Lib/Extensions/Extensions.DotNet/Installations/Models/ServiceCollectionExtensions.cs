using Microsoft.Extensions.DependencyInjection;
using Luthetus.Common.RazorLib.Installations.Models;
using Luthetus.Ide.RazorLib.Installations.Models;
using Luthetus.Extensions.DotNet.CSharpProjects.Displays;
using Luthetus.Extensions.DotNet.DotNetSolutions.Displays;
using Luthetus.Extensions.DotNet.Menus.Models;
using Luthetus.Extensions.DotNet.Nugets.Models;
using Luthetus.Extensions.DotNet.ComponentRenderers.Models;
using Luthetus.Extensions.DotNet.Nugets.Displays;
using Luthetus.Extensions.DotNet.CommandLines.Models;
using Luthetus.Extensions.DotNet.BackgroundTasks.Models;
using Luthetus.Extensions.DotNet.CompilerServices.Displays;

namespace Luthetus.Extensions.DotNet.Installations.Models;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddLuthetusExtensionsDotNetServices(
		this IServiceCollection services,
		LuthetusHostingInformation hostingInformation,
		Func<LuthetusIdeConfig, LuthetusIdeConfig>? configure = null)
	{
		return services
			.AddScoped<INugetPackageManagerProvider, NugetPackageManagerProviderAzureSearchUsnc>()
			.AddScoped<DotNetCliOutputParser>()
			.AddScoped<DotNetBackgroundTaskApi>()
			.AddScoped<ICompilerServicesMenuOptionsFactory, CompilerServicesMenuOptionsFactory>()
			.AddScoped<ICompilerServicesComponentRenderers>(_ => _compilerServicesComponentRenderers);
	}

	private static readonly CompilerServicesTreeViews _compilerServicesTreeViews = new(
		typeof(TreeViewCSharpProjectDependenciesDisplay),
		typeof(TreeViewCSharpProjectNugetPackageReferencesDisplay),
		typeof(TreeViewCSharpProjectToProjectReferencesDisplay),
		typeof(TreeViewCSharpProjectNugetPackageReferenceDisplay),
		typeof(TreeViewCSharpProjectToProjectReferenceDisplay),
		typeof(TreeViewSolutionFolderDisplay),
        typeof(TreeViewCompilerServiceDisplay));

	private static readonly CompilerServicesComponentRenderers _compilerServicesComponentRenderers = new(
		typeof(NuGetPackageManager),
		typeof(RemoveCSharpProjectFromSolutionDisplay),
		_compilerServicesTreeViews);
}