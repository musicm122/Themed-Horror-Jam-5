using Godot;
using ThemedHorrorJam5.Scripts.GDUtils;

namespace ThemedHorrorJam5.Scripts.Extensions
{
    public static class Area2DExtensions
    {
        public const string BodyEnteredSignal = "body_entered";
        public const string BodyExitedSignal = "body_exited";

        public static bool ConnectBodyEntered(this Area2D area2D, Object target, string methodName) =>
            area2D.TryConnectSignal(BodyEnteredSignal, target, methodName);

        public static bool ConnectBodyExited(this Area2D area2D, Object target, string methodName) =>
            area2D.TryConnectSignal(BodyExitedSignal, target, methodName);

        public static void DisconnectBodyEntered(this Area2D area2D, string methodName)
        {
            area2D.TryDisconnectSignal(BodyEnteredSignal, area2D, methodName);
        }

        public static void DisconnectBodyExited(this Area2D area2D, string methodName)
        {
            area2D.TryDisconnectSignal(BodyExitedSignal, area2D, methodName);
        }

    }
}