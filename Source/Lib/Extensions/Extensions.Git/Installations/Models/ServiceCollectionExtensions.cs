using Microsoft.Extensions.DependencyInjection;
using Luthetus.Common.RazorLib.Installations.Models;
using Luthetus.Ide.RazorLib.Installations.Models;
using Luthetus.Extensions.Git.ComponentRenderers.Models;
using Luthetus.Extensions.Git.Displays;
using Luthetus.Extensions.Git.Models;

namespace Luthetus.Extensions.Git.Installations.Models;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddLuthetusExtensionsGitServices(
		this IServiceCollection services,
		LuthetusHostingInformation hostingInformation,
		Func<LuthetusIdeConfig, LuthetusIdeConfig>? configure = null)
	{
		return services
			.AddScoped<GitCliOutputParser>();
	}

	private static readonly GitTreeViews _gitTreeViews = new(
        typeof(TreeViewGitFileDisplay));
}
