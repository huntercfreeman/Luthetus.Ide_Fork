using System.Collections.Immutable;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Luthetus.Common.RazorLib.JsRuntimes.Models;
using Luthetus.TextEditor.RazorLib.Cursors.Models;
using Luthetus.TextEditor.RazorLib.Edits.Models;
using Luthetus.TextEditor.RazorLib.Lexers.Models;

namespace Luthetus.TextEditor.RazorLib.Commands.Models.Defaults;

public static class TextEditorCommandDefaultFacts
{
    public static readonly TextEditorCommand DoNothingDiscard = new(
        "DoNothingDiscard", "defaults_do-nothing-discard", false, false, TextEditKind.None, null,
        interfaceCommandArgs => Task.CompletedTask);

    public static readonly TextEditorCommand Copy = new(
        "Copy", "defaults_copy", false, false, TextEditKind.None, null,
        interfaceCommandArgs =>
        {
            var commandArgs = (TextEditorCommandArgs)interfaceCommandArgs;
			TextEditorCommandDefaultFunctions.Copy(commandArgs.EditContext)
        });

    public static readonly TextEditorCommand Cut = new(
        "Cut", "defaults_cut", false, true, TextEditKind.None, null,
        interfaceCommandArgs =>
        {
            var commandArgs = (TextEditorCommandArgs)interfaceCommandArgs;
			TextEditorCommandDefaultFunctions.Cut(commandArgs.EditContext);
        });

    public static readonly TextEditorCommand PasteCommand = new(
        "Paste", "defaults_paste", false, true, TextEditKind.Other, "defaults_paste",
        interfaceCommandArgs =>
        {
            var commandArgs = (TextEditorCommandArgs)interfaceCommandArgs;
			TextEditorCommandDefaultFunctions.Paste(commandArgs.EditContext);
        });

    public static readonly TextEditorCommand Save = new(
        "Save", "defaults_save", false, false, TextEditKind.None, null,
        interfaceCommandArgs =>
        {
            var commandArgs = (TextEditorCommandArgs)interfaceCommandArgs;
			TextEditorCommandDefaultFunctions.Save(commandArgs.EditContext);
        });

    public static readonly TextEditorCommand SelectAll = new(
        "Select All", "defaults_select-all", false, false, TextEditKind.None, null,
        interfaceCommandArgs =>
        {
            var commandArgs = (TextEditorCommandArgs)interfaceCommandArgs;
			TextEditorCommandDefaultFunctions.SelectAll(commandArgs.EditContext);
        });

    public static readonly TextEditorCommand Undo = new(
        "Undo", "defaults_undo", false, true, TextEditKind.None, null,
        interfaceCommandArgs =>
        {
            var commandArgs = (TextEditorCommandArgs)interfaceCommandArgs;
			TextEditorCommandDefaultFunctions.Undo(commandArgs.EditContext);
        });

    public static readonly TextEditorCommand Redo = new(
        "Redo", "defaults_redo", false, true, TextEditKind.None, null,
        interfaceCommandArgs =>
        {
            var commandArgs = (TextEditorCommandArgs)interfaceCommandArgs;
            TextEditorCommandDefaultFunctions.Redo(commandArgs.EditContext);
        });

    public static readonly TextEditorCommand Remeasure = new(
        "Remeasure", "defaults_remeasure", false, false, TextEditKind.None, null,
        interfaceCommandArgs =>
        {
            var commandArgs = (TextEditorCommandArgs)interfaceCommandArgs;
            TextEditorCommandDefaultFunctions.Remeasure(commandArgs.EditContext)
        });

    public static readonly TextEditorCommand RefreshSyntaxHighlighting = new(
        "Refresh Syntax Highlighting", "defaults_refresh_syntax_highlighting", false, false, TextEditKind.None, null,
        interfaceCommandArgs =>
        {
            var commandArgs = (TextEditorCommandArgs)interfaceCommandArgs;
            
            modelModifier.CompilerService.ResourceWasModified(
				modelModifier.ResourceUri,
				ImmutableArray<TextEditorTextSpan>.Empty);
        });

    public static readonly TextEditorCommand ScrollLineDown = new(
        "Scroll Line Down", "defaults_scroll-line-down", false, false, TextEditKind.None, null,
        interfaceCommandArgs =>
        {
            var commandArgs = (TextEditorCommandArgs)interfaceCommandArgs;
            TextEditorCommandDefaultFunctions.ScrollLineDown(commandArgs.EditContext);
        });

