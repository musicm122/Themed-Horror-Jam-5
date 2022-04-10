using Godot;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace ThemedHorrorJam5.Scripts.GDUtils
{
    public static class NodeExtensions
    {
        public static bool TryConnectSignal(this Node node, string signal, Godot.Object target, string methodName)
        {
            Error result;
            try
            {
                result = node.Connect(signal, target, methodName);
                if (result != Error.Ok)
                {
                    GD.PrintErr(MethodBase.GetCurrentMethod().Name, new ApplicationException(result.ToString()), $"ConnectBodyEntered failed with {result}");
                    GD.PrintStack();
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr(ex);
                throw;
            }
            return result == Error.Ok;
        }

        public static bool TryDisconnectSignal(this Node node, string signal, Godot.Object target, string methodName){
            try
            {
                node.Disconnect(signal, target, methodName);
                return true;
            }
            catch (Exception ex)
            {
                GD.PrintErr(ex);
                return false;
            }
        }

        public static bool IsPlayer(this Node node)
        {
            var result = StringExtensions.Equals(node.Name, "player");
            return result;
        }

        public static void Pause(this Node node)
        {
            node.GetTree().Paused = true;
        }

        public static bool IsPaused(this Node node) => node.GetTree().Paused;

        public static void Unpause(this Node node)
        {
            node.GetTree().Paused = false;
        }

        public static void TogglePause(this Node node)
        {
            node.GetTree().Paused = (!node.GetTree().Paused);
        }

        public static T GetNode<T>(this Node node, string path) where T : Node
        {
            return (T)node.GetNode(path);
        }

        public static async Task WaitForSeconds(this Node node, float seconds)
        {
            try
            {
                await node.ToSignal(node.GetTree().CreateTimer(seconds), "timeout");
            }
            catch (System.Exception ex)
            {
                GD.PrintErr("WaitForSeconds threw", ex);
                throw;
            }
        }
    }
}