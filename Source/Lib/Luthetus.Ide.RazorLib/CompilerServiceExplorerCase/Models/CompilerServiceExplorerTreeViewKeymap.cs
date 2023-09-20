﻿using Fluxor;
using Luthetus.Common.RazorLib.Commands.Models;
using Luthetus.Common.RazorLib.Keyboard.Models;
using Luthetus.Common.RazorLib.TreeView.Models;
using Luthetus.Ide.RazorLib.EditorCase.States;
using Luthetus.Ide.RazorLib.TreeViewImplementationsCase.Models;

namespace Luthetus.Ide.RazorLib.CompilerServiceExplorerCase.Models;

public class CompilerServiceExplorerTreeViewKeyboardEventHandler : TreeViewKeyboardEventHandler
{
    private readonly EditorSync _editorSync;

    public CompilerServiceExplorerTreeViewKeyboardEventHandler(
        EditorSync editorSync,
        ITreeViewService treeViewService)
        : base(treeViewService)
    {
        _editorSync = editorSync;
    }

    public override void OnKeyDown(TreeViewCommandParameter treeViewCommandParameter)
    {
        if (treeViewCommandParameter.KeyboardEventArgs is null)
            return;

        base.OnKeyDown(treeViewCommandParameter);

        switch (treeViewCommandParameter.KeyboardEventArgs.Code)
        {
            case KeyboardKeyFacts.WhitespaceCodes.ENTER_CODE:
                InvokeOpenInEditor(treeViewCommandParameter, true);
                return;
            case KeyboardKeyFacts.WhitespaceCodes.SPACE_CODE:
                InvokeOpenInEditor(treeViewCommandParameter, false);
                return;
        }

        if (treeViewCommandParameter.KeyboardEventArgs.CtrlKey)
        {
            CtrlModifiedKeymap(treeViewCommandParameter);
            return;
        }
        else if (treeViewCommandParameter.KeyboardEventArgs.AltKey)
        {
            AltModifiedKeymap(treeViewCommandParameter);
            return;
        }
    }

    private void CtrlModifiedKeymap(TreeViewCommandParameter treeViewCommandParameter)
    {
        if (treeViewCommandParameter.KeyboardEventArgs is null)
            return;

        if (treeViewCommandParameter.KeyboardEventArgs.AltKey)
        {
            CtrlAltModifiedKeymap(treeViewCommandParameter);
            return;
        }

        switch (treeViewCommandParameter.KeyboardEventArgs.Key)
        {
            default:
                return;
        }
    }

    private void AltModifiedKeymap(TreeViewCommandParameter treeViewCommandParameter)
    {
        return;
    }

    private void CtrlAltModifiedKeymap(TreeViewCommandParameter treeViewCommandParameter)
    {
        return;
    }

    private void InvokeOpenInEditor(
        TreeViewCommandParameter treeViewCommandParameter,
        bool shouldSetFocusToEditor)
    {
        var activeNode = treeViewCommandParameter.TreeViewState.ActiveNode;

        if (activeNode is not TreeViewNamespacePath treeViewNamespacePath)
            return;

        _editorSync.Dispatcher.Dispatch(new EditorState.OpenInEditorAction(
            _editorSync,
            treeViewNamespacePath.Item.AbsolutePath,
            shouldSetFocusToEditor));

        return;
    }
}