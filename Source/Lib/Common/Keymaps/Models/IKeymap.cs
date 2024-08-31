using Luthetus.Common.RazorLib.Commands.Models;
using Luthetus.Common.RazorLib.Keys.Models;

namespace Luthetus.Common.RazorLib.Keymaps.Models;

public interface IKeymap
{
	/// <summary>Unique identifier for the keymap</summary>
	public Key<Keymap> Key { get; }
	/// <summary>User facing name for the keymap</summary>
    public string DisplayName { get; }

	public bool TryRegister(IKeymapArgs args, CommandNoType command);
    public bool TryMap(IKeymapArgs args, out CommandNoType command);
	public List<KeyValuePair<IKeymapArgs, CommandNoType>> GetKeyValuePairList();

    public static readonly IKeymap Empty = new KeymapDoNothing();
}
