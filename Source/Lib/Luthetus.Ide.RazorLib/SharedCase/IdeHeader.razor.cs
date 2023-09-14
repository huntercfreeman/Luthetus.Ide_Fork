using Fluxor;
using Fluxor.Blazor.Web.Components;
using Luthetus.Common.RazorLib.Dialog;
using Luthetus.Common.RazorLib.Dropdown;
using Luthetus.Common.RazorLib.Menu;
using Luthetus.Common.RazorLib.Store.DialogCase;
using Luthetus.Common.RazorLib.Store.DropdownCase;
using Luthetus.Ide.RazorLib.DotNetSolutionCase;
using Luthetus.Ide.RazorLib.DotNetSolutionCase.Displays;
using Luthetus.Ide.RazorLib.DotNetSolutionCase.States;
using Luthetus.Ide.RazorLib.DotNetSolutionCase.Views;
using Luthetus.Ide.RazorLib.EditorCase;
using Luthetus.Ide.RazorLib.FolderExplorerCase;
using Microsoft.AspNetCore.Components;
using System.Collections.Immutable;

namespace Luthetus.Ide.RazorLib.SharedCase;

public partial class IdeHeader : FluxorComponent
{
    [Inject]
    private IDispatcher Dispatcher { get; set; } = null!;
    [Inject]
    private DotNetSolutionSync DotNetSolutionSync { get; set; } = null!;

    private DropdownKey _dropdownKeyFile = DropdownKey.NewKey();
    private MenuRecord _menuFile = new(ImmutableArray<MenuOptionRecord>.Empty);
    private ElementReference? _buttonFileElementReference;

    protected override Task OnInitializedAsync()
    {
        InitializeMenuFile();

        return base.OnInitializedAsync();
    }

    private void InitializeMenuFile()
    {
        var menuOptions = new List<MenuOptionRecord>();

        // Menu Option New
        {
            var menuOptionNewDotNetSolution = new MenuOptionRecord(
                ".NET Solution",
                MenuOptionKind.Other,
                OpenNewDotNetSolutionDialog);

            var menuOptionNew = new MenuOptionRecord(
                "New",
                MenuOptionKind.Other,
                SubMenu: new MenuRecord(
                    new[]
                    {
                    menuOptionNewDotNetSolution
                    }.ToImmutableArray()));

            menuOptions.Add(menuOptionNew);
        }

        // Menu Option Open
        {
            var menuOptionOpenFile = new MenuOptionRecord(
                "File",
                MenuOptionKind.Other,
                () => Dispatcher.Dispatch(new EditorRegistry.ShowInputFileAction()));

            var menuOptionOpenDirectory = new MenuOptionRecord(
                "Directory",
                MenuOptionKind.Other,
                () => FolderExplorerRegistry.ShowInputFile(Dispatcher));

            var menuOptionOpenCSharpProject = new MenuOptionRecord(
                "C# Project - TODO: Adhoc Sln",
                MenuOptionKind.Other,
                () => Dispatcher.Dispatch(new EditorRegistry.ShowInputFileAction()));

            var menuOptionOpenDotNetSolution = new MenuOptionRecord(
                ".NET Solution",
                MenuOptionKind.Other,
                () => DotNetSolutionState.ShowInputFile(DotNetSolutionSync));

            var menuOptionOpen = new MenuOptionRecord(
                "Open",
                MenuOptionKind.Other,
                SubMenu: new MenuRecord(
                    new[]
                    {
                    menuOptionOpenFile,
                    menuOptionOpenDirectory,
                    menuOptionOpenCSharpProject,
                    menuOptionOpenDotNetSolution
                    }.ToImmutableArray()));

            menuOptions.Add(menuOptionOpen);
        }

        _menuFile = new MenuRecord(menuOptions.ToImmutableArray());
    }

    private void AddActiveDropdownKey(DropdownKey dropdownKey)
    {
        Dispatcher.Dispatch(new DropdownRegistry.AddActiveAction(
            dropdownKey));
    }

    /// <summary>
    /// TODO: Make this method abstracted into a component that takes care of the UI to show the dropdown and to restore focus when menu closed
    /// </summary>
    private async Task RestoreFocusToButtonDisplayComponentFileAsync()
    {
        try
        {
            if (_buttonFileElementReference is not null)
            {
                await _buttonFileElementReference.Value.FocusAsync();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private void OpenNewDotNetSolutionDialog()
    {
        var dialogRecord = new DialogRecord(
            DialogKey.NewKey(),
            "New .NET Solution",
            typeof(DotNetSolutionFormDisplay),
            new Dictionary<string, object?>
            {
                {
                    nameof(DotNetSolutionFormDisplay.View),
                    new DotNetSolutionFormView()
                }
            },
            null)
        {
            IsResizable = true
        };

        Dispatcher.Dispatch(new DialogRegistry.RegisterAction(
            dialogRecord));
    }
}