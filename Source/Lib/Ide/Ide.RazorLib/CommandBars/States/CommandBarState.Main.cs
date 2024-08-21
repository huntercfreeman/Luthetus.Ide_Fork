using Fluxor;

namespace Luthetus.Ide.RazorLib.CommandBars.States;

[FeatureState]
public partial record CommandBarState(bool ShouldDisplay)
{
	public CommandBarState() : this(false)
	{
	}
}
