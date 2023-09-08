﻿using Luthetus.Ide.ClassLib.TreeViewImplementations.Helper;
using Luthetus.Ide.ClassLib.ComponentRenderers;
using Luthetus.Ide.ClassLib.ComponentRenderers.Types;
using Luthetus.Common.RazorLib.TreeView.TreeViewClasses;
using Luthetus.Common.RazorLib.WatchWindow.TreeViewClasses;
using Luthetus.Common.RazorLib.FileSystem.Interfaces;
using Luthetus.Common.RazorLib.ComponentRenderers;
using Luthetus.CompilerServices.Lang.CSharp.CompilerServiceCase;
using Luthetus.TextEditor.RazorLib.CompilerServiceCase;
using Luthetus.CompilerServices.Lang.FSharp;
using Luthetus.Common.RazorLib.WatchWindow;

namespace Luthetus.Ide.ClassLib.TreeViewImplementations.CompilerServiceExplorerCase;

public class TreeViewCompilerServicesExplorerRoot : TreeViewWithType<CompilerServicesExplorerRoot>
{
    public TreeViewCompilerServicesExplorerRoot(
        CompilerServicesExplorerRoot compilerServicesExplorerRoot,
        ILuthetusIdeComponentRenderers luthetusIdeComponentRenderers,
        ILuthetusCommonComponentRenderers luthetusCommonComponentRenderers,
        IFileSystemProvider fileSystemProvider,
        IEnvironmentProvider environmentProvider,
        bool isExpandable,
        bool isExpanded)
            : base(
                compilerServicesExplorerRoot,
                isExpandable,
                isExpanded)
    {
        LuthetusIdeComponentRenderers = luthetusIdeComponentRenderers;
        LuthetusCommonComponentRenderers = luthetusCommonComponentRenderers;
        FileSystemProvider = fileSystemProvider;
        EnvironmentProvider = environmentProvider;
    }

    public ILuthetusIdeComponentRenderers LuthetusIdeComponentRenderers { get; }
    public ILuthetusCommonComponentRenderers LuthetusCommonComponentRenderers { get; }
    public IFileSystemProvider FileSystemProvider { get; }
    public IEnvironmentProvider EnvironmentProvider { get; }

    public override bool Equals(object? obj)
    {
        if (obj is not TreeViewCompilerServicesExplorerRoot treeViewCompilerServicesExplorerRoot)
            return false;

        return true;
    }

    public override int GetHashCode()
    {
        return nameof(Item.CSharpCompilerService).GetHashCode();
    }

    public override TreeViewRenderer GetTreeViewRenderer()
    {
        return new TreeViewRenderer(
            LuthetusIdeComponentRenderers.TreeViewCompilerServicesExplorerRootRendererType,
            new Dictionary<string, object?>
            {
                {
                    nameof(ITreeViewCompilerServicesExplorerRootRendererType.TreeViewCompilerServicesExplorerRoot),
                    this
                },
            });
    }

    public override async Task LoadChildrenAsync()
    {
        try
        {
            var newChildren = new List<TreeViewNoType>();

            //var treeViewCSharpBinder = new TreeViewCSharpBinder(
            //    Item.CSharpCompilerService.Binder,
            //    LuthetusIdeComponentRenderers,
            //    LuthetusCommonComponentRenderers,
            //    FileSystemProvider,
            //    EnvironmentProvider,
            //    true,
            //    false);

            //newChildren.Add(treeViewCSharpBinder);

            var watchWindowObjectWrap = new WatchWindowObjectWrap(
                Item.CSharpCompilerService.Binder,
                Item.CSharpCompilerService.Binder.GetType(),
                "Binder",
                true);

            var treeViewReflection = new TreeViewReflection(
                watchWindowObjectWrap,
                true,
                false,
                LuthetusCommonComponentRenderers.WatchWindowTreeViewRenderers!);

            newChildren.Add(treeViewReflection);

            var folder = new Folder("C# Files", () =>
            {
                var folderChildren = new List<TreeViewNoType>();

                var cSharpResources = Item.CSharpCompilerService.CompilerServiceResources
                    .Select(x => (CSharpResource)x)
                    .OrderBy(x => x.ResourceUri.Value);

                foreach (var cSharpResource in cSharpResources)
                {
                    var treeViewCSharpResource = new TreeViewCSharpResource(
                        cSharpResource,
                        LuthetusIdeComponentRenderers,
                        LuthetusCommonComponentRenderers,
                        FileSystemProvider,
                        EnvironmentProvider,
                        true,
                        false);

                    folderChildren.Add(treeViewCSharpResource);
                }

                return Task.FromResult(folderChildren);
            });

            var treeViewFolder = new TreeViewFolder(
                folder,
                LuthetusIdeComponentRenderers,
                LuthetusCommonComponentRenderers,
                FileSystemProvider,
                EnvironmentProvider,
                true,
                false);

            newChildren.Add(treeViewFolder);

            var oldChildrenMap = Children
                .ToDictionary(child => child);

            foreach (var newChild in newChildren)
            {
                if (oldChildrenMap.TryGetValue(newChild, out var oldChild))
                {
                    newChild.IsExpanded = oldChild.IsExpanded;
                    newChild.IsExpandable = oldChild.IsExpandable;
                    newChild.IsHidden = oldChild.IsHidden;
                    newChild.TreeViewNodeKey = oldChild.TreeViewNodeKey;
                    newChild.Children = oldChild.Children;
                }
            }

            for (int i = 0; i < newChildren.Count; i++)
            {
                var newChild = newChildren[i];

                newChild.IndexAmongSiblings = i;
                newChild.Parent = this;
                newChild.TreeViewChangedKey = TreeViewChangedKey.NewTreeViewChangedKey();
            }

            Children = newChildren;
        }
        catch (Exception exception)
        {
            Children = new List<TreeViewNoType>
        {
            new TreeViewException(
                exception,
                false,
                false,
                LuthetusCommonComponentRenderers.WatchWindowTreeViewRenderers)
            {
                Parent = this,
                IndexAmongSiblings = 0,
            }
        };
        }

        TreeViewChangedKey = TreeViewChangedKey.NewTreeViewChangedKey();
    }

    public override void RemoveRelatedFilesFromParent(List<TreeViewNoType> siblingsAndSelfTreeViews)
    {
        // This method is meant to do nothing in this case.
    }
}