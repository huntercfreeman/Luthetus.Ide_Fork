﻿using Luthetus.CompilerServices.Lang.DotNetSolution;
using Luthetus.Ide.RazorLib.NugetCase;
using System.Collections.Immutable;

namespace Luthetus.Ide.ClassLib.Store.NugetPackageManagerCase;

public partial record NuGetPackageManagerRegistry
{
    public record SetSelectedProjectToModifyAction(IDotNetProject? SelectedProjectToModify);
    public record SetNugetQueryAction(string NugetQuery);
    public record SetIncludePrereleaseAction(bool IncludePrerelease);
    public record SetMostRecentQueryResultAction(ImmutableArray<NugetPackageRecord> QueryResult);
}