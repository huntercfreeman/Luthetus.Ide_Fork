﻿using Luthetus.Common.RazorLib.Keys.Models;
using Luthetus.Common.RazorLib.Tabs.Models;
using Luthetus.Common.RazorLib.Tabs.States;
using System.Collections.Immutable;
using Fluxor;
using Microsoft.Extensions.DependencyInjection;

namespace Luthetus.Common.Tests.Basis.Tabs.States;

/// <summary>
/// <see cref="TabState.Reducer"/>
/// </summary>
public class TabStateReducerTests
{
    /// <summary>
    /// <see cref="TabState.Reducer.ReduceRegisterTabGroupAction(TabState, TabState.RegisterTabGroupAction)"/>
    /// </summary>
    [Fact]
    public void ReduceRegisterTabGroupAction()
    {
        InitializeTabStateReducerTests(
            out var dispatcher,
            out var tabStateWrap,
            out var tabGroup,
            out _,
            out _,
            out _,
            out _);

        var registerTabGroupAction = new TabState.RegisterTabGroupAction(tabGroup);

        Assert.DoesNotContain(
            tabStateWrap.Value.TabGroupList,
            x => x.Key == tabGroup.Key);

        dispatcher.Dispatch(registerTabGroupAction);

        Assert.Contains(
            tabStateWrap.Value.TabGroupList,
            x => x.Key == tabGroup.Key);
    }

    /// <summary>
    /// <see cref="TabState.Reducer.ReduceDisposeTabGroupAction(TabState, TabState.DisposeTabGroupAction)"/>
    /// </summary>
    [Fact]
    public void ReduceDisposeTabGroupAction()
    {
        InitializeTabStateReducerTests(
            out var dispatcher,
            out var tabStateWrap,
            out var tabGroup,
            out _,
            out _,
            out _,
            out _);

        var registerTabGroupAction = new TabState.RegisterTabGroupAction(tabGroup);
        dispatcher.Dispatch(registerTabGroupAction);

        Assert.Contains(
            tabStateWrap.Value.TabGroupList,
            x => x.Key == tabGroup.Key);

        var disposeTabGroupAction = new TabState.DisposeTabGroupAction(tabGroup.Key);
        dispatcher.Dispatch(disposeTabGroupAction);

        Assert.DoesNotContain(
            tabStateWrap.Value.TabGroupList,
            x => x.Key == tabGroup.Key);
    }

    /// <summary>
    /// <see cref="TabState.Reducer.ReduceSetTabEntryListAction(TabState, TabState.SetTabEntryListAction)"/>
    /// </summary>
    [Fact]
    public void ReduceSetTabEntryListAction()
    {
        InitializeTabStateReducerTests(
            out var dispatcher,
            out var tabStateWrap,
            out var tabGroup,
            out _,
            out _,
            out _,
            out var tabEntries);

        var registerTabGroupAction = new TabState.RegisterTabGroupAction(tabGroup);
        dispatcher.Dispatch(registerTabGroupAction);

        Assert.Contains(tabStateWrap.Value.TabGroupList, x => x.Key == tabGroup.Key);

        dispatcher.Dispatch(new TabState.SetTabEntryListAction(
            tabGroup.Key,
            tabEntries));

        tabGroup = tabStateWrap.Value.TabGroupList.Single(x => x.Key == tabGroup.Key);

        var emptyTabEntries = ImmutableList<TabEntryNoType>.Empty;
        Assert.NotEqual(emptyTabEntries, tabGroup.EntryList);

        dispatcher.Dispatch(new TabState.SetTabEntryListAction(
            tabGroup.Key,
            emptyTabEntries));

        tabGroup = tabStateWrap.Value.TabGroupList.Single(x => x.Key == tabGroup.Key);
        Assert.Equal(emptyTabEntries, tabGroup.EntryList);
    }

    /// <summary>
    /// <see cref="TabState.Reducer.ReduceSetActiveTabEntryKeyAction(TabState, TabState.SetActiveTabEntryKeyAction)"/>
    /// </summary>
    [Fact]
    public void ReduceSetActiveTabEntryKeyAction()
    {
        InitializeTabStateReducerTests(
            out var dispatcher,
            out var tabStateWrap,
            out var tabGroup,
            out _,
            out _,
            out var blueTabEntry,
            out var tabEntries);

        var registerTabGroupAction = new TabState.RegisterTabGroupAction(tabGroup);
        dispatcher.Dispatch(registerTabGroupAction);

        Assert.Contains(tabStateWrap.Value.TabGroupList, x => x.Key == tabGroup.Key);

        dispatcher.Dispatch(new TabState.SetTabEntryListAction(
            tabGroup.Key,
            tabEntries));

        tabGroup = tabStateWrap.Value.TabGroupList.Single(x => x.Key == tabGroup.Key);

        var emptyTabEntries = ImmutableList<TabEntryNoType>.Empty;
        Assert.NotEqual(emptyTabEntries, tabGroup.EntryList);

        dispatcher.Dispatch(new TabState.SetActiveTabEntryKeyAction(
            tabGroup.Key,
            blueTabEntry.TabEntryKey));

        tabGroup = tabStateWrap.Value.TabGroupList.Single(x => x.Key == tabGroup.Key);
        Assert.Equal(blueTabEntry.TabEntryKey, tabGroup.ActiveEntryKey);
    }

    private void InitializeTabStateReducerTests(
        out IDispatcher dispatcher,
        out IState<TabState> tabStateWrap,
        out TabGroup sampleTabGroup,
        out TabEntryWithType<ColorKindTest> redTabEntry,
        out TabEntryWithType<ColorKindTest> greenTabEntry,
        out TabEntryWithType<ColorKindTest> blueTabEntry,
        out ImmutableList<TabEntryNoType> tabEntries)
    {
        var services = new ServiceCollection()
            .AddFluxor(options => options.ScanAssemblies(typeof(TabState).Assembly));

        var serviceProvider = services.BuildServiceProvider();

        var store = serviceProvider.GetRequiredService<IStore>();
        store.InitializeAsync().Wait();

        tabStateWrap = serviceProvider.GetRequiredService<IState<TabState>>();

        dispatcher = serviceProvider.GetRequiredService<IDispatcher>();

        redTabEntry = new TabEntryWithType<ColorKindTest>(
            ColorKindTest.Red,
            tabEntry => ((TabEntryWithType<ColorKindTest>)tabEntry).Item.ToString(),
            _ => { });

        greenTabEntry = new TabEntryWithType<ColorKindTest>(
            ColorKindTest.Green,
            tabEntry => ((TabEntryWithType<ColorKindTest>)tabEntry).Item.ToString(),
            _ => { });

        blueTabEntry = new TabEntryWithType<ColorKindTest>(
            ColorKindTest.Blue,
            tabEntry => ((TabEntryWithType<ColorKindTest>)tabEntry).Item.ToString(),
            _ => { });

        var temporaryTabEntries = tabEntries = new TabEntryNoType[]
        {
            redTabEntry,
            greenTabEntry,
            blueTabEntry,
        }.ToImmutableList();

        sampleTabGroup = new TabGroup(
            loadTabEntriesArgs => Task.FromResult(new TabGroupLoadTabEntriesOutput(temporaryTabEntries)),
            Key<TabGroup>.NewKey());
    }
}
