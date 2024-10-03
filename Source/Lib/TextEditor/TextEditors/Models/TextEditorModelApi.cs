using System.Collections.Immutable;
using Microsoft.AspNetCore.Components.Web;
using Fluxor;
using Luthetus.Common.RazorLib.BackgroundTasks.Models;
using Luthetus.Common.RazorLib.Keys.Models;
using Luthetus.TextEditor.RazorLib.CompilerServices.Interfaces;
using Luthetus.TextEditor.RazorLib.Decorations.Models;
using Luthetus.TextEditor.RazorLib.Lexers.Models;
using Luthetus.TextEditor.RazorLib.Rows.Models;
using Luthetus.TextEditor.RazorLib.TextEditors.States;
using Luthetus.Common.RazorLib.Keymaps.Models;

namespace Luthetus.TextEditor.RazorLib.TextEditors.Models;

public class TextEditorModelApi : ITextEditorModelApi
{
    private readonly ITextEditorService _textEditorService;
    private readonly IDecorationMapperRegistry _decorationMapperRegistry;
    private readonly ICompilerServiceRegistry _compilerServiceRegistry;
    private readonly IBackgroundTaskService _backgroundTaskService;
    private readonly IDispatcher _dispatcher;

    public TextEditorModelApi(
        ITextEditorService textEditorService,
        IDecorationMapperRegistry decorationMapperRegistry,
        ICompilerServiceRegistry compilerServiceRegistry,
        IBackgroundTaskService backgroundTaskService,
        IDispatcher dispatcher)
    {
        _textEditorService = textEditorService;
        _decorationMapperRegistry = decorationMapperRegistry;
        _compilerServiceRegistry = compilerServiceRegistry;
        _backgroundTaskService = backgroundTaskService;
        _dispatcher = dispatcher;
    }

    #region CREATE_METHODS
    public void RegisterCustom(TextEditorModel model)
    {
        _dispatcher.Dispatch(new TextEditorState.RegisterModelAction(
            TextEditorService.AuthenticatedActionKey,
            model));
    }

    public void RegisterTemplated(
        string extensionNoPeriod,
        ResourceUri resourceUri,
        DateTime resourceLastWriteTime,
        string initialContent,
        string? overrideDisplayTextForFileExtension = null)
    {
        var model = new TextEditorModel(
            resourceUri,
            resourceLastWriteTime,
            overrideDisplayTextForFileExtension ?? extensionNoPeriod,
            initialContent,
            _decorationMapperRegistry.GetDecorationMapper(extensionNoPeriod),
            _compilerServiceRegistry.GetCompilerService(extensionNoPeriod));

        _dispatcher.Dispatch(new TextEditorState.RegisterModelAction(
            TextEditorService.AuthenticatedActionKey,
            model));
    }
    #endregion

    #region READ_METHODS
    public ImmutableArray<TextEditorViewModel> GetViewModelsOrEmpty(ResourceUri resourceUri)
    {
        return _textEditorService.TextEditorStateWrap.Value.ViewModelList
            .Where(x => x.ResourceUri == resourceUri)
            .ToImmutableArray();
    }

    public string? GetAllText(ResourceUri resourceUri)
    {
    	var exists = _textEditorService.TextEditorStateWrap.Value.__ModelList.TryGetValue(
			resourceUri, out var inModel);
		
		if (exists)
	        return inModel.GetAllText();
                
        return null;
    }

    public TextEditorModel? GetOrDefault(ResourceUri resourceUri)
    {
        var exists = _textEditorService.TextEditorStateWrap.Value.__ModelList.TryGetValue(
			resourceUri, out var inModel);
		
		return inModel;
    }

    public Dictionary<ResourceUri, TextEditorModel> GetModels()
    {
        return _textEditorService.TextEditorStateWrap.Value.__ModelList;
    }
    #endregion

    #region UPDATE_METHODS
    public void UndoEdit(
	    ITextEditorEditContext editContext,
        TextEditorModelModifier modelModifier)
    {
        modelModifier.UndoEdit();
    }

    public void SetUsingLineEndKind(
        ITextEditorEditContext editContext,
        TextEditorModelModifier modelModifier,
        LineEndKind lineEndKind)
    {
        modelModifier.SetLineEndKindPreference(lineEndKind);
    }

    public void SetResourceData(
        ITextEditorEditContext editContext,
        TextEditorModelModifier modelModifier,
        DateTime resourceLastWriteTime)
    {
        modelModifier.SetResourceData(modelModifier.ResourceUri, resourceLastWriteTime);
    }

    public void Reload(
        ITextEditorEditContext editContext,
        TextEditorModelModifier modelModifier,
        string content,
        DateTime resourceLastWriteTime)
    {
        modelModifier.SetContent(content);
        modelModifier.SetResourceData(modelModifier.ResourceUri, resourceLastWriteTime);
    }

    public void RedoEdit(
    	ITextEditorEditContext editContext,
        TextEditorModelModifier modelModifier)
    {
        modelModifier.RedoEdit();
    }

    public void InsertText(
        ITextEditorEditContext editContext,
        TextEditorModelModifier modelModifier,
        CursorModifierBagTextEditor cursorModifierBag,
        string content,
        CancellationToken cancellationToken)
    {
        modelModifier.Insert(content, cursorModifierBag, cancellationToken: cancellationToken);
    }

    public void InsertTextUnsafe(
        ITextEditorEditContext editContext,
        TextEditorModelModifier modelModifier,
        CursorModifierBagTextEditor cursorModifierBag,
        string content,
        CancellationToken cancellationToken)
    {
        modelModifier.Insert(content, cursorModifierBag, cancellationToken: cancellationToken);
    }