    public static readonly TextEditorCommand ScrollLineUp = new(
        "Scroll Line Up", "defaults_scroll-line-up", false, false, TextEditKind.None, null,
        interfaceCommandArgs =>
        {
            var commandArgs = (TextEditorCommandArgs)interfaceCommandArgs;
            TextEditorCommandDefaultFunctions.ScrollLineUp(commandArgs.EditContext);
        });

    public static readonly TextEditorCommand ScrollPageDown = new(
        "Scroll Page Down", "defaults_scroll-page-down", false, false, TextEditKind.None, null,
        interfaceCommandArgs =>
        {
            var commandArgs = (TextEditorCommandArgs)interfaceCommandArgs;
            TextEditorCommandDefaultFunctions.ScrollPageDown(commandArgs.EditContext);
        });

    public static readonly TextEditorCommand ScrollPageUp = new(
        "Scroll Page Up", "defaults_scroll-page-up", false, false, TextEditKind.None, null,
        interfaceCommandArgs =>
        {
            var commandArgs = (TextEditorCommandArgs)interfaceCommandArgs;
            TextEditorCommandDefaultFunctions.ScrollPageUp(commandArgs.EditContext);
        });

    public static readonly TextEditorCommand CursorMovePageBottom = new(
        "Move Cursor to Bottom of the Page", "defaults_cursor-move-page-bottom", false, false, TextEditKind.None, null,
        interfaceCommandArgs =>
        {
            var commandArgs = (TextEditorCommandArgs)interfaceCommandArgs;
            TextEditorCommandDefaultFunctions.CursorMovePageBottom(commandArgs.EditContext);
        });

    public static readonly TextEditorCommand CursorMovePageTop = new(
        "Move Cursor to Top of the Page", "defaults_cursor-move-page-top", false, false, TextEditKind.None, null,
        interfaceCommandArgs =>
        {
            var commandArgs = (TextEditorCommandArgs)interfaceCommandArgs;
			TextEditorCommandDefaultFunctions.CursorMovePageTop(commandArgs.EditContext);
        });

    public static readonly TextEditorCommand Duplicate = new(
        "Duplicate", "defaults_duplicate", false, false, TextEditKind.None, null,
        interfaceCommandArgs =>
        {
            var commandArgs = (TextEditorCommandArgs)interfaceCommandArgs;
			TextEditorCommandDefaultFunctions.Duplicate(commandArgs.EditContext);
        });

    public static readonly TextEditorCommand IndentMore = new(
        "Indent More", "defaults_indent-more", false, false, TextEditKind.None, null,
        interfaceCommandArgs =>
        {
            var commandArgs = (TextEditorCommandArgs)interfaceCommandArgs;
			TextEditorCommandDefaultFunctions.IndentMore(commandArgs.EditContext);
        });

    public static readonly TextEditorCommand IndentLess = new(
        "Indent Less", "defaults_indent-less", false, false, TextEditKind.None, null,
        interfaceCommandArgs =>
        {
            var commandArgs = (TextEditorCommandArgs)interfaceCommandArgs;
			TextEditorCommandDefaultFunctions.IndentLess(commandArgs.EditContext);
        });

    public static readonly TextEditorCommand ClearTextSelection = new(
        "ClearTextSelection", "defaults_clear-text-selection", false, false, TextEditKind.None, null,
        interfaceCommandArgs =>
        {
            var commandArgs = (TextEditorCommandArgs)interfaceCommandArgs;
			TextEditorCommandDefaultFunctions.ClearTextSelection(commandArgs.EditContext);
        });

    public static readonly TextEditorCommand NewLineBelow = new(
        "NewLineBelow", "defaults_new-line-below", false, false, TextEditKind.None, null,
        interfaceCommandArgs =>
        {
            var commandArgs = (TextEditorCommandArgs)interfaceCommandArgs;
			TextEditorCommandDefaultFunctions.NewLineBelow(commandArgs.EditContext);
        });

    public static readonly TextEditorCommand NewLineAbove = new(
        "NewLineAbove", "defaults_new-line-above", false, false, TextEditKind.None, null,
        interfaceCommandArgs =>
        {
            var commandArgs = (TextEditorCommandArgs)interfaceCommandArgs;
			TextEditorCommandDefaultFunctions.NewLineAbove(commandArgs.EditContext);
        });
    
