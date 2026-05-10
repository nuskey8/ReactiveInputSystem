using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using R3;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace ReactiveInputSystem
{
    public static partial class InputRx
    {
        public static Observable<Unit> OnTouchDown(int touchId, CancellationToken cancellationToken = default)
        {
            return OnAfterUpdate(cancellationToken)
                .Where(touchId, (_, touchId) => Touchscreen.current != null &&
                    Touchscreen.current.touches.Count > touchId &&
                    Touchscreen.current.touches[touchId].press.wasPressedThisFrame
                );
        }

        public static Observable<Unit> OnTouchDown(Touchscreen touchscreen, int touchId, CancellationToken cancellationToken = default)
        {
            Error.ArgumentNullException(touchscreen);

            return OnAfterUpdate(cancellationToken)
                .Where((touchscreen, touchId), (_, state) =>
                    state.touchscreen.touches.Count > state.touchId &&
                    state.touchscreen.touches[state.touchId].press.wasPressedThisFrame
                );
        }

        public static Observable<Unit> OnTouch(int touchId, CancellationToken cancellationToken = default)
        {
            return OnAfterUpdate(cancellationToken)
                .Where(touchId, (_, touchId) => Touchscreen.current != null &&
                    Touchscreen.current.touches.Count > touchId &&
                    Touchscreen.current.touches[touchId].press.isPressed
                );
        }

        public static Observable<Unit> OnTouch(Touchscreen touchscreen, int touchId, CancellationToken cancellationToken = default)
        {
            Error.ArgumentNullException(touchscreen);

            return OnAfterUpdate(cancellationToken)
                .Where((touchscreen, touchId), (_, state) =>
                    state.touchscreen.touches.Count > state.touchId &&
                    state.touchscreen.touches[state.touchId].press.isPressed
                );
        }

        public static Observable<Unit> OnTouchUp(int touchId, CancellationToken cancellationToken = default)
        {
            return OnAfterUpdate(cancellationToken)
                .Where(touchId, (_, touchId) => Touchscreen.current != null &&
                    Touchscreen.current.touches.Count > touchId &&
                    Touchscreen.current.touches[touchId].press.wasReleasedThisFrame
                );
        }

        public static Observable<Unit> OnTouchUp(Touchscreen touchscreen, int touchId, CancellationToken cancellationToken = default)
        {
            Error.ArgumentNullException(touchscreen);

            return OnAfterUpdate(cancellationToken)
                .Where((touchscreen, touchId), (_, state) =>
                    state.touchscreen.touches.Count > state.touchId &&
                    state.touchscreen.touches[state.touchId].press.wasReleasedThisFrame
                );
        }

        public static Observable<TouchPhase> OnTouchPhaseChanged(int touchId, CancellationToken cancellationToken = default)
        {
            return Observable.EveryValueChanged<object, TouchPhase>(null,
                _ =>
                {
                    if (Touchscreen.current == null || Touchscreen.current.touches.Count <= touchId) return default;
                    return Touchscreen.current.touches[touchId].phase.value;
                },
                InputSystemFrameProvider.AfterUpdate,
                cancellationToken
            );
        }

        public static Observable<TouchPhase> OnTouchPhaseChanged(Touchscreen touchscreen, int touchId, CancellationToken cancellationToken = default)
        {
            Error.ArgumentNullException(touchscreen);

            return Observable.EveryValueChanged(touchscreen,
                touchscreen =>
                {
                    if (touchscreen.touches.Count <= touchId) return default;
                    return touchscreen.touches[touchId].phase.value;
                },
                InputSystemFrameProvider.AfterUpdate,
                cancellationToken
            );
        }

        public static Observable<Vector2> OnTouchPositionChanged(int touchId, CancellationToken cancellationToken = default)
        {
            return Observable.EveryValueChanged<object, Vector2>(null,
                _ =>
                {
                    if (Touchscreen.current == null || Touchscreen.current.touches.Count <= touchId) return default;
                    return Touchscreen.current.touches[touchId].position.value;
                },
                InputSystemFrameProvider.AfterUpdate,
                cancellationToken
            );
        }

        public static Observable<Vector2> OnTouchPositionChanged(Touchscreen touchscreen, int touchId, CancellationToken cancellationToken = default)
        {
            Error.ArgumentNullException(touchscreen);

            return Observable.EveryValueChanged(touchscreen,
                touchscreen =>
                {
                    if (touchscreen.touches.Count <= touchId) return default;
                    return touchscreen.touches[touchId].position.value;
                },
                InputSystemFrameProvider.AfterUpdate,
                cancellationToken
            );
        }

        public static Observable<Vector2> OnTouchDeltaChanged(int touchId, CancellationToken cancellationToken = default)
        {
            return Observable.EveryValueChanged<object, Vector2>(null,
                _ =>
                {
                    if (Touchscreen.current == null || Touchscreen.current.touches.Count <= touchId) return default;
                    return Touchscreen.current.touches[touchId].delta.value;
                },
                InputSystemFrameProvider.AfterUpdate,
                cancellationToken
            );
        }

        public static Observable<Vector2> OnTouchDeltaChanged(Touchscreen touchscreen, int touchId, CancellationToken cancellationToken = default)
        {
            Error.ArgumentNullException(touchscreen);
            
            return Observable.EveryValueChanged(touchscreen,
                touchscreen =>
                {
                    if (touchscreen.touches.Count <= touchId) return default;
                    return touchscreen.touches[touchId].delta.value;
                },
                InputSystemFrameProvider.AfterUpdate,
                cancellationToken
            );
        }
    }
}