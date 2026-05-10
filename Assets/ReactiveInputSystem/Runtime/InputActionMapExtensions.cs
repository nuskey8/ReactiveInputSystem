using System.Threading;
using UnityEngine.InputSystem;
using R3;

namespace ReactiveInputSystem
{
    public static class InputActionMapExtensions
    {
        public static Observable<InputAction.CallbackContext> OnActionTriggered(this InputActionMap inputActions, CancellationToken cancellationToken = default)
        {
            Error.ArgumentNullException(inputActions);
            return Observable.FromEvent<InputAction.CallbackContext>(
                h => inputActions.actionTriggered += h,
                h => inputActions.actionTriggered -= h,
                cancellationToken
            );
        }
    }
}