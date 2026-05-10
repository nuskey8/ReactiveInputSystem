using System;
using System.Threading;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using R3;

namespace ReactiveInputSystem
{
    public static partial class InputRx
    {
        public static Observable<InputControl> OnAnyButtonPress()
        {
            return InputSystem.onAnyButtonPress.ToObservable();
        }

        public static Observable<InputEventPtr> OnEvent()
        {
            return InputSystem.onEvent.ToObservable();
        }

        public static Observable<Unit> OnBeforeUpdate(CancellationToken cancellationToken = default)
        {
            return Observable.EveryUpdate(InputSystemFrameProvider.BeforeUpdate, cancellationToken);
        }

        public static Observable<Unit> OnAfterUpdate(CancellationToken cancellationToken = default)
        {
            return Observable.EveryUpdate(InputSystemFrameProvider.AfterUpdate, cancellationToken);
        }

        public static Observable<(object, InputActionChange)> OnActionChange(CancellationToken cancellationToken = default)
        {
            return Observable.FromEvent<Action<object, InputActionChange>, (object, InputActionChange)>(
                h => (x, y) => h((x, y)),
                h => InputSystem.onActionChange += h,
                h => InputSystem.onActionChange -= h,
                cancellationToken
            );
        }

        public static Observable<(InputDevice, InputDeviceChange)> OnDeviceChange(CancellationToken cancellationToken = default)
        {
            return Observable.FromEvent<Action<InputDevice, InputDeviceChange>, (InputDevice, InputDeviceChange)>(
                h => (x, y) => h((x, y)),
                h => InputSystem.onDeviceChange += h,
                h => InputSystem.onDeviceChange -= h,
                cancellationToken
            );
        }
    }
}