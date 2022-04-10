using Godot;
using ThemedHorrorJam5.Scripts.GDUtils;

namespace ThemedHorrorJam5.Scripts.Extensions
{
    public static class Area2DExtensions
    {
        private const string BodyEnteredSignal = "body_entered";
        private const string BodyExitedSignal = "body_exited";

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

        /*
                
        InteractableArea.Disconnect(PlayerInteracting(Examinable examinable);
        InteractableArea.Disconnect(PlayerInteractingComplete(Examinable examinable);
        InteractableArea.Disconnect(PlayerInteractingUnavailable(Examinable examinable);
        InteractableArea.Disconnect(PlayerInteractingAvailable(Examinable examinable);
        */
    }
}