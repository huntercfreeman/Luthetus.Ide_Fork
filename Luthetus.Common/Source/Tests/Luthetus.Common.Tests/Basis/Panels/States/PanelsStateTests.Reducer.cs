﻿using Fluxor;
using Luthetus.Common.RazorLib.Contexts.Models;
using Luthetus.Common.RazorLib.Dimensions.Models;
using Luthetus.Common.RazorLib.Icons.Displays.Codicon;
using Luthetus.Common.RazorLib.Keys.Models;
using Luthetus.Common.RazorLib.Panels.Models;
using Luthetus.Common.RazorLib.Panels.States;
using Luthetus.Common.RazorLib.Resizes.Displays;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Immutable;

namespace Luthetus.Common.Tests.Basis.Panels.States;

/// <summary>
/// <see cref="PanelsState.Reducer"/>
/// </summary>
public class PanelsStateReducerTests
{
    /// <summary>
    /// <see cref="PanelsState.Reducer.ReduceRegisterPanelGroupAction(PanelsState, PanelsState.RegisterPanelGroupAction)"/>
    /// </summary>
    [Fact]
    public void ReduceRegisterPanelGroupAction()
    {
        InitializePanelsStateReducerTests(
            out var serviceProvider,
            out var panelsStateWrap,
            out var dispatcher,
            out var panelGroup,
            out var panelTab);

        Assert.DoesNotContain(panelsStateWrap.Value.PanelGroupBag, x => x == panelGroup);
        
        dispatcher.Dispatch(new PanelsState.RegisterPanelGroupAction(panelGroup));
        Assert.Contains(panelsStateWrap.Value.PanelGroupBag, x => x == panelGroup);
    }

    /// <summary>
    /// <see cref="PanelsState.Reducer.ReduceDisposePanelGroupAction(PanelsState, PanelsState.DisposePanelGroupAction)"/>
    /// </summary>
    [Fact]
    public void ReduceDisposePanelGroupAction()
    {
        InitializePanelsStateReducerTests(
            out var serviceProvider,
            out var panelsStateWrap,
            out var dispatcher,
            out var panelGroup,
            out var panelTab);

        Assert.DoesNotContain(panelsStateWrap.Value.PanelGroupBag, x => x == panelGroup);

        dispatcher.Dispatch(new PanelsState.RegisterPanelGroupAction(panelGroup));
        Assert.Contains(panelsStateWrap.Value.PanelGroupBag, x => x == panelGroup);

        dispatcher.Dispatch(new PanelsState.DisposePanelGroupAction(panelGroup.Key));
        Assert.DoesNotContain(panelsStateWrap.Value.PanelGroupBag, x => x == panelGroup);
    }

    /// <summary>
    /// <see cref="PanelsState.Reducer.ReduceRegisterPanelTabAction(PanelsState, PanelsState.RegisterPanelTabAction)"/>
    /// </summary>
    [Fact]
    public void ReduceRegisterPanelTabAction()
    {
        InitializePanelsStateReducerTests(
            out var serviceProvider,
            out var panelsStateWrap,
            out var dispatcher,
            out var panelGroup,
            out var panelTab);

        dispatcher.Dispatch(new PanelsState.RegisterPanelGroupAction(panelGroup));
        Assert.Contains(panelsStateWrap.Value.PanelGroupBag, x => x == panelGroup);

        // Ensure the panelGroup is not empty.
        // This allows one to test the 'insertAtIndexZero' logic.
        // That is to say, insert at start or end logic.
        {
            dispatcher.Dispatch(new PanelsState.RegisterPanelTabAction(
                panelGroup.Key,
                panelTab,
                true));

            var localPanelGroup = panelsStateWrap.Value.PanelGroupBag.Single(x => x.Key == panelGroup.Key);
            Assert.Contains(localPanelGroup.TabBag, x => x == panelTab);
            Assert.NotEmpty(localPanelGroup.TabBag);
        }

        // InsertAtIndexZero == false
        {
            var insertAtIndexZero = false;

            var localPanelTab = new PanelTab(
                    Key<PanelTab>.NewKey(),
                    panelGroup.ElementDimensions,
                    new(),
                    // Awkwardly need to provide a type here. Will provide an Icon but this usually
                    // would be more along the lines of "typeof(SolutionExplorerDisplay)"
                    typeof(IconCSharpClass),
                    typeof(IconFolder),
                    "Solution Explorer")
            {
                ContextRecordKey = ContextFacts.SolutionExplorerContext.ContextKey
            };

            dispatcher.Dispatch(new PanelsState.RegisterPanelTabAction(
                panelGroup.Key,
                localPanelTab,
                insertAtIndexZero));

            var localPanelGroup = panelsStateWrap.Value.PanelGroupBag.Single(x => x.Key == panelGroup.Key);
            Assert.Contains(localPanelGroup.TabBag, x => x == localPanelTab);
            Assert.Equal(localPanelGroup.TabBag.Last(), localPanelTab);
        }

        // InsertAtIndexZero == true
        {
            var insertAtIndexZero = true;

            var localPanelTab = new PanelTab(
                    Key<PanelTab>.NewKey(),
                    panelGroup.ElementDimensions,
                    new(),
                    // Awkwardly need to provide a type here. Will provide an Icon but this usually
                    // would be more along the lines of "typeof(SolutionExplorerDisplay)"
                    typeof(IconCSharpClass),
                    typeof(IconFolder),
                    "Solution Explorer")
            {
                ContextRecordKey = ContextFacts.SolutionExplorerContext.ContextKey
            };

            dispatcher.Dispatch(new PanelsState.RegisterPanelTabAction(
                panelGroup.Key,
                localPanelTab,
                insertAtIndexZero));

            var localPanelGroup = panelsStateWrap.Value.PanelGroupBag.Single(x => x.Key == panelGroup.Key);
            Assert.Contains(localPanelGroup.TabBag, x => x == localPanelTab);
            Assert.Equal(localPanelGroup.TabBag.First(), localPanelTab);
        }
    }

