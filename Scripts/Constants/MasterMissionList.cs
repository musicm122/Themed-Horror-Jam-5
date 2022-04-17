using System.Collections.Generic;
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
                                (player) => player.HasItem("Foo's Glasses"))
                };

        public static MissionElement GetMissionByTitle(string title) => Missions.Find(m => m.Title == title);
    }
}