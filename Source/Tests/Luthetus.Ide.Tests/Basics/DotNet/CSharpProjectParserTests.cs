﻿using Luthetus.Common.RazorLib.FileSystem.Classes.FilePath;
using Luthetus.Common.RazorLib.FileSystem.Classes.Local;
using Luthetus.Common.RazorLib.Misc;
using Luthetus.CompilerServices.Lang.DotNetSolution.CSharp;
using Luthetus.CompilerServices.Lang.Xml.Html.SyntaxActors;
using Luthetus.TextEditor.RazorLib.Lexing;
using Xunit;

namespace Luthetus.Ide.Tests.Basics.DotNet;

public class CSharpProjectParserTests
{
    [Fact]
    public void AdhocTest()
    {
        var localEnvironmentProvider = new LocalEnvironmentProvider();

        var projectAbsoluteFilePathString = @"C:\Users\hunte\Repos\Demos\BlazorCrudApp\BlazorCrudApp\BlazorCrudApp.csproj";

        var projectAbsoluteFilePath = new AbsolutePath(
            projectAbsoluteFilePathString,
            false,
            localEnvironmentProvider);

        var resourceUri = new ResourceUri(string.Empty);

        var htmlLexer = new TextEditorHtmlLexer(resourceUri);

        var textSpans = htmlLexer.Lex(PROJECT_TEST_DATA, RenderStateKey.NewKey());

        var htmlSyntaxUnit = HtmlSyntaxTree.ParseText(
            resourceUri,
            PROJECT_TEST_DATA);

        var syntaxNodeRoot = htmlSyntaxUnit.RootTagSyntax;

        var cSharpProjectSyntaxWalker = new CSharpProjectSyntaxWalker();

        cSharpProjectSyntaxWalker.Visit(syntaxNodeRoot);

        var packageReferences = cSharpProjectSyntaxWalker.TagNodes
            .Where(ts => (ts.OpenTagNameNode?.TextEditorTextSpan.GetText() ?? string.Empty) == "PackageReference")
            .ToList();

        var attributeNameValueTuples = packageReferences
            .SelectMany(x => x.AttributeNodes)
            .Select(x => (
                $"Name: {x.AttributeNameSyntax.TextEditorTextSpan.GetText()}",
                $"Value: {x.AttributeValueSyntax.TextEditorTextSpan.GetText()}"))
            .ToArray();
    }

    [Fact]
    public void GetReferencedNugetPackages()
    {
        var localEnvironmentProvider = new LocalEnvironmentProvider();

        var projectAbsoluteFilePathString = @"C:\Users\hunte\Repos\Demos\BlazorCrudApp\BlazorCrudApp\BlazorCrudApp.csproj";

        var projectAbsoluteFilePath = new AbsolutePath(
            projectAbsoluteFilePathString,
            false,
            localEnvironmentProvider);

        var resourceUri = new ResourceUri(projectAbsoluteFilePathString);

        var htmlLexer = new TextEditorHtmlLexer(resourceUri);

        var textSpans = htmlLexer.Lex(PROJECT_TEST_DATA, RenderStateKey.NewKey());

        var htmlSyntaxUnit = HtmlSyntaxTree.ParseText(
            resourceUri,
            PROJECT_TEST_DATA);

        var syntaxNodeRoot = htmlSyntaxUnit.RootTagSyntax;

        var cSharpProjectSyntaxWalker = new CSharpProjectSyntaxWalker();

        cSharpProjectSyntaxWalker.Visit(syntaxNodeRoot);

        var packageReferences = cSharpProjectSyntaxWalker.TagNodes
            .Where(ts => (ts.OpenTagNameNode?.TextEditorTextSpan.GetText() ?? string.Empty) == "PackageReference")
            .ToList();

        var attributeNameValueTuples = packageReferences
            .SelectMany(x => x.AttributeNodes)
            .Select(x => (
                $"Name: {x.AttributeNameSyntax.TextEditorTextSpan.GetText()}",
                $"Value: {x.AttributeValueSyntax.TextEditorTextSpan.GetText()}"))
            .ToArray();
    }

    private const string PROJECT_TEST_DATA = @"<Project Sdk=""Microsoft.NET.Sdk.Web"">

  <PropertyGroup>
    <TargetFramework>net6.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>

	<ItemGroup>
		<PackageReference Include=""Fluxor.Blazor.Web"" Version=""5.7.0"" />
	</ItemGroup>
</Project>
";
}