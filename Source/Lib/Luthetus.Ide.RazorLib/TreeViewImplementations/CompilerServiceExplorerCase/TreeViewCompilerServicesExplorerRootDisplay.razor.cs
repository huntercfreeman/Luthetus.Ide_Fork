﻿using Luthetus.Common.RazorLib.TreeView.TreeViewClasses;
using Luthetus.Ide.ClassLib.ComponentRenderers.Types;
using Luthetus.Ide.ClassLib.TreeViewImplementations;
using Luthetus.Ide.ClassLib.TreeViewImplementations.CompilerServiceExplorerCase;
using Microsoft.AspNetCore.Components;

namespace Luthetus.Ide.RazorLib.TreeViewImplementations.CompilerServiceExplorerCase;

public partial class TreeViewCompilerServicesExplorerRootDisplay : ComponentBase, ITreeViewCompilerServicesExplorerRootRendererType
{
    [Parameter, EditorRequired]
    public TreeViewCompilerServicesExplorerRoot TreeViewCompilerServicesExplorerRoot { get; set; } = null!;
}