    public void HandleKeyboardEvent(
        ITextEditorEditContext editContext,
        TextEditorModelModifier modelModifier,
        CursorModifierBagTextEditor cursorModifierBag,
        KeymapArgs keymapArgs,
        CancellationToken cancellationToken)
    {
        modelModifier.HandleKeyboardEvent(keymapArgs, cursorModifierBag, cancellationToken);
    }

    public void HandleKeyboardEventUnsafe(
        ITextEditorEditContext editContext,
        TextEditorModelModifier modelModifier,
        CursorModifierBagTextEditor cursorModifierBag,
        KeymapArgs keymapArgs,
        CancellationToken cancellationToken)
    {
        modelModifier.HandleKeyboardEvent(keymapArgs, cursorModifierBag, cancellationToken);
    }

    public void DeleteTextByRange(
        ITextEditorEditContext editContext,
        TextEditorModelModifier modelModifier,
        CursorModifierBagTextEditor cursorModifierBag,
        int count,
        CancellationToken cancellationToken)
    {
        modelModifier.DeleteByRange(count, cursorModifierBag, cancellationToken);
    }

    public void DeleteTextByRangeUnsafe(
        ITextEditorEditContext editContext,
        TextEditorModelModifier modelModifier,
        CursorModifierBagTextEditor cursorModifierBag,
        int count,
        CancellationToken cancellationToken)
    {
        modelModifier.DeleteByRange(count, cursorModifierBag, cancellationToken);
    }

    public void DeleteTextByMotion(
        ITextEditorEditContext editContext,
        TextEditorModelModifier modelModifier,
        CursorModifierBagTextEditor cursorModifierBag,
        MotionKind motionKind,
        CancellationToken cancellationToken)
    {
        modelModifier.DeleteTextByMotion(motionKind, cursorModifierBag, cancellationToken);
    }

    public void DeleteTextByMotionUnsafe(
        ITextEditorEditContext editContext,
        TextEditorModelModifier modelModifier,
        CursorModifierBagTextEditor cursorModifierBag,
        MotionKind motionKind,
        CancellationToken cancellationToken)
    {
        modelModifier.DeleteTextByMotion(motionKind, cursorModifierBag, cancellationToken);
    }

    public void AddPresentationModel(
        ITextEditorEditContext editContext,
        TextEditorModelModifier modelModifier,
        TextEditorPresentationModel emptyPresentationModel)
    {
        modelModifier.PerformRegisterPresentationModelAction(emptyPresentationModel);
    }

    public void StartPendingCalculatePresentationModel(
        ITextEditorEditContext editContext,
        TextEditorModelModifier modelModifier,
        Key<TextEditorPresentationModel> presentationKey,
        TextEditorPresentationModel emptyPresentationModel)
    {
        modelModifier.StartPendingCalculatePresentationModel(presentationKey, emptyPresentationModel);
    }

    public void CompletePendingCalculatePresentationModel(
        ITextEditorEditContext editContext,
        TextEditorModelModifier modelModifier,
        Key<TextEditorPresentationModel> presentationKey,
        TextEditorPresentationModel emptyPresentationModel,
        ImmutableArray<TextEditorTextSpan> calculatedTextSpans)
    {
        modelModifier.CompletePendingCalculatePresentationModel(
            presentationKey,
            emptyPresentationModel,
            calculatedTextSpans);
    }

    public void ApplyDecorationRange(
        ITextEditorEditContext editContext,
        TextEditorModelModifier modelModifier,
        IEnumerable<TextEditorTextSpan> textSpans)
    {
        var localRichCharacterList = modelModifier.RichCharacterList;

        var positionsPainted = new HashSet<int>();

        foreach (var textEditorTextSpan in textSpans)
        {
            for (var i = textEditorTextSpan.StartingIndexInclusive; i < textEditorTextSpan.EndingIndexExclusive; i++)
            {
                if (i < 0 || i >= localRichCharacterList.Length)
                    continue;

                modelModifier.__SetDecorationByte(i, textEditorTextSpan.DecorationByte);
                positionsPainted.Add(i);
            }
        }

        for (var i = 0; i < localRichCharacterList.Length - 1; i++)
        {
            if (!positionsPainted.Contains(i))
            {
                // DecorationByte of 0 is to be 'None'
                modelModifier.__SetDecorationByte(i, 0);
            }
        }
        
        modelModifier.ShouldReloadVirtualizationResult = true;
    }

    public void ApplySyntaxHighlighting(
        ITextEditorEditContext editContext,
        TextEditorModelModifier modelModifier)
    {
        var syntacticTextSpansList = modelModifier.CompilerService.GetTokenTextSpansFor(modelModifier.ResourceUri);
        var symbolsList = modelModifier.CompilerService.GetSymbolsFor(modelModifier.ResourceUri);

        var symbolTextSpansList = symbolsList.Select(s => s.TextSpan);

        var textSpanList = new List<TextEditorTextSpan>();
        textSpanList.AddRange(syntacticTextSpansList);
        textSpanList.AddRange(symbolTextSpansList);

        ApplyDecorationRange(
        	editContext,
            modelModifier,
            textSpanList);
    }
    #endregion

    #region DELETE_METHODS
    public void Dispose(ResourceUri resourceUri)
    {
        _dispatcher.Dispatch(new TextEditorState.DisposeModelAction(
            TextEditorService.AuthenticatedActionKey,
            resourceUri));
    }
    #endregion
}