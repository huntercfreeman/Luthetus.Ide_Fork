using Fluxor;
using Luthetus.Common.RazorLib.KeyCase.Models;
using Luthetus.Ide.RazorLib.TerminalCase.Models;

namespace Luthetus.Ide.RazorLib.TerminalCase.States;

[FeatureState]
public partial record WellKnownTerminalSessionsState(Key<TerminalSession> ActiveTerminalSessionKey)
{
    public WellKnownTerminalSessionsState() : this(TerminalSessionFacts.GENERAL_TERMINAL_SESSION_KEY)
    {
    }
}
