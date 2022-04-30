using Godot;
using ThemedHorrorJam5.Scripts.GDUtils;

namespace ThemedHorrorJam5.Scripts.Extensions
{
    public static class ButtonExtensions
    {
        public const string PressedSignal = "pressed";
        public const string ButtonDownSignal = "button_down";
        public const string ButtonUpSignal = "button_up";
        public const string ToggledSignal = "toggled";

        public static bool ConnectButtonPressed(this Button btn, Object target, string methodName) =>
            btn.TryConnectSignal(PressedSignal, target, methodName);

        public static bool ConnectButtonDown(this Button btn, Object target, string methodName) =>
            btn.TryConnectSignal(ButtonDownSignal, target, methodName);

        public static bool ConnectButtonUp(this Button btn, Object target, string methodName) =>
            btn.TryConnectSignal(ButtonUpSignal, target, methodName);

        public static bool ConnectButtonToggled(this Button btn, Object target, string methodName) =>
            btn.TryConnectSignal(ToggledSignal, target, methodName);

        public static void DisconnectButtonPressed(this Button btn, string methodName)
        {
            btn.TryDisconnectSignal(PressedSignal, btn, methodName);
        }
        public static void DisconnectButtonDown(this Button btn, string methodName)
        {
            btn.TryDisconnectSignal(ButtonDownSignal, btn, methodName);
        }
        public static void DisconnectButtonUp(this Button btn, string methodName)
        {
            btn.TryDisconnectSignal(ButtonUpSignal, btn, methodName);
        }
        public static void DisconnectButtonToggled(this Button btn, string methodName)
        {
            btn.TryDisconnectSignal(ToggledSignal, btn, methodName);
        }
    }
}