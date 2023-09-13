﻿using Luthetus.Ide.RazorLib.ContextCase;

namespace Luthetus.Ide.RazorLib.CommandCase;

public interface ICommandFactory
{
    public void Initialize();
    public ICommand ConstructFocusContextElementCommand(ContextRecord contextRecord);
}
