﻿namespace Luthetus.Ide.ClassLib.ComponentRenderers;

public class LuthetusIdeComponentRenderers : ILuthetusIdeComponentRenderers
{
    public LuthetusIdeComponentRenderers(
        Type? booleanPromptOrCancelRendererType,
        Type? fileFormRendererType,
        Type? deleteFileFormRendererType,
        Type? treeViewNamespacePathRendererType,
        Type? treeViewAbsoluteFilePathRendererType,
        Type? treeViewGitFileRendererType,
        Type? nuGetPackageManagerRendererType,
        Type? gitDisplayRendererType,
        Type? removeCSharpProjectFromSolutionRendererType,
        Type? inputFileRendererType,
        Type? treeViewCSharpProjectDependenciesRendererType,
        Type? treeViewCSharpProjectNugetPackageReferencesRendererType,
        Type? treeViewCSharpProjectToProjectReferencesRendererType,
        Type? treeViewLightWeightNugetPackageRecordRendererType,
        Type? treeViewCSharpProjectToProjectReferenceRendererType,
        Type? treeViewSolutionFolderRendererType,
        Type? treeViewCompilerServicesExplorerRootRendererType,
        Type? treeViewCSharpResourceRendererType,
        Type? treeViewCSharpBinderRendererType,
        Type? treeViewFolderRendererType)
    {
        BooleanPromptOrCancelRendererType = booleanPromptOrCancelRendererType;
        FileFormRendererType = fileFormRendererType;
        DeleteFileFormRendererType = deleteFileFormRendererType;
        TreeViewNamespacePathRendererType = treeViewNamespacePathRendererType;
        TreeViewAbsoluteFilePathRendererType = treeViewAbsoluteFilePathRendererType;
        TreeViewGitFileRendererType = treeViewGitFileRendererType;
        NuGetPackageManagerRendererType = nuGetPackageManagerRendererType;
        GitDisplayRendererType = gitDisplayRendererType;
        RemoveCSharpProjectFromSolutionRendererType = removeCSharpProjectFromSolutionRendererType;
        InputFileRendererType = inputFileRendererType;
        TreeViewCompilerServicesExplorerRootRendererType = treeViewCompilerServicesExplorerRootRendererType;
        TreeViewCSharpResourceRendererType = treeViewCSharpResourceRendererType;
        TreeViewCSharpBinderRendererType = treeViewCSharpBinderRendererType;
        TreeViewFolderRendererType = treeViewFolderRendererType;
        TreeViewCSharpProjectDependenciesRendererType = treeViewCSharpProjectDependenciesRendererType;
        TreeViewCSharpProjectNugetPackageReferencesRendererType = treeViewCSharpProjectNugetPackageReferencesRendererType;
        TreeViewCSharpProjectToProjectReferencesRendererType = treeViewCSharpProjectToProjectReferencesRendererType;
        TreeViewLightWeightNugetPackageRecordRendererType = treeViewLightWeightNugetPackageRecordRendererType;
        TreeViewCSharpProjectToProjectReferenceRendererType = treeViewCSharpProjectToProjectReferenceRendererType;
        TreeViewSolutionFolderRendererType = treeViewSolutionFolderRendererType;
    }

    public Type? BooleanPromptOrCancelRendererType { get; }
    public Type? FileFormRendererType { get; }
    public Type? DeleteFileFormRendererType { get; }
    public Type? TreeViewNamespacePathRendererType { get; }
    public Type? TreeViewSolutionFolderRendererType { get; }
    public Type? TreeViewCSharpProjectDependenciesRendererType { get; }
    public Type? TreeViewCSharpProjectNugetPackageReferencesRendererType { get; }
    public Type? TreeViewCSharpProjectToProjectReferencesRendererType { get; }
    public Type? TreeViewLightWeightNugetPackageRecordRendererType { get; }
    public Type? TreeViewCSharpProjectToProjectReferenceRendererType { get; }
    public Type? TreeViewAbsoluteFilePathRendererType { get; }
    public Type? TreeViewGitFileRendererType { get; }
    public Type? NuGetPackageManagerRendererType { get; }
    public Type? GitDisplayRendererType { get; }
    public Type? RemoveCSharpProjectFromSolutionRendererType { get; }
    public Type? InputFileRendererType { get; }
    public Type? TreeViewCompilerServicesExplorerRootRendererType { get; }
    public Type? TreeViewCSharpResourceRendererType { get; }
    public Type? TreeViewCSharpBinderRendererType { get; }
    public Type? TreeViewFolderRendererType { get; }
}