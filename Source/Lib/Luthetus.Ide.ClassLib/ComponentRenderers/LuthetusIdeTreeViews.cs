﻿namespace Luthetus.Ide.ClassLib.ComponentRenderers;

public class LuthetusIdeTreeViews
{
    public LuthetusIdeTreeViews(
        Type? treeViewNamespacePathRendererType,
        Type? treeViewAbsoluteFilePathRendererType,
        Type? treeViewGitFileRendererType,
        Type? treeViewCompilerServiceRendererType,
        Type? treeViewCSharpProjectDependenciesRendererType,
        Type? treeViewCSharpProjectNugetPackageReferencesRendererType,
        Type? treeViewCSharpProjectToProjectReferencesRendererType,
        Type? treeViewLightWeightNugetPackageRecordRendererType,
        Type? treeViewCSharpProjectToProjectReferenceRendererType,
        Type? treeViewSolutionFolderRendererType)
    {
        TreeViewNamespacePathRendererType = treeViewNamespacePathRendererType;
        TreeViewAbsoluteFilePathRendererType = treeViewAbsoluteFilePathRendererType;
        TreeViewGitFileRendererType = treeViewGitFileRendererType;
        TreeViewCompilerServiceRendererType = treeViewCompilerServiceRendererType;
        TreeViewCSharpProjectDependenciesRendererType = treeViewCSharpProjectDependenciesRendererType;
        TreeViewCSharpProjectNugetPackageReferencesRendererType = treeViewCSharpProjectNugetPackageReferencesRendererType;
        TreeViewCSharpProjectToProjectReferencesRendererType = treeViewCSharpProjectToProjectReferencesRendererType;
        TreeViewLightWeightNugetPackageRecordRendererType = treeViewLightWeightNugetPackageRecordRendererType;
        TreeViewCSharpProjectToProjectReferenceRendererType = treeViewCSharpProjectToProjectReferenceRendererType;
        TreeViewSolutionFolderRendererType = treeViewSolutionFolderRendererType;
    }

    public Type? TreeViewNamespacePathRendererType { get; }
    public Type? TreeViewSolutionFolderRendererType { get; }
    public Type? TreeViewCSharpProjectDependenciesRendererType { get; }
    public Type? TreeViewCSharpProjectNugetPackageReferencesRendererType { get; }
    public Type? TreeViewCSharpProjectToProjectReferencesRendererType { get; }
    public Type? TreeViewLightWeightNugetPackageRecordRendererType { get; }
    public Type? TreeViewCSharpProjectToProjectReferenceRendererType { get; }
    public Type? TreeViewAbsoluteFilePathRendererType { get; }
    public Type? TreeViewGitFileRendererType { get; }
    public Type? TreeViewCompilerServiceRendererType { get; }
}