﻿using Xunit;
using Luthetus.TextEditor.RazorLib.TextEditors.States;
using Luthetus.TextEditor.RazorLib.TextEditors.Models.TextEditorModels;
using System.Collections.Immutable;
using Luthetus.Common.RazorLib.FileSystems.Models;
using Luthetus.TextEditor.RazorLib.CompilerServices;
using Luthetus.TextEditor.RazorLib.Decorations.Models;
using Luthetus.TextEditor.RazorLib.Lexes.Models;
using Luthetus.TextEditor.RazorLib.TextEditors.Models;

namespace Luthetus.TextEditor.Tests.Basis.TextEditors.States;

/// <summary>
/// <see cref="TextEditorModelState"/>
/// </summary>
public class TextEditorModelStateMainTests
{
	/// <summary>
	/// <see cref="TextEditorModelState()"/>
	/// </summary>
	[Fact]
	public void Constructor()
	{
		var modelState = new TextEditorModelState();
		Assert.Equal(ImmutableList<TextEditorModel>.Empty, modelState.ModelBag);
	}

	/// <summary>
	/// <see cref="TextEditorModelState.ModelBag"/>
	/// </summary>
	[Fact]
	public void ModelBag()
	{
        var modelState = new TextEditorModelState();
        Assert.Equal(ImmutableList<TextEditorModel>.Empty, modelState.ModelBag);

		var model = new TextEditorModel(
            new ResourceUri("/unitTesting.txt"),
            DateTime.UtcNow,
            ExtensionNoPeriodFacts.TXT,
            "AlphabetSoup",
            new TextEditorDecorationMapperDefault(),
            new TextEditorCompilerServiceDefault());

		var outModelBag = modelState.ModelBag.Add(model);
        Assert.NotEqual(ImmutableList<TextEditorModel>.Empty, outModelBag);

		var outModelState = new TextEditorModelState
		{
			ModelBag = outModelBag
		};

		Assert.Equal(outModelBag, outModelState.ModelBag);
	}
}