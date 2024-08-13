using System.Collections.Immutable;
using CliWrap.EventStream;
using Luthetus.Common.RazorLib.Keys.Models;
using Luthetus.TextEditor.RazorLib.Lexers.Models;
using Luthetus.TextEditor.RazorLib.CompilerServices.Syntax.Symbols;
using Luthetus.TextEditor.RazorLib.CompilerServices.Interfaces;

namespace Luthetus.Ide.RazorLib.Terminals.Models;

/// <summary>Output Data</summary>
public interface ITerminalOutput : IDisposable
{
	public ImmutableList<ITerminalOutputFormatter> OutputFormatterList { get; }

	/// <summary>
	/// TODO: Make this 'Action<Key<TerminalCommandParsed>>?' so one can
	///       track the output of a specific command as it is being executed?
	/// </summary>
	public event Action? OnWriteOutput;
	
	public void WriteOutput(TerminalCommandParsed terminalCommandParsed, CommandEvent commandEvent);
	public void ClearOutput();
	public ITerminalOutputFormatted GetOutputFormatted(string terminalOutputFormatterName);
	public TerminalCommandParsed? GetParsedCommandOrDefault(Key<TerminalCommandRequest> terminalCommandRequestKey);
	public ImmutableList<TerminalCommandParsed> GetParsedCommandList();
}