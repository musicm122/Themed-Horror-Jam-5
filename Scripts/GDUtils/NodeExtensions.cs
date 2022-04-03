using Godot;

namespace ThemedHorrorJam5.Scripts.GDUtils
{
    public static class NodeExtensions
    {
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
    }

}