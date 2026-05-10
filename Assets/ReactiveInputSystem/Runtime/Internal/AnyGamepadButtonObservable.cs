using System;
using System.Threading;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using R3;

namespace ReactiveInputSystem
{
    internal sealed class AnyGamepadButtonDown : AnyGamepadButtonObservableBase
    {
        public AnyGamepadButtonDown(Gamepad gamepad, CancellationToken cancellationToken) : base(gamepad, cancellationToken) { }

        protected override bool CheckGamepad(Gamepad gamepad, GamepadButton gamepadButton)
        {
            return gamepad[gamepadButton].wasPressedThisFrame;
        }
    }

    internal sealed class AnyGamepadButton : AnyGamepadButtonObservableBase
    {
        public AnyGamepadButton(Gamepad gamepad, CancellationToken cancellationToken) : base(gamepad, cancellationToken) { }

        protected override bool CheckGamepad(Gamepad gamepad, GamepadButton gamepadButton)
        {
            return gamepad[gamepadButton].isPressed;
        }
    }

    internal sealed class AnyGamepadButtonUp : AnyGamepadButtonObservableBase
    {
        public AnyGamepadButtonUp(Gamepad gamepad, CancellationToken cancellationToken) : base(gamepad, cancellationToken) { }

        protected override bool CheckGamepad(Gamepad gamepad, GamepadButton gamepadButton)
        {
            return gamepad[gamepadButton].wasReleasedThisFrame;
        }
    }

    internal abstract class AnyGamepadButtonObservableBase : Observable<GamepadButton>
    {
        public AnyGamepadButtonObservableBase(Gamepad gamepad, CancellationToken cancellationToken)
        {
            this.gamepad = gamepad;
            this.cancellationToken = cancellationToken;
        }

        readonly Gamepad gamepad;
        readonly CancellationToken cancellationToken;

        protected abstract bool CheckGamepad(Gamepad gamepad, GamepadButton gamepadButton);

        protected override IDisposable SubscribeCore(Observer<GamepadButton> observer)
        {
            if (observer.IsDisposed)
            {
                return Disposable.Empty;
            }

            var runner = new FrameRunnerWorkItem(this, observer, gamepad, cancellationToken);
            InputSystemFrameProvider.AfterUpdate.Register(runner);
            return runner;
        }

        sealed class FrameRunnerWorkItem : CancellableFrameRunnerWorkItemBase<GamepadButton>
        {
            static readonly GamepadButton[] allGamepadButtons = (GamepadButton[])Enum.GetValues(typeof(GamepadButton));
            static readonly List<GamepadButton> buffer = new();

            readonly AnyGamepadButtonObservableBase source;
            readonly Gamepad gamepad;

            public FrameRunnerWorkItem(AnyGamepadButtonObservableBase source, Observer<GamepadButton> observer, Gamepad gamepad, CancellationToken cancellationToken) : base(observer, cancellationToken)
            {
                this.source = source;
                this.gamepad = gamepad;
            }

            protected override bool MoveNextCore(long _)
            {
                buffer.Clear();

                try
                {
                    var gamepad = this.gamepad ?? Gamepad.current;
                    if (gamepad == null) return true;

                    foreach (var button in allGamepadButtons)
                    {
                        if (source.CheckGamepad(gamepad, button))
                        {
                            buffer.Add(button);
                        }
                    }
                }
                catch (Exception ex)
                {
                    PublishOnCompleted(ex);
                    return false;
                }

                foreach (var button in buffer)
                {
                    PublishOnNext(button);
                }

                return true;
            }
        }
    }
}