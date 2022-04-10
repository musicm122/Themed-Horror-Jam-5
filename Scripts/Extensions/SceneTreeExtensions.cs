using Godot;
using System.Collections.Generic;
using ThemedHorrorJam5.Scripts.ItemComponents;

namespace ThemedHorrorJam5.Scripts.GDUtils
{
    public static class SceneTreeExtensions
    {
        public static void AddItem(this SceneTree tree, string name, int amt = 1) => tree.CallGroup(Groups.Player, "AddItem", name, amt);

        public static List<Examinable> GetExaminableCollection(this SceneTree tree) => tree.GetNodesByType<Examinable>();

        public static List<LockedDoor> GetLockedDoorCollection(this SceneTree tree) => tree.GetNodesByType<LockedDoor>();

        public static List<T> GetNodesByType<T>(this SceneTree tree)
        {
            var retval = new List<T>();
            foreach (var child in tree.CurrentScene.GetChildren())
            {
                if (child is T t)
                {
                    retval.Add(t);
                }
            }
            return retval;
        }

        public static bool HasPlayerNode(this SceneTree tree) =>
            //get_tree().get_current_scene()
            tree.CurrentScene.FindNode("Player") != null;

        public static (bool, Player) GetPlayerNode(this SceneTree tree) =>
            tree.HasPlayerNode() ? (true, tree.CurrentScene.FindNode("Player") as Player) : (false, null);
    }
}