﻿using Fluxor;
using Luthetus.Common.RazorLib.Dialogs.Models;
using Luthetus.Common.RazorLib.Dialogs.States;
using Luthetus.Common.RazorLib.Keys.Models;
using Luthetus.Common.RazorLib.Notifications.Displays;
using Microsoft.Extensions.DependencyInjection;

namespace Luthetus.Common.Tests.Basis.Dialogs.States;

/// <summary>
/// <see cref="DialogState.Reducer"/>
/// </summary>
public class DialogStateReducerTests
{
    /// <summary>
    /// <see cref="DialogState.Reducer.ReduceRegisterAction(DialogState, DialogState.RegisterAction)"/>
    /// </summary>
    [Fact]
    public void ReduceRegisterAction()
    {
        InitializeDialogStateReducerTests(
            out var _, out var dialogStateWrap, out var dispatcher, out var dialogRecord);

        Assert.Empty(dialogStateWrap.Value.DialogList);

        dispatcher.Dispatch(new DialogState.RegisterAction(dialogRecord));

        Assert.NotEmpty(dialogStateWrap.Value.DialogList);
        Assert.Contains(dialogStateWrap.Value.DialogList, x => x == dialogRecord);
    }

    /// <summary>
    /// <see cref="DialogState.Reducer.ReduceSetIsMaximizedAction(DialogState, DialogState.SetIsMaximizedAction)"/>
    /// </summary>
    [Fact]
    public void ReduceSetIsMaximizedAction()
    {
        InitializeDialogStateReducerTests(
            out var _, out var dialogStateWrap, out var dispatcher, out var dialogRecord);

        Assert.Empty(dialogStateWrap.Value.DialogList);

        dispatcher.Dispatch(new DialogState.RegisterAction(dialogRecord));

        Assert.NotEmpty(dialogStateWrap.Value.DialogList);
        Assert.Single(dialogStateWrap.Value.DialogList);

        Assert.False(dialogRecord.IsMaximized);
        dispatcher.Dispatch(new DialogState.SetIsMaximizedAction(dialogRecord.Key, true));

        dialogRecord = dialogStateWrap.Value.DialogList.Single();
        Assert.True(dialogRecord.IsMaximized);
    }

    /// <summary>
    /// <see cref="DialogState.Reducer.ReduceDisposeAction(DialogState, DialogState.DisposeAction)"/>
    /// </summary>
    [Fact]
    public void ReduceDisposeAction()
    {
        InitializeDialogStateReducerTests(
            out var _, out var dialogStateWrap, out var dispatcher, out var dialogRecord);

        Assert.Empty(dialogStateWrap.Value.DialogList);

        dispatcher.Dispatch(new DialogState.RegisterAction(dialogRecord));

        Assert.NotEmpty(dialogStateWrap.Value.DialogList);
        Assert.Contains(dialogStateWrap.Value.DialogList, x => x == dialogRecord);

        dispatcher.Dispatch(new DialogState.DisposeAction(dialogRecord.Key));

        Assert.Empty(dialogStateWrap.Value.DialogList);
    }

    private void InitializeDialogStateReducerTests(
        out ServiceProvider serviceProvider,
        out IState<DialogState> dialogStateWrap,
        out IDispatcher dispatcher,
        out DialogRecord sampleDialogRecord)
    {
        var services = new ServiceCollection()
            .AddScoped<IDialogService, DialogService>()
            .AddFluxor(options => options.ScanAssemblies(typeof(IDialogService).Assembly));

        serviceProvider = services.BuildServiceProvider();

        var store = serviceProvider.GetRequiredService<IStore>();
        store.InitializeAsync().Wait();

        dialogStateWrap = serviceProvider.GetRequiredService<IState<DialogState>>();

        dispatcher = serviceProvider.GetRequiredService<IDispatcher>();

        sampleDialogRecord = new DialogRecord(Key<DialogRecord>.NewKey(), "Test title",
            typeof(CommonInformativeNotificationDisplay),
            new Dictionary<string, object?>
            {
                {
                    nameof(CommonInformativeNotificationDisplay.Message),
                    "Test message"
                }
            },
            null);
    }
}