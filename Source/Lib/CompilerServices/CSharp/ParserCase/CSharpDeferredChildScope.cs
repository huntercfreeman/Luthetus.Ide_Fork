using Luthetus.TextEditor.RazorLib.CompilerServices.Syntax;
using Luthetus.TextEditor.RazorLib.CompilerServices.Interfaces;
using Luthetus.TextEditor.RazorLib.CompilerServices.Syntax.Nodes.Interfaces;
using Luthetus.CompilerServices.CSharp.CompilerServiceCase;

namespace Luthetus.CompilerServices.CSharp.ParserCase;

public class CSharpDeferredChildScope
{
	public CSharpDeferredChildScope(
		int openTokenIndex,
		int closeTokenIndex,
		ICodeBlockOwner pendingCodeBlockOwner)
	{
		OpenTokenIndex = openTokenIndex;
		CloseTokenIndex = closeTokenIndex;
		PendingCodeBlockOwner = pendingCodeBlockOwner;
	}
	
	public int OpenTokenIndex { get; }
	public int CloseTokenIndex { get; }
	public ICodeBlockOwner PendingCodeBlockOwner { get; }
	
	public int TokenIndexToRestore { get; private set; }
	
	public void PrepareMainParserLoop(int tokenIndexToRestore, CSharpCompilationUnit compilationUnit)
	{
		TokenIndexToRestore = tokenIndexToRestore;
		compilationUnit.ParserModel.CurrentCodeBlockBuilder.PermitInnerPendingCodeBlockOwnerToBeParsed = true;
		
		compilationUnit.ParserModel.CurrentCodeBlockBuilder.DequeuedIndexForChildList = null;
		
		compilationUnit.ParserModel.TokenWalker.DeferredParsing(
			OpenTokenIndex,
			CloseTokenIndex,
			TokenIndexToRestore);
		
		compilationUnit.ParserModel.SyntaxStack.Push(PendingCodeBlockOwner);
		compilationUnit.ParserModel.CurrentCodeBlockBuilder.InnerPendingCodeBlockOwner = PendingCodeBlockOwner;
	}
}
