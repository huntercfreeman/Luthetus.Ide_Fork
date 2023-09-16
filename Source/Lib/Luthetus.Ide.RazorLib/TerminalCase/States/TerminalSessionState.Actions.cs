﻿using Luthetus.Common.RazorLib.KeyCase;
using Luthetus.Ide.RazorLib.TerminalCase.Models;

namespace Luthetus.Ide.RazorLib.TerminalCase.States;

public partial record TerminalSessionState
{
    public record RegisterTerminalSessionAction(TerminalSession TerminalSession);
    public record UpdateTerminalSessionStateKeyAction(TerminalSession TerminalSession);
    public record DisposeTerminalSessionAction(Key<TerminalSession> TerminalSessionKey);
}