    public static readonly TextEditorCommand MoveLineDown = new(
        "MoveLineDown", "defaults_move-line-down", false, false, TextEditKind.None, null,
        interfaceCommandArgs =>
        {
            var commandArgs = (TextEditorCommandArgs)interfaceCommandArgs;
            TextEditorCommandDefaultFunctions.MoveLineDown(commandArgs.EditContext);
        });
    
    public static readonly TextEditorCommand MoveLineUp = new(
        "MoveLineUp", "defaults_move-line-up", false, false, TextEditKind.None, null,
        interfaceCommandArgs =>
        {
            var commandArgs = (TextEditorCommandArgs)interfaceCommandArgs;

            commandArgs.TextEditorService.PostUnique(
                nameof(MoveLineUp),
                TextEditorCommandDefaultFunctions.MoveLineUpFactory(
                    commandArgs.ModelResourceUri,
                    commandArgs.ViewModelKey,
                    commandArgs));
			return Task.CompletedTask;
        })
    {
        TextEditorFuncFactory = interfaceCommandArgs =>
        {
            var commandArgs = (TextEditorCommandArgs)interfaceCommandArgs;

            return TextEditorCommandDefaultFunctions.MoveLineUpFactory(
                commandArgs.ModelResourceUri,
                commandArgs.ViewModelKey,
                commandArgs);
        }
    };

    public static TextEditorCommand GoToMatchingCharacterFactory(bool shouldSelectText) => new(
        "GoToMatchingCharacter", "defaults_go-to-matching-character", false, true, TextEditKind.None, null,
        interfaceCommandArgs =>
        {
            var commandArgs = (TextEditorCommandArgs)interfaceCommandArgs;
            commandArgs.ShouldSelectText = shouldSelectText;

            commandArgs.TextEditorService.PostUnique(
                nameof(GoToMatchingCharacterFactory),
                TextEditorCommandDefaultFunctions.GoToMatchingCharacterFactory(
                    commandArgs.ModelResourceUri,
                    commandArgs.ViewModelKey,
                    commandArgs));
			return Task.CompletedTask;
        })
    {
        TextEditorFuncFactory = interfaceCommandArgs =>
        {
            var commandArgs = (TextEditorCommandArgs)interfaceCommandArgs;

            return TextEditorCommandDefaultFunctions.GoToMatchingCharacterFactory(
                commandArgs.ModelResourceUri,
                commandArgs.ViewModelKey,
                commandArgs);
        }
    };

    public static readonly TextEditorCommand RelatedFilesQuickPick = new(
        "RelatedFilesQuickPick", "defaults_related-files-quick-pick", false, true, TextEditKind.None, null,
        interfaceCommandArgs =>
        {
            var commandArgs = (TextEditorCommandArgs)interfaceCommandArgs;

            commandArgs.TextEditorService.PostRedundant(
                nameof(RelatedFilesQuickPick),
                commandArgs.ModelResourceUri,
                commandArgs.ViewModelKey,
                TextEditorCommandDefaultFunctions.RelatedFilesQuickPickFactory(
                    commandArgs.ModelResourceUri,
                    commandArgs.ViewModelKey,
                    commandArgs));
			return Task.CompletedTask;
        })
    {
        TextEditorFuncFactory = interfaceCommandArgs =>
        {
            var commandArgs = (TextEditorCommandArgs)interfaceCommandArgs;

            return TextEditorCommandDefaultFunctions.RelatedFilesQuickPickFactory(
                commandArgs.ModelResourceUri,
                commandArgs.ViewModelKey,
                commandArgs);
        }
    };
    
    public static readonly TextEditorCommand GoToDefinition = new(
        "GoToDefinition", "defaults_go-to-definition", false, true, TextEditKind.None, null,
        interfaceCommandArgs =>
        {
            var commandArgs = (TextEditorCommandArgs)interfaceCommandArgs;

            commandArgs.TextEditorService.PostRedundant(
                nameof(GoToDefinition),
                commandArgs.ModelResourceUri,
                commandArgs.ViewModelKey,
                TextEditorCommandDefaultFunctions.GoToDefinitionFactory(
                    commandArgs.ModelResourceUri,
                    commandArgs.ViewModelKey,
                    commandArgs));
			return Task.CompletedTask;
        })
    {
        TextEditorFuncFactory = interfaceCommandArgs =>
        {
            var commandArgs = (TextEditorCommandArgs)interfaceCommandArgs;

            return TextEditorCommandDefaultFunctions.GoToDefinitionFactory(
                commandArgs.ModelResourceUri,
                commandArgs.ViewModelKey,
                commandArgs);
        }
    };

