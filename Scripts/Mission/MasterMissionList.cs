using System.Collections.Generic;
using Godot;
using ThemedHorrorJam5.Scripts.Mission;

namespace ThemedHorrorJam5.Scripts.Constants
{
    public static class MasterMissionList
    {
        public static readonly List<MissionElement> Missions =
            new List<MissionElement>(){
                new MissionElement(
                                "Find the glasses",
                                "Find Foo's missing specs.",
                                (playerState) => playerState.Inventory.HasItem("Foo's Glasses"))
                };

        public static MissionElement GetMissionByTitle(string title)
        {
            var result = Missions.Find(m => m.Title == title);
            if (result == null)
            {
                GD.PrintErr($"GetMissionByTitle({title}) not found");
                throw new MissionNotFoundException(title);
            }
            return result;
        }
    }
}