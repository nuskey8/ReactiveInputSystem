# ReactiveInputSystem
 Reactive Extensions for Unity Input System

[![license](https://img.shields.io/badge/LICENSE-MIT-green.svg)](LICENSE)

[English README is here.](README.md)

## 概要

ReactiveInputSystemはInput Systemのイベントやデバイス入力をObservableに変換する機能を提供するライブラリです。

## セットアップ

### 要件

* Unity 2021.3 以上 (Unity 2022.2 以上を推奨)
* Input System 1.0.0 以上
* R3 0.1.0 以上

### インストール

1. Window > Package ManagerからPackage Managerを開く
2. 「+」ボタン > Add package from git URL
3. 以下のURLを入力する

```
https://github.com/nuskey8/ReactiveInputSystem.git?path=Assets/ReactiveInputSystem
```

あるいはPackages/manifest.jsonを開き、dependenciesブロックに以下を追記

```json
{
    "dependencies": {
        "com.nuskey8.reactive-input-system": "https://github.com/nuskey8/ReactiveInputSystem.git?path=Assets/ReactiveInputSystem"
    }
}
```

## 拡張メソッド

ReactiveInputSystemを導入することで、`InputAction`や`PlayerInput`, `PlayerInputManager`などのイベントをObservableに変換する拡張メソッドが追加されます。

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

## デバイス入力

`InputRx`クラスからあらゆるデバイスの入力をObservableとして取得することができます。

```cs
InputRx.OnKeyDown(Key.Space)
    .Subscribe(_ => ...);

InputRx.OnMousePositionChanged()
    .Subscribe(x => ...);

InputRx.OnGamepadButtonDown(GamepadButton.North, cancellationToken)
    .Subscribe(_ => ...);
```

また、`OnAny**`系のメソッドを使用することで、入力されたボタンの情報を取得できます。

```cs
InputRx.OnAnyKeyDown()
    .Subscribe(key => ...);

InputRx.OnAnyMouseButtonUp()
    .Subscribe(mouseButton => ...);

InputRx.OnAnyGamepadButton()
    .Subscribe(gamepadButton => ...);
```

## その他のイベント

`InputRx`では`InputSystem`クラスのイベントや`InputUser`のイベントをObservableに変換するメソッドも提供されています。

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

Control Path関連のユーティリティとして`InputControlPathEx`クラスが用意されています。

`InputControlPathEx.GetControlPath()`を用いることで`Key`や`MouseButton`、`GamepadButton`などの列挙型からControl Pathを取得することができます。これは`InputRx.OnAny**()`と組み合わせてインタラクティブなリバインディングを実装する際などに便利です。

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

## ライセンス

[MIT License](LICENSE)