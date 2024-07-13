using Luthetus.Common.RazorLib.ComponentRenderers.Models;
using Luthetus.Common.RazorLib.WatchWindows.Models;
using Luthetus.Common.RazorLib.TreeViews.Models;
using Luthetus.Common.RazorLib.Keys.Models;
using Luthetus.Common.RazorLib.TreeViews.Models.Utils;
using Luthetus.Ide.RazorLib.TestExplorers.Displays.Internals;

namespace Luthetus.Ide.RazorLib.TestExplorers.Models;

public class TreeViewGroup : TreeViewWithType<string>
{
    public TreeViewGroup(
            string displayText,
            bool isExpandable,
            bool isExpanded)
        : base(displayText, isExpandable, isExpanded)
    {
    }

    public override bool Equals(object? obj)
    {
        if (obj is not TreeViewGroup treeViewGroup)
            return false;

        return treeViewGroup.Item == Item;
    }

    public override int GetHashCode() => Item.GetHashCode();

    public override TreeViewRenderer GetTreeViewRenderer()
    {
        return new TreeViewRenderer(
            typeof(TreeViewGroupDisplay),
            new Dictionary<string, object?>
            {
                {
                    nameof(TreeViewGroupDisplay.TreeViewGroup),
                    this
                },
            });
    }

    public override Task LoadChildListAsync()
    {
    	return Task.CompletedTask;
    }

    public override void RemoveRelatedFilesFromParent(List<TreeViewNoType> siblingsAndSelfTreeViews)
    {
        return;
    }
}
