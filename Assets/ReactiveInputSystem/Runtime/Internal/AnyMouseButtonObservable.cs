using System;
using System.Threading;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using R3;

namespace ReactiveInputSystem
{
    internal sealed class AnyMouseButtonDown : AnyMouseButtonObservableBase
    {
        public AnyMouseButtonDown(Mouse mouse, CancellationToken cancellationToken) : base(mouse, cancellationToken) { }

        protected override bool CheckMouse(Mouse mouse, MouseButton mouseButton)
        {
            return InputControlHelper.GetMouseButtonControl(mouse, mouseButton).wasPressedThisFrame;
        }
    }

    internal sealed class AnyMouseButton : AnyMouseButtonObservableBase
    {
        public AnyMouseButton(Mouse mouse, CancellationToken cancellationToken) : base(mouse, cancellationToken) { }

        protected override bool CheckMouse(Mouse mouse, MouseButton mouseButton)
        {
            return InputControlHelper.GetMouseButtonControl(mouse, mouseButton).isPressed;
        }
    }

    internal sealed class AnyMouseButtonUp : AnyMouseButtonObservableBase
    {
        public AnyMouseButtonUp(Mouse mouse, CancellationToken cancellationToken) : base(mouse, cancellationToken) { }

        protected override bool CheckMouse(Mouse mouse, MouseButton mouseButton)
        {
            return InputControlHelper.GetMouseButtonControl(mouse, mouseButton).wasReleasedThisFrame;
        }
    }

    internal abstract class AnyMouseButtonObservableBase : Observable<MouseButton>
    {
        public AnyMouseButtonObservableBase(Mouse mouse, CancellationToken cancellationToken)
        {
            this.mouse = mouse;
            this.cancellationToken = cancellationToken;
        }

        readonly Mouse mouse;
        readonly CancellationToken cancellationToken;

        protected abstract bool CheckMouse(Mouse mouse, MouseButton mouseButton);

        protected override IDisposable SubscribeCore(Observer<MouseButton> observer)
        {
            if (observer.IsDisposed)
            {
                return Disposable.Empty;
            }

            var runner = new FrameRunnerWorkItem(this, observer, mouse, cancellationToken);
            InputSystemFrameProvider.AfterUpdate.Register(runner);
            return runner;
        }

        sealed class FrameRunnerWorkItem : CancellableFrameRunnerWorkItemBase<MouseButton>
        {
            static readonly MouseButton[] allMouseButtons = (MouseButton[])Enum.GetValues(typeof(MouseButton));
            static readonly List<MouseButton> buffer = new();

            readonly AnyMouseButtonObservableBase source;
            readonly Mouse mouse;

            public FrameRunnerWorkItem(AnyMouseButtonObservableBase source, Observer<MouseButton> observer, Mouse mouse, CancellationToken cancellationToken) : base(observer, cancellationToken)
            {
                this.source = source;
                this.mouse = mouse;
            }

            protected override bool MoveNextCore(long _)
            {
                buffer.Clear();

                try
                {
                    var mouse = this.mouse ?? Mouse.current;
                    if (mouse == null) return true;

                    foreach (var button in allMouseButtons)
                    {
                        if (source.CheckMouse(mouse, button))
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