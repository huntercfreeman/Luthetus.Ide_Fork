using Microsoft.AspNetCore.Components;
using Fluxor;
using Luthetus.Common.RazorLib.Options.States;

namespace Luthetus.Extensions.DotNet.CSharpProjects.Displays;

public partial class TreeViewCSharpProjectToProjectReferencesDisplay : ComponentBase
{
    [Inject]
    private IState<AppOptionsState> AppOptionsStateWrap { get; set; } = null!;
}