﻿using System.Collections;
using Luthetus.Common.RazorLib.ComponentRenderers.Models;
using Luthetus.Common.RazorLib.TreeViews.Models;

namespace Luthetus.Common.RazorLib.WatchWindows.Models;

public class TreeViewReflection : TreeViewWithType<WatchWindowObject>
{
    private readonly ILuthetusCommonComponentRenderers _luthetusCommonComponentRenderers;

    public TreeViewReflection(
            WatchWindowObject watchWindowObject,
            bool isExpandable,
            bool isExpanded,
            ILuthetusCommonComponentRenderers luthetusCommonComponentRenderers)
        : base(watchWindowObject, isExpandable, isExpanded)
    {
        _luthetusCommonComponentRenderers = luthetusCommonComponentRenderers;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not TreeViewReflection treeViewReflection)
            return false;

        return treeViewReflection.Item == Item;
    }

    public override int GetHashCode()
    {
        return Item.GetHashCode();
    }

    public override TreeViewRenderer GetTreeViewRenderer()
    {
        return new TreeViewRenderer(
            _luthetusCommonComponentRenderers.LuthetusCommonTreeViews.TreeViewReflectionRenderer,
            new Dictionary<string, object?>
            {
                {
                    nameof(TreeViewReflection),
                    this
                },
            });
    }

    public override Task LoadChildBagAsync()
    {
        var previousChildren = new List<TreeViewNoType>(ChildBag);

        try
        {
            ChildBag.Clear();

            ChildBag.Add(new TreeViewFields(
                Item,
                true,
                false,
                _luthetusCommonComponentRenderers));

            ChildBag.Add(new TreeViewProperties(
                Item,
                true,
                false,
                _luthetusCommonComponentRenderers));

            if (Item.Item is IEnumerable)
            {
                ChildBag.Add(new TreeViewEnumerable(
                    Item,
                    true,
                    false,
                    _luthetusCommonComponentRenderers));
            }

            if (Item.ItemType.IsInterface && Item.Item is not null)
            {
                var interfaceImplementation = new WatchWindowObject(
                    Item.Item,
                    Item.Item.GetType(),
                    "InterfaceImplementation",
                    false);

                ChildBag.Add(new TreeViewInterfaceImplementation(
                    interfaceImplementation,
                    true,
                    false,
                    _luthetusCommonComponentRenderers));
            }
        }
        catch (Exception e)
        {
            ChildBag.Clear();

            ChildBag.Add(new TreeViewException(
                e,
                false,
                false,
                _luthetusCommonComponentRenderers));
        }

        LinkChildren(previousChildren, ChildBag);

        return Task.CompletedTask;
    }
}