﻿using Fluxor;
using System.Collections.Immutable;

namespace Luthetus.Ide.ClassLib.Store.TerminalCase;

[FeatureState]
public record TerminalSessionsState(ImmutableDictionary<TerminalSessionKey, TerminalSession> TerminalSessionMap)
{
    public TerminalSessionsState()
        : this(ImmutableDictionary<TerminalSessionKey, TerminalSession>.Empty)
    {
    }
}