    public static readonly TextEditorCommand ShowFindAllDialog = new(
        "OpenFindDialog", "defaults_open-find-dialog", false, false, TextEditKind.None, null,
        interfaceCommandArgs =>
        {
            var commandArgs = (TextEditorCommandArgs)interfaceCommandArgs;

            commandArgs.TextEditorService.PostRedundant(
                nameof(ShowFindAllDialog),
                commandArgs.ModelResourceUri,
                commandArgs.ViewModelKey,
                TextEditorCommandDefaultFunctions.ShowFindAllDialogFactory(
                    commandArgs.ModelResourceUri,
                    commandArgs.ViewModelKey,
                    commandArgs));
			return Task.CompletedTask;
        })
    {
        TextEditorFuncFactory = interfaceCommandArgs =>
        {
            var commandArgs = (TextEditorCommandArgs)interfaceCommandArgs;

            return TextEditorCommandDefaultFunctions.ShowFindAllDialogFactory(
                commandArgs.ModelResourceUri,
                commandArgs.ViewModelKey,
                commandArgs);
        }
    };

    /// <summary>
    /// <see cref="ShowTooltipByCursorPosition"/> is to fire the @onmouseover event
    /// so to speak. Such that a tooltip appears if one were to have moused over a symbol or etc...
    /// </summary>
    public static readonly TextEditorCommand ShowTooltipByCursorPosition = new(
        "ShowTooltipByCursorPosition", "defaults_show-tooltip-by-cursor-position", false, true, TextEditKind.None, null,
        interfaceCommandArgs =>
        {
            var commandArgs = (TextEditorCommandArgs)interfaceCommandArgs;

            commandArgs.TextEditorService.PostRedundant(
                nameof(ShowTooltipByCursorPosition),
                commandArgs.ModelResourceUri,
                commandArgs.ViewModelKey,
                TextEditorCommandDefaultFunctions.ShowTooltipByCursorPositionFactory(
                    commandArgs.ModelResourceUri,
                    commandArgs.ViewModelKey,
                    commandArgs));
			return Task.CompletedTask;
        })
    {
        TextEditorFuncFactory = interfaceCommandArgs =>
        {
            var commandArgs = (TextEditorCommandArgs)interfaceCommandArgs;

            return TextEditorCommandDefaultFunctions.ShowTooltipByCursorPositionFactory(
                commandArgs.ModelResourceUri,
                commandArgs.ViewModelKey,
                commandArgs);
        }
    };

    public static readonly TextEditorCommand ShowFindOverlay = new(
        "ShowFindOverlay", "defaults_show-find-overlay", false, true, TextEditKind.None, null,
        interfaceCommandArgs =>
        {
            var commandArgs = (TextEditorCommandArgs)interfaceCommandArgs;

            commandArgs.TextEditorService.PostRedundant(
                nameof(ShowFindOverlay),
                commandArgs.ModelResourceUri,
                commandArgs.ViewModelKey,
                async editContext =>
                {
                    var viewModelModifier = editContext.GetViewModelModifier(commandArgs.ViewModelKey);

                    if (viewModelModifier is null)
                        return;

					// If the user has an active text selection,
					// then populate the find overlay with their selection.
					{
						var modelModifier = editContext.GetModelModifier(commandArgs.ModelResourceUri);
			            var cursorModifierBag = editContext.GetCursorModifierBag(viewModelModifier?.ViewModel);
			            var primaryCursorModifier = editContext.GetPrimaryCursorModifier(cursorModifierBag);
			
			            if (modelModifier is null || cursorModifierBag is null || primaryCursorModifier is null)
			                return;
			
			            var selectedText = TextEditorSelectionHelper.GetSelectedText(primaryCursorModifier, modelModifier);

						if (selectedText is not null)
						{
							viewModelModifier.ViewModel = viewModelModifier.ViewModel with
	                        {
	                            FindOverlayValue = selectedText,
	                        };
						}
					}

                    if (viewModelModifier.ViewModel.ShowFindOverlay)
                    {
                        await commandArgs.ServiceProvider.GetRequiredService<IJSRuntime>().GetLuthetusCommonApi()
                            .FocusHtmlElementById(viewModelModifier.ViewModel.FindOverlayId)
                            .ConfigureAwait(false);
                    }
                    else
                    {
                        viewModelModifier.ViewModel = viewModelModifier.ViewModel with
                        {
                            ShowFindOverlay = true,
                        };
                    }
                    
                    return;
                });
			return Task.CompletedTask;
        });
}