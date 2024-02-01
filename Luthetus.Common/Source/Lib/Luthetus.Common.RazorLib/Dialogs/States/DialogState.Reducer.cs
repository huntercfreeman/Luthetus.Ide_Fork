﻿using Fluxor;

namespace Luthetus.Common.RazorLib.Dialogs.States;

public partial record DialogState
{
    public class Reducer
    {
        [ReducerMethod]
        public static DialogState ReduceRegisterAction(
            DialogState inState,
            RegisterAction registerAction)
        {
            if (inState.DialogList.Any(x => x.Key == registerAction.Entry.Key))
                return inState;

            var outDialogList = inState.DialogList.Add(registerAction.Entry);

            return inState with 
            {
                DialogList = outDialogList,
                ActiveDialogKey = registerAction.Entry.Key,
            };
        }

        [ReducerMethod]
        public static DialogState ReduceSetIsMaximizedAction(
            DialogState inState,
            SetIsMaximizedAction setIsMaximizedAction)
        {
            var inDialog = inState.DialogList.FirstOrDefault(
                x => x.Key == setIsMaximizedAction.Key);

            if (inDialog is null)
                return inState;

            var outDialogList = inState.DialogList.Replace(inDialog, inDialog with
            {
                IsMaximized = setIsMaximizedAction.IsMaximized
            });

            return inState with { DialogList = outDialogList };
        }
        
        [ReducerMethod]
        public static DialogState ReduceSetActiveDialogKeyAction(
            DialogState inState,
            SetActiveDialogKeyAction setActiveDialogKeyAction)
        {
            return inState with { ActiveDialogKey = setActiveDialogKeyAction.DialogKey };
        }

        [ReducerMethod]
        public static DialogState ReduceDisposeAction(
            DialogState inState,
            DisposeAction disposeAction)
        {
            var inDialog = inState.DialogList.FirstOrDefault(
                x => x.Key == disposeAction.Key);

            if (inDialog is null)
                return inState;

            var outDialogList = inState.DialogList.Remove(inDialog);

            return inState with { DialogList = outDialogList };
        }
    }
}