using System;
using System.Threading;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using R3;

namespace ReactiveInputSystem
{
    internal sealed class AnyKeyDown : AnyKeyObservableBase
    {
        public AnyKeyDown(Keyboard keyboard, CancellationToken cancellationToken) : base(keyboard, cancellationToken) { }

        protected override bool CheckKey(Keyboard keyboard, Key key)
        {
            return keyboard[key].wasPressedThisFrame;
        }
    }

    internal sealed class AnyKey : AnyKeyObservableBase
    {
        public AnyKey(Keyboard keyboard, CancellationToken cancellationToken) : base(keyboard, cancellationToken) { }

        protected override bool CheckKey(Keyboard keyboard, Key key)
        {
            return keyboard[key].isPressed;
        }
    }

    internal sealed class AnyKeyUp : AnyKeyObservableBase
    {
        public AnyKeyUp(Keyboard keyboard, CancellationToken cancellationToken) : base(keyboard, cancellationToken) { }

        protected override bool CheckKey(Keyboard keyboard, Key key)
        {
            return keyboard[key].wasReleasedThisFrame;
        }
    }

    internal abstract class AnyKeyObservableBase : Observable<Key>
    {
        public AnyKeyObservableBase(Keyboard keyboard, CancellationToken cancellationToken)
        {
            this.keyboard = keyboard;
            this.cancellationToken = cancellationToken;
        }

        readonly Keyboard keyboard;
        readonly CancellationToken cancellationToken;

        protected abstract bool CheckKey(Keyboard keyboard, Key key);

        protected override IDisposable SubscribeCore(Observer<Key> observer)
        {
            if (observer.IsDisposed)
            {
                return Disposable.Empty;
            }

            var runner = new FrameRunnerWorkItem(this, observer, keyboard, cancellationToken);
            InputSystemFrameProvider.AfterUpdate.Register(runner);
            return runner;
        }

        sealed class FrameRunnerWorkItem : CancellableFrameRunnerWorkItemBase<Key>
        {
            static readonly Key[] allKeys = (Key[])Enum.GetValues(typeof(Key));
            static readonly List<Key> buffer = new();

            readonly AnyKeyObservableBase source;
            readonly Keyboard keyboard;

            public FrameRunnerWorkItem(AnyKeyObservableBase source, Observer<Key> observer, Keyboard keyboard, CancellationToken cancellationToken) : base(observer, cancellationToken)
            {
                this.source = source;
                this.keyboard = keyboard;
            }

            protected override bool MoveNextCore(long _)
            {
                buffer.Clear();

                try
                {
                    var keyboard = this.keyboard ?? Keyboard.current;
                    if (keyboard == null) return true;

                    var keyCount = keyboard.allKeys.Count;

                    foreach (var key in allKeys)
                    {
                        var index = (int)key - 1;
                        if (index < 0 || index >= keyCount) continue;

                        if (source.CheckKey(keyboard, key))
                        {
                            buffer.Add(key);
                        }
                    }
                }
                catch (Exception ex)
                {
                    PublishOnCompleted(ex);
                    return false;
                }

                foreach (var key in buffer)
                {
                    PublishOnNext(key);
                }

                return true;
            }
        }
    }
}