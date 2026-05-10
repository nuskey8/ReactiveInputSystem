using System.Text;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace ReactiveInputSystem
{
    public static class InputControlPathEx
    {
        const string KeyboardLayout = "<Keyboard>";
        const string MouseLayout = "<Mouse>";
        const string GamepadLayout = "<Gamepad>";
        const string ButtonStr = "Button";

        static readonly StringBuilder sharedBuilder = new();

        public static string GetControlPath(Key key)
        {
            sharedBuilder.Clear();
            sharedBuilder.Append(KeyboardLayout)
                .Append('/')
                .Append(key.ToString());
            return sharedBuilder.ToString();
        }

        public static string GetControlPath(MouseButton mouseButton)
        {
            sharedBuilder.Clear();
            sharedBuilder.Append(MouseLayout)
                .Append('/')
                .Append(mouseButton.ToString())
                .Append(ButtonStr);
            return sharedBuilder.ToString();
        }

        public static string GetControlPath(GamepadButton gamepadButton)
        {
            static string EnumToString(GamepadButton gamepadButton)
            {
                return gamepadButton switch
                {
                    GamepadButton.West => GamepadButton.West.ToString(),
                    GamepadButton.North => GamepadButton.North.ToString(),
                    GamepadButton.South => GamepadButton.South.ToString(),
                    GamepadButton.East => GamepadButton.East.ToString(),
                    GamepadButton.DpadUp => "dpad/up",
                    GamepadButton.DpadDown => "dpad/down",
                    GamepadButton.DpadLeft => "dpad/left",
                    GamepadButton.DpadRight => "dpad/right",
                    _ => gamepadButton.ToString()
                };
            }

            sharedBuilder.Clear();
            sharedBuilder.Append(GamepadLayout)
                .Append('/')
                .Append(EnumToString(gamepadButton));
            return sharedBuilder.ToString();
        }
    }
}