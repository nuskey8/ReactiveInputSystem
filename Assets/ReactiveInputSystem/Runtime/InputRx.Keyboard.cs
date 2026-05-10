using System.Threading;
using UnityEngine.InputSystem;
using R3;
using UnityEngine.InputSystem.LowLevel;

namespace ReactiveInputSystem
{
    public static partial class InputRx
    {
        public static Observable<char> OnTextInput(Keyboard keyboard, CancellationToken cancellationToken = default)
        {
            return Observable.FromEvent<char>(
                h => keyboard.onTextInput += h,
                h => keyboard.onTextInput -= h,
                cancellationToken
            );
        }

        public static Observable<char> OnTextInput(CancellationToken cancellationToken = default)
        {
            return Observable.FromEvent<char>(
                h => Keyboard.current.onTextInput += h,
                h => Keyboard.current.onTextInput -= h,
                cancellationToken
            );
        }

        public static Observable<IMECompositionString> OnIMECompositionChange(Keyboard keyboard, CancellationToken cancellationToken = default)
        {
            Error.ArgumentNullException(keyboard);

            return Observable.FromEvent<IMECompositionString>(
                h => keyboard.onIMECompositionChange += h,
                h => keyboard.onIMECompositionChange -= h,
                cancellationToken
            );
        }

        public static Observable<IMECompositionString> OnIMECompositionChange(CancellationToken cancellationToken = default)
        {
            return Observable.FromEvent<IMECompositionString>(
                h => Keyboard.current.onIMECompositionChange += h,
                h => Keyboard.current.onIMECompositionChange -= h,
                cancellationToken
            );
        }

        public static Observable<Key> OnAnyKeyDown(Keyboard keyboard, CancellationToken cancellationToken = default)
        {
            Error.ArgumentNullException(keyboard);
            return new AnyKeyDown(keyboard, cancellationToken);
        }

        public static Observable<Key> OnAnyKeyDown(CancellationToken cancellationToken = default)
        {
            return new AnyKeyDown(null, cancellationToken);
        }

        public static Observable<Key> OnAnyKey(Keyboard keyboard, CancellationToken cancellationToken = default)
        {
            Error.ArgumentNullException(keyboard);
            return new AnyKey(keyboard, cancellationToken);
        }

        public static Observable<Key> OnAnyKey(CancellationToken cancellationToken = default)
        {
            return new AnyKey(null, cancellationToken);
        }


        public static Observable<Key> OnAnyKeyUp(Keyboard keyboard, CancellationToken cancellationToken = default)
        {
            Error.ArgumentNullException(keyboard);
            return new AnyKeyUp(keyboard, cancellationToken);
        }

        public static Observable<Key> OnAnyKeyUp(CancellationToken cancellationToken = default)
        {
            return new AnyKeyUp(null, cancellationToken);
        }

        public static Observable<Unit> OnKeyDown(Keyboard keyboard, Key key, CancellationToken cancellationToken = default)
        {
            Error.ArgumentNullException(keyboard);

            return OnAfterUpdate(cancellationToken)
                .Where((keyboard, key), (_, state) => state.keyboard[state.key].wasPressedThisFrame);
        }

        public static Observable<Unit> OnKeyDown(Key key, CancellationToken cancellationToken = default)
        {
            return OnAfterUpdate(cancellationToken)
                .Where(key, (_, key) => Keyboard.current != null && Keyboard.current[key].wasPressedThisFrame);
        }

        public static Observable<Unit> OnKey(Keyboard keyboard, Key key, CancellationToken cancellationToken = default)
        {
            Error.ArgumentNullException(keyboard);

            return OnAfterUpdate(cancellationToken)
                .Where((keyboard, key), (_, state) => state.keyboard[state.key].isPressed);
        }

        public static Observable<Unit> OnKey(Key key, CancellationToken cancellationToken = default)
        {
            return OnAfterUpdate(cancellationToken)
                .Where(key, (_, key) => Keyboard.current != null && Keyboard.current[key].isPressed);
        }

        public static Observable<Unit> OnKeyUp(Keyboard keyboard, Key key, CancellationToken cancellationToken = default)
        {
            Error.ArgumentNullException(keyboard);
            
            return OnAfterUpdate(cancellationToken)
                .Where((keyboard, key), (_, state) => state.keyboard[state.key].wasReleasedThisFrame);
        }

        public static Observable<Unit> OnKeyUp(Key key, CancellationToken cancellationToken = default)
        {
            return OnAfterUpdate(cancellationToken)
                .Where(key, (_, key) => Keyboard.current != null && Keyboard.current[key].wasReleasedThisFrame);
        }
    }
}