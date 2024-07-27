namespace Luthetus.Ide.RazorLib.Terminals.Models.NewCode;

public class TerminalInputTextEditor : ITerminalInput
{
	private readonly ITerminal _terminal;

	public TerminalInputTextEditor(ITerminal terminal)
	{
		_terminal = terminal;
	}

	public void OnAfterWorkingDirectoryChanged(string workingDirectoryAbsolutePathString)
	{
	}
	
	public void OnHandleCommandStarting()
	{
	}
}
