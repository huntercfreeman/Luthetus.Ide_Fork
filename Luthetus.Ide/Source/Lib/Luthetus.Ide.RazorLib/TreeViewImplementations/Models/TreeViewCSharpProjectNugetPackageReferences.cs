﻿using Luthetus.CompilerServices.Lang.Xml.Html.SyntaxActors;
using Luthetus.Common.RazorLib.FileSystems.Models;
using Luthetus.Common.RazorLib.TreeViews.Models;
using Luthetus.Common.RazorLib.Keys.Models;
using Luthetus.Ide.RazorLib.Nugets.Models;
using Luthetus.Ide.RazorLib.ComponentRenderers.Models;
using Luthetus.CompilerServices.Lang.DotNetSolution.Models.Project;

namespace Luthetus.Ide.RazorLib.TreeViewImplementations.Models;

public class TreeViewCSharpProjectNugetPackageReferences : TreeViewWithType<CSharpProjectNugetPackageReferences>
{
    public TreeViewCSharpProjectNugetPackageReferences(
            CSharpProjectNugetPackageReferences cSharpProjectNugetPackageReferences,
            ILuthetusIdeComponentRenderers ideComponentRenderers,
            IFileSystemProvider fileSystemProvider,
            IEnvironmentProvider environmentProvider,
            bool isExpandable,
            bool isExpanded)
        : base(cSharpProjectNugetPackageReferences, isExpandable, isExpanded)
    {
        IdeComponentRenderers = ideComponentRenderers;
        FileSystemProvider = fileSystemProvider;
        EnvironmentProvider = environmentProvider;
    }

    public ILuthetusIdeComponentRenderers IdeComponentRenderers { get; }
    public IFileSystemProvider FileSystemProvider { get; }
    public IEnvironmentProvider EnvironmentProvider { get; }

    public override bool Equals(object? obj)
    {
        if (obj is not TreeViewCSharpProjectNugetPackageReferences otherTreeView)
            return false;

        return otherTreeView.GetHashCode() == GetHashCode();
    }

    public override int GetHashCode() => Item.CSharpProjectNamespacePath.AbsolutePath.Value.GetHashCode();

    public override TreeViewRenderer GetTreeViewRenderer()
    {
        return new TreeViewRenderer(
            IdeComponentRenderers.LuthetusIdeTreeViews.TreeViewCSharpProjectNugetPackageReferencesRendererType,
            null);
    }

    public override async Task LoadChildBagAsync()
    {
        var content = await FileSystemProvider.File.ReadAllTextAsync(
            Item.CSharpProjectNamespacePath.AbsolutePath.Value);

        var htmlSyntaxUnit = HtmlSyntaxTree.ParseText(
            new(Item.CSharpProjectNamespacePath.AbsolutePath.Value),
            content);

        var syntaxNodeRoot = htmlSyntaxUnit.RootTagSyntax;

        var cSharpProjectSyntaxWalker = new CSharpProjectSyntaxWalker();

        cSharpProjectSyntaxWalker.Visit(syntaxNodeRoot);

        var packageReferences = cSharpProjectSyntaxWalker.TagNodes
            .Where(ts => (ts.OpenTagNameNode?.TextEditorTextSpan.GetText() ?? string.Empty) == "PackageReference")
            .ToList();

        List<LightWeightNugetPackageRecord> lightWeightNugetPackageRecords = new();

        foreach (var packageReference in packageReferences)
        {
            var attributeNameValueTuples = packageReference
                .AttributeNodes
                .Select(x => (
                    x.AttributeNameSyntax.TextEditorTextSpan
                        .GetText()
                        .Trim(),
                    x.AttributeValueSyntax.TextEditorTextSpan
                        .GetText()
                        .Replace("\"", string.Empty)
                        .Replace("=", string.Empty)
                        .Trim()))
                .ToArray();

            var includeAttribute = attributeNameValueTuples.FirstOrDefault(x => x.Item1 == "Include");
            var versionAttribute = attributeNameValueTuples.FirstOrDefault(x => x.Item1 == "Version");

            var lightWeightNugetPackageRecord = new LightWeightNugetPackageRecord(
                includeAttribute.Item2,
                includeAttribute.Item2,
                versionAttribute.Item2);

            lightWeightNugetPackageRecords.Add(lightWeightNugetPackageRecord);
        }

        var cSharpProjectAbsolutePathString = Item.CSharpProjectNamespacePath.AbsolutePath.Value;

        var newChildBag = lightWeightNugetPackageRecords.Select(
            npr => (TreeViewNoType)new TreeViewCSharpProjectNugetPackageReference(
                new(cSharpProjectAbsolutePathString, npr),
                IdeComponentRenderers,
                FileSystemProvider,
                EnvironmentProvider,
                false,
                false)
            {
                TreeViewChangedKey = Key<TreeViewChanged>.NewKey()
            })
            .ToList();

        for (int i = 0; i < newChildBag.Count; i++)
        {
            var newChild = newChildBag[i];

            newChild.IndexAmongSiblings = i;
            newChild.Parent = this;
            newChild.TreeViewChangedKey = Key<TreeViewChanged>.NewKey();
        }

        ChildBag = newChildBag;
        TreeViewChangedKey = Key<TreeViewChanged>.NewKey();
    }

    public override void RemoveRelatedFilesFromParent(List<TreeViewNoType> siblingsAndSelfTreeViews)
    {
        return;
    }
}