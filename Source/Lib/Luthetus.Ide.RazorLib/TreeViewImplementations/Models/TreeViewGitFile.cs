﻿using Luthetus.Common.RazorLib.TreeViews.Models;
using Luthetus.Ide.RazorLib.ComponentRenderers.Models;
using Luthetus.Ide.RazorLib.Gits.Models;

namespace Luthetus.Ide.RazorLib.TreeViewImplementations.Models;

public class TreeViewGitFile : TreeViewWithType<GitFile>
{
    public TreeViewGitFile(
        GitFile gitFile,
        ILuthetusIdeComponentRenderers luthetusIdeComponentRenderers,
        bool isExpandable,
        bool isExpanded)
            : base(
                gitFile,
                isExpandable,
                isExpanded)
    {
        LuthetusIdeComponentRenderers = luthetusIdeComponentRenderers;
    }

    public ILuthetusIdeComponentRenderers LuthetusIdeComponentRenderers { get; }

    public override bool Equals(object? obj)
    {
        if (obj is not TreeViewGitFile treeViewGitFile)
            return false;

        return treeViewGitFile.Item.AbsolutePath.FormattedInput ==
               Item.AbsolutePath.FormattedInput;
    }

    public override int GetHashCode()
    {
        return Item.AbsolutePath.FormattedInput.GetHashCode();
    }

    public override TreeViewRenderer GetTreeViewRenderer()
    {
        return new TreeViewRenderer(
            LuthetusIdeComponentRenderers.LuthetusIdeTreeViews.TreeViewGitFileRendererType,
            new Dictionary<string, object?>
            {
            {
                nameof(ITreeViewGitFileRendererType.TreeViewGitFile),
                this
            },
            });
    }

    public override Task LoadChildBagAsync()
    {
        return Task.CompletedTask;
    }

    public override void RemoveRelatedFilesFromParent(List<TreeViewNoType> siblingsAndSelfTreeViews)
    {
        // This method is meant to do nothing in this case.
    }
}