using Fluxor;
using Fluxor.Blazor.Web.Components;
using Luthetus.Common.RazorLib.Dialog.States;
using Luthetus.Common.RazorLib.Dropdown.States;
using Luthetus.Ide.RazorLib.DotNetSolutionCase.Displays;
using Luthetus.Ide.RazorLib.DotNetSolutionCase.States;
using Luthetus.Ide.RazorLib.EditorCase.States;
using Luthetus.Ide.RazorLib.FolderExplorerCase.States;
using Microsoft.AspNetCore.Components;
using System.Collections.Immutable;
using Luthetus.Common.RazorLib.Dialogs.Models;
using Luthetus.Common.RazorLib.Menus.Models;
using Luthetus.Common.RazorLib.Dropdowns.Models;
using Luthetus.Common.RazorLib.Keys.Models;

namespace Luthetus.Ide.RazorLib.SharedCase.Displays;

public partial class IdeHeader : FluxorComponent
{
    [Inject]
    private IDispatcher Dispatcher { get; set; } = null!;
    [Inject]
    private DotNetSolutionSync DotNetSolutionSync { get; set; } = null!;
    [Inject]
    private EditorSync EditorSync { get; set; } = null!;
    [Inject]
    private FolderExplorerSync FolderExplorerSync { get; set; } = null!;

    private Key<DropdownRecord> _dropdownKeyFile = Key<DropdownRecord>.NewKey();
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
                () => EditorSync.ShowInputFile());

            var menuOptionOpenDirectory = new MenuOptionRecord(
                "Directory",
                MenuOptionKind.Other,
                () => FolderExplorerSync.ShowInputFile());

            var menuOptionOpenCSharpProject = new MenuOptionRecord(
                "C# Project - TODO: Adhoc Sln",
                MenuOptionKind.Other,
                () => EditorSync.ShowInputFile());

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

    private void AddActiveDropdownKey(Key<DropdownRecord> dropdownKey)
    {
        Dispatcher.Dispatch(new DropdownState.AddActiveAction(
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
            Key<DialogRecord>.NewKey(),
            "New .NET Solution",
            typeof(DotNetSolutionFormDisplay),
            null,
            null)
        {
            IsResizable = true
        };

        Dispatcher.Dispatch(new DialogState.RegisterAction(
            dialogRecord));
    }
}