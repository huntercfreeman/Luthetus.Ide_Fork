namespace Luthetus.Ide.Wasm.Facts;

public partial class InitialSolutionFacts
{
	public const string BLAZOR_CRUD_APP_ALL_C_SHARP_SYNTAX_ABSOLUTE_FILE_PATH = @"/BlazorCrudApp/BlazorCrudApp.Wasm/AllCSharpSyntax.cs";

    public const string BLAZOR_CRUD_APP_ALL_C_SHARP_SYNTAX_CONTENTS = @"namespace Luthetus.CompilerServices.CSharp;

/// <summary> Aim to type out every possible syntax and combination of syntax in this file and do so as succinctly as possible (it doesn't have to compile). https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/ ;v1.0.0 </summary>
public class AllCSharpSyntax 
{
	public void AccessModifiers() {
		public A;
		protected internal B;
		protected C;
		internal D;
		private protected E;
		private F;
	}

	public void StorageModifiers() {
		struct A { }
		class B { }
		interface C { }
		enum D { }
		record E { }
		record struct F { }
	}
	
	public void AccessModifiers_StorageModifiers() {
		public struct AA { }
		public class AB { }
		public interface AC { }
		public enum AD { }
		public record AE { }
		public record struct AF { }
		
		protected internal struct BA { }
		protected internal class BB { }
		protected internal interface BC { }
		protected internal enum BD { }
		protected internal record BE { }
		protected internal record struct BF { }
		
		protected struct CA { }
		protected class CB { }
		protected interface CC { }
		protected enum CD { }
		protected record CE { }
		protected record struct CF { }
		
		internal struct DA { }
		internal class DB { }
		internal interface DC { }
		internal enum DD { }
		internal record DE { }
		internal record struct DF { }
		
		private protected struct EA { }
		private protected class EB { }
		private protected interface EC { }
		private protected enum ED { }
		private protected record EE { }
		private protected record struct EF { }
		
		private struct FA { }
		private class FB { }
		private interface FC { }
		private enum FD { }
		private record FE { }
		private record struct FF { }
    }
    
    public void NonContextualKeywords()
	{
		abstract; as;
		base; bool; break; byte;
		case; catch; char; checked; class; const; continue;
		decimal; default; delegate; do; double;
		else; enum; event; explicit; extern;
		false; finally; fixed; float; for; foreach;
		goto;
		if; implicit; in; int; interface; internal; is;
		lock; long;
		namespace; new; null;
		object; operator; out; override;
		params; private; protected; public;
		readonly; ref; return;
		sbyte; sealed; short; sizeof; stackalloc; static; string; struct; switch;
		this; throw; true; try; typeof;
		uint; ulong; unchecked; unsafe; ushort; using;
		virtual; void; volatile;
		while;
	}
	
    public void ContextualKeywords()
    {
    	add; and; alias; ascending; args; async; await;
    	by;
    	descending; dynamic;
    	equals;
    	file; from;
    	get; global; group;
    	init; into;
    	join;
    	let;
    	managed;
    	nameof; nint; not; notnull; nuint;
    	on; or; orderby;
    	partial;
    	record; remove; required;
    	scoped; select; set;
    	unmanaged;
    	value; var;
    	when; where; with;
    	yield;
    }
    
	public void Operators()
	{
		/* Primary */
		x.y; f(x); a[i]; x?.y; x?[y]; x++; x--; x!; new; typeof; checked; unchecked; default; nameof; delegate; sizeof; stackalloc; x->y;
		/* Unary */
		+x; -x; !x; ~x; ++x; --x; ^x; (T)x; await; &x; *x; true and false;
		/* Range */
		x..y;
		/* switch and with expressions */
		switch; with;
		/* Multiplicative */
		x * y; x / y; x % y;
		/* Additive */
		x + y; x – y;
		/* Shift */
		x << y; x >> y; x >>> y;
		/* Relational and type-testing */
		x < y; x > y; x <= y; x >= y; is; as;
		/* Equality */
		x == y; x != y;
		/* Boolean logical AND or bitwise logical AND */
		x & y;
		/* Boolean logical XOR or bitwise logical XOR */
		x ^ y;
		/* Boolean logical OR or bitwise logical OR */
		x | y;
		/* Conditional AND */
		x && y;
		/* Conditional OR */
		x || y;
		/* Null-coalescing operator */
		x ?? y;
		/* Conditional operator */
		c ? t : f
		/* Assignment and lambda declaration */
		x = y; x += y; x -= y; x *= y; x /= y; x %= y; x &= y; x |= y; x ^= y; x <<= y; x >>= y; x >>>= y; x ??= y; =>;
	}
    
    // VariableKind
    // ============
	// Local
	// Field
	// Property
}
";
}
