using UnityEngine;
using UnityEngine.InputSystem;

namespace ReactiveInputSystem
{
    internal static class ReactiveInputSystemInitializer
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        static void Init()
        {
            // register callbacks
            InputSystem.onBeforeUpdate += ((InputSystemFrameProvider)InputSystemFrameProvider.BeforeUpdate).OnUpdate;
            InputSystem.onAfterUpdate += ((InputSystemFrameProvider)InputSystemFrameProvider.AfterUpdate).OnUpdate;
        }
    }
}