    /// <summary>
    /// <see cref="PanelsState.Reducer.ReduceDisposePanelTabAction(PanelsState, PanelsState.DisposePanelTabAction)"/>
    /// </summary>
    [Fact]
    public void ReduceDisposePanelTabAction()
    {
        InitializePanelsStateReducerTests(
            out var serviceProvider,
            out var panelsStateWrap,
            out var dispatcher,
            out var panelGroup,
            out var panelTab);

        Assert.Empty(panelGroup.TabBag);

        dispatcher.Dispatch(new PanelsState.RegisterPanelGroupAction(panelGroup));
        Assert.Contains(panelsStateWrap.Value.PanelGroupBag, x => x == panelGroup);

        dispatcher.Dispatch(new PanelsState.RegisterPanelTabAction(
                panelGroup.Key,
                panelTab,
                true));

        var localPanelGroup = panelsStateWrap.Value.PanelGroupBag.Single(x => x.Key == panelGroup.Key);
        Assert.Contains(localPanelGroup.TabBag, x => x == panelTab);
        Assert.NotEmpty(localPanelGroup.TabBag);

        dispatcher.Dispatch(new PanelsState.DisposePanelTabAction(
            panelGroup.Key,
            panelTab.Key));

        localPanelGroup = panelsStateWrap.Value.PanelGroupBag.Single(x => x.Key == panelGroup.Key);
        Assert.DoesNotContain(localPanelGroup.TabBag, x => x == panelTab);
        Assert.Empty(localPanelGroup.TabBag);
    }

    /// <summary>
    /// <see cref="PanelsState.Reducer.ReduceSetActivePanelTabAction(PanelsState, PanelsState.SetActivePanelTabAction)"/>
    /// </summary>
    [Fact]
    public void ReduceSetActivePanelTabAction()
    {
        InitializePanelsStateReducerTests(
            out var serviceProvider,
            out var panelsStateWrap,
            out var dispatcher,
            out var panelGroup,
            out var panelTab);

        Assert.Empty(panelGroup.TabBag);

        dispatcher.Dispatch(new PanelsState.RegisterPanelGroupAction(panelGroup));
        Assert.Contains(panelsStateWrap.Value.PanelGroupBag, x => x == panelGroup);

        dispatcher.Dispatch(new PanelsState.RegisterPanelTabAction(
                panelGroup.Key,
                panelTab,
                true));

        var localPanelGroup = panelsStateWrap.Value.PanelGroupBag.Single(x => x.Key == panelGroup.Key);
        Assert.Contains(localPanelGroup.TabBag, x => x == panelTab);
        Assert.NotEmpty(localPanelGroup.TabBag);

        Assert.NotEqual(panelTab.Key, localPanelGroup.ActiveTabKey);

        dispatcher.Dispatch(new PanelsState.SetActivePanelTabAction(
            panelGroup.Key,
            panelTab.Key));
        
        localPanelGroup = panelsStateWrap.Value.PanelGroupBag.Single(x => x.Key == panelGroup.Key);
        Assert.Equal(panelTab.Key, localPanelGroup.ActiveTabKey);
    }

    /// <summary>
    /// <see cref="PanelsState.Reducer.ReduceSetPanelTabAsActiveByContextRecordKeyAction(PanelsState, PanelsState.SetPanelTabAsActiveByContextRecordKeyAction)"/>
    /// </summary>
    [Fact]
    public void ReduceSetPanelTabAsActiveByContextRecordKeyAction()
    {
        InitializePanelsStateReducerTests(
            out var serviceProvider,
            out var panelsStateWrap,
            out var dispatcher,
            out var panelGroup,
            out var panelTab);

        Assert.Empty(panelGroup.TabBag);

        dispatcher.Dispatch(new PanelsState.RegisterPanelGroupAction(panelGroup));
        Assert.Contains(panelsStateWrap.Value.PanelGroupBag, x => x == panelGroup);

        List<(ContextRecord contextRecord, PanelTab panelTab)> panelTabTupleBag = new();

        foreach (var context in ContextFacts.AllContextsBag)
        {
            var localPanelTab = new PanelTab(
                Key<PanelTab>.NewKey(),
                panelGroup.ElementDimensions,
                new(),
                // Awkwardly need to provide a type here. Will provide an Icon but this usually
                // would be more along the lines of "typeof(SolutionExplorerDisplay)"
                typeof(IconCSharpClass),
                typeof(IconFolder),
                "Solution Explorer")
            {
                ContextRecordKey = context.ContextKey
            };

            panelTabTupleBag.Add((context, localPanelTab));

            dispatcher.Dispatch(new PanelsState.RegisterPanelTabAction(
                panelGroup.Key,
                localPanelTab,
                false));

            var localPanelGroup = panelsStateWrap.Value.PanelGroupBag.Single(x => x.Key == panelGroup.Key);
            Assert.Contains(localPanelGroup.TabBag, x => x == localPanelTab);
        }

        panelTabTupleBag.Reverse();

        foreach (var panelTabTuple in panelTabTupleBag)
        {
            dispatcher.Dispatch(new PanelsState.SetPanelTabAsActiveByContextRecordKeyAction(
                panelTabTuple.contextRecord.ContextKey));

            var localPanelGroup = panelsStateWrap.Value.PanelGroupBag.Single(x => x.Key == panelGroup.Key);
            Assert.Equal(panelTabTuple.panelTab.Key, localPanelGroup.ActiveTabKey);
        }
    }

    /// <summary>
    /// <see cref="PanelsState.Reducer.ReduceSetDragEventArgsAction(PanelsState, PanelsState.SetDragEventArgsAction)"/>
    /// </summary>
    [Fact]
    public void ReduceSetDragEventArgsAction()
    {
        InitializePanelsStateReducerTests(
            out var serviceProvider,
            out var panelsStateWrap,
            out var dispatcher,
            out var panelGroup,
            out var panelTab);

        Assert.Empty(panelGroup.TabBag);

        dispatcher.Dispatch(new PanelsState.RegisterPanelGroupAction(panelGroup));
        Assert.Contains(panelsStateWrap.Value.PanelGroupBag, x => x == panelGroup);

        dispatcher.Dispatch(new PanelsState.RegisterPanelTabAction(
            panelGroup.Key,
            panelTab,
            false));

        Assert.Null(panelsStateWrap.Value.DragEventArgs);

        var dragEventArgs = (panelTab, panelGroup);
        
        dispatcher.Dispatch(new PanelsState.SetDragEventArgsAction(dragEventArgs));
        Assert.Equal(panelsStateWrap.Value.DragEventArgs, dragEventArgs);
    }

    private void InitializePanelsStateReducerTests(
        out ServiceProvider serviceProvider,
        out IState<PanelsState> panelsStateWrap,
        out IDispatcher dispatcher,
        out PanelGroup samplePanelGroup,
        out PanelTab samplePanelTab)
    {
        var services = new ServiceCollection()
            .AddFluxor(options => options.ScanAssemblies(typeof(PanelsState).Assembly));

        serviceProvider = services.BuildServiceProvider();

        var store = serviceProvider.GetRequiredService<IStore>();
        store.InitializeAsync().Wait();

        panelsStateWrap = serviceProvider.GetRequiredService<IState<PanelsState>>();

        dispatcher = serviceProvider.GetRequiredService<IDispatcher>();

        samplePanelGroup = new PanelGroup(
                Key<PanelGroup>.NewKey(),
                Key<PanelTab>.Empty,
                new ElementDimensions(),
                ImmutableArray<PanelTab>.Empty);

        var samplePanelGroupWidth = samplePanelGroup.ElementDimensions.DimensionAttributeBag
            .Single(da => da.DimensionAttributeKind == DimensionAttributeKind.Width);

        samplePanelGroupWidth.DimensionUnitBag.AddRange(new[]
        {
            new DimensionUnit
            {
                Value = 33.3333,
                DimensionUnitKind = DimensionUnitKind.Percentage
            },
            new DimensionUnit
            {
                Value = ResizableColumn.RESIZE_HANDLE_WIDTH_IN_PIXELS / 2,
                DimensionUnitKind = DimensionUnitKind.Pixels,
                DimensionOperatorKind = DimensionOperatorKind.Subtract
            }
        });

        samplePanelTab = new PanelTab(
            Key<PanelTab>.NewKey(),
            samplePanelGroup.ElementDimensions,
            new(),
            // Awkwardly need to provide a type here. Will provide an Icon but this usually
            // would be more along the lines of "typeof(SolutionExplorerDisplay)"
            typeof(IconCSharpClass),
            typeof(IconFolder),
            "Solution Explorer")
        {
            ContextRecordKey = ContextFacts.SolutionExplorerContext.ContextKey
        };
    }
}