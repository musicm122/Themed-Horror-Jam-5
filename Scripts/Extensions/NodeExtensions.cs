using Godot;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ThemedHorrorJam5.Scripts.GDUtils
{
    public static class NodeExtensions
    {
        public static bool IsPlayer(this Node node) => node.Name.ToLower() == "player";

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