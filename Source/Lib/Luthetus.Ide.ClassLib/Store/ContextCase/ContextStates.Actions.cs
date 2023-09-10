using Luthetus.Ide.ClassLib.Context;
using Luthetus.Ide.ClassLib.JavaScriptObjects;
using System.Collections.Immutable;

namespace Luthetus.Ide.ClassLib.Store.ContextCase;

public partial record ContextStates
{
    public record SetActiveContextRecordsAction(ImmutableArray<ContextRecord> ContextRecords);
    public record ToggleSelectInspectionTargetAction;
    public record SetSelectInspectionTargetTrueAction;
    public record SetSelectInspectionTargetFalseAction;
    public record SetInspectionTargetAction(ImmutableArray<ContextRecord>? ContextRecords);
    public record AddMeasuredHtmlElementDimensionsAction(ContextRecord ContextRecord, ImmutableArray<ContextRecord> ContextBoundaryHeirarchy, MeasuredHtmlElementDimensions MeasuredHtmlElementDimensions);
}