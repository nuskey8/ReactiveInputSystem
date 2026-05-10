using System.Threading;
using UnityEngine.InputSystem;
using R3;

namespace ReactiveInputSystem
{
    public static class PlayerInputManagerExtensions
    {
        public static Observable<PlayerInput> OnPlayerJoined(this PlayerInputManager playerInputManager, CancellationToken cancellationToken = default)
        {
            return Observable.FromEvent<PlayerInput>(
                h => playerInputManager.onPlayerJoined += h,
                h => playerInputManager.onPlayerJoined -= h,
                cancellationToken
            );
        }

        public static Observable<PlayerInput> OnPlayerLeft(this PlayerInputManager playerInputManager, CancellationToken cancellationToken = default)
        {
            return Observable.FromEvent<PlayerInput>(
                h => playerInputManager.onPlayerLeft += h,
                h => playerInputManager.onPlayerLeft -= h,
                cancellationToken
            );
        }
    }
}