# ReactiveInputSystem
 Reactive Extensions for Unity Input System

[![license](https://img.shields.io/badge/LICENSE-MIT-green.svg)](LICENSE)

[日本語版READMEはこちら](README_JA.md)

## Overview

ReactiveInputSystem is a library that provides functionality to convert events and device inputs from the Input System into Observables.

## Setup

### Requirements

* Unity 2021.3 or later (Unity 2022.2 or later recommended)
* Input System 1.0.0 or later
* R3 0.1.0 or later

### Installation

1. Open Package Manager from Window > Package Manager.
2. Click the "+" button > Add package from git URL.
3. Enter the following URL:

```plaintext
https://github.com/nuskey8/ReactiveInputSystem.git?path=Assets/ReactiveInputSystem
```

Alternatively, open Packages/manifest.json and add the following to the dependencies block:

```json
{
    "dependencies": {
        "com.nuskey8.reactive-input-system": "https://github.com/nuskey8/ReactiveInputSystem.git?path=Assets/ReactiveInputSystem"
    }
}
```

## Extension Methods

By introducing ReactiveInputSystem, extension methods are added to convert events from `InputAction`, `PlayerInput`, `PlayerInputManager`, and others into Observables.

```cs
using UnityEngine.InputSystem;
using R3;
using ReactiveInputSystem;

InputAction inputAction;

inputAction.StartedAsObservable(cancellationToken)
    .Subscribe(x => ...);
inputAction.PerformedAsObservable(cancellationToken)
    .Subscribe(x => ...);
inputAction.CanceledAsObservable(cancellationToken)
    .Subscribe(x => ...);

PlayerInput playerInput;

playerInput.OnActionTriggeredAsObservable(cancellationToken)
    .Subscribe(x => ...)
```

## Device Input

You can obtain input from any device as an Observable using the `InputRx` class.

```cs
InputRx.OnKeyDown(Key.Space)
    .Subscribe(_ => ...);

InputRx.OnMousePositionChanged()
    .Subscribe(x => ...);

InputRx.OnGamepadButtonDown(GamepadButton.North, cancellationToken)
    .Subscribe(_ => ...);
```

Additionally, using the `OnAny**` methods allows you to retrieve information about the pressed button.

```cs
InputRx.OnAnyKeyDown()
    .Subscribe(key => ...);

InputRx.OnAnyMouseButtonUp()
    .Subscribe(mouseButton => ...);

InputRx.OnAnyGamepadButton()
    .Subscribe(gamepadButton => ...);
```

## Other Events

`InputRx` provides methods to convert events from the `InputSystem` class and events from `InputUser` into Observables.

```cs
// InputSystem.onAfterUpdate
InputRx.OnAfterUpdate()
    .Subscribe(_ => ...);

// InputSystem.onAnyButtonPress
InputRx.OnAnyButtonPress()
    .Subscribe(control => ...);

// InputUser.onChange
InputRx.OnUserChange()
    .Subscribe(x => ...);
```

## InputControlPathEx

As a utility for Control Path-related operations, the `InputControlPathEx` class is provided.

You can use `InputControlPathEx.GetControlPath()` to obtain a Control Path from enumerations such as `Key`, `MouseButton`, `GamepadButton`, etc. This can be useful when implementing interactive rebinding, for example, with `InputRx.OnAny**()`.

```cs
InputAction inputAction;

async Task RebindAsync(CancellationToken cancellationToken)
{
    inputAction.Disable();

    var path = await InputRx.OnAnyKeyDown(cancellationToken)
        .Select(x => InputControlPathEx.GetControlPath(x))
        .FirstAsync();

    inputAction.ApplyBindingOverride(0, path);
    inputAction.Enable();
}
```

## License

[MIT License](LICENSE)