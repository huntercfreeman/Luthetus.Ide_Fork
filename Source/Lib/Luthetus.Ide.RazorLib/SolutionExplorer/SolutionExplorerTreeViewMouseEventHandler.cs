﻿using BlazorCommon.RazorLib.BackgroundTaskCase;
using BlazorCommon.RazorLib.TreeView;
using BlazorCommon.RazorLib.TreeView.Commands;
using BlazorCommon.RazorLib.TreeView.Events;
using BlazorTextEditor.RazorLib;
using Fluxor;
using Luthetus.Ide.ClassLib.ComponentRenderers;
using Luthetus.Ide.ClassLib.FileSystem.Interfaces;
using Luthetus.Ide.ClassLib.Store.EditorCase;
using Luthetus.Ide.ClassLib.TreeViewImplementations;

namespace Luthetus.Ide.RazorLib.SolutionExplorer;

public class SolutionExplorerTreeViewMouseEventHandler : TreeViewMouseEventHandler
{
    private readonly IDispatcher _dispatcher;
    private readonly ITextEditorService _textEditorService;
    private readonly ILuthetusIdeComponentRenderers _blazorStudioComponentRenderers;
    private readonly IFileSystemProvider _fileSystemProvider;
    private readonly IBackgroundTaskQueue _backgroundTaskQueue;

    public SolutionExplorerTreeViewMouseEventHandler(
        IDispatcher dispatcher,
        ITextEditorService textEditorService,
        ILuthetusIdeComponentRenderers blazorStudioComponentRenderers,
        IFileSystemProvider fileSystemProvider,
        ITreeViewService treeViewService,
        IBackgroundTaskQueue backgroundTaskQueue)
        : base(treeViewService)
    {
        _dispatcher = dispatcher;
        _textEditorService = textEditorService;
        _blazorStudioComponentRenderers = blazorStudioComponentRenderers;
        _fileSystemProvider = fileSystemProvider;
        _backgroundTaskQueue = backgroundTaskQueue;
    }

    public override async Task<bool> OnDoubleClickAsync(
        ITreeViewCommandParameter treeViewCommandParameter)
    {
        _ = base.OnDoubleClickAsync(treeViewCommandParameter);

        if (treeViewCommandParameter.TargetNode
            is not TreeViewNamespacePath treeViewNamespacePath)
        {
            return false;
        }

        if (treeViewNamespacePath.Item is null)
            return false;

        await EditorState.OpenInEditorAsync(
            treeViewNamespacePath.Item.AbsoluteFilePath,
            true,
            _dispatcher,
            _textEditorService,
            _blazorStudioComponentRenderers,
            _fileSystemProvider,
            _backgroundTaskQueue);

        return true;
    }
}