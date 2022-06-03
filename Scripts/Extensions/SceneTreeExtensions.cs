using Godot;
using System.Collections.Generic;
using System.Linq;
using ThemedHorrorJam5.Entities;
using ThemedHorrorJam5.Scripts.ItemComponents;

namespace ThemedHorrorJam5.Scripts.GDUtils
{
    public static class SceneTreeExtensions
    {
        public static void AddItem(this SceneTree tree, string name, int amt = 1) {
            var playerTuple = tree.GetPlayerNode();
            if(!playerTuple.Item1)
            {
                GD.PushWarning("Player node not found");
                return;
            }
            playerTuple.Item2.AddItem(name, amt);
        }

        public static void RemoveItem(this SceneTree tree, string name, int amt = 1) {
            tree.CallGroup(Groups.Player, "RemoveItem", name, amt);
            var playerTuple = tree.GetPlayerNode();
            if (!playerTuple.Item1)
            {
                GD.PushWarning("Player node not found");
                return;
            }
            playerTuple.Item2.RemoveItems(name, amt);
        }

        public static void AddMission(this SceneTree tree, string title) =>
            tree.CallGroup(Groups.Player, "AddMission", title);

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
            tree.CurrentScene.FindNode("Player") != null;

        public static (bool, PlayerV2) GetPlayerNode(this SceneTree tree) =>
            tree.HasPlayerNode() ? (true, tree.CurrentScene.FindNode("Player") as PlayerV2) : (false, null);


        public static (bool, List<Navigation2D>?) GetNavigation2dNodes(this SceneTree tree)
        {
            var navNodes = tree.GetNodesByType<Navigation2D>();
            if (navNodes.Count > 0)
            {
                return (true, navNodes);
            }
            else
            {
                return (false, default(List<Navigation2D>));
            }
        }
    }
}