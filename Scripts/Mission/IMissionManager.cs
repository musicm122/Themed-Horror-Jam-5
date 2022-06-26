using System;
using System.Collections.Generic;
using ThemedHorrorJam5.Entities;
using ThemedHorrorJam5.Scripts.Mission;

namespace ThemedHorrorJam5.Scripts.ItemComponents
{
    public interface IMissionManager
    {
        event EventHandler<MissionManagerEventArgs> AddMissionEvent;
        event EventHandler<MissionManagerEventArgs> RemoveMissionEvent;

        void AddIfDNE(MissionElement mission);
        IEnumerable<MissionElement> Completed();
        int Count();
        IEnumerable<string> Details();
        string Display();
        void EvaluateMissionsState(PlayerDataStore playerDataStore);
        bool HasMission(string name);
        void Remove(MissionElement mission);
        void RemoveByTitle(string missionTitle);
        IEnumerable<string> Titles();
        IEnumerable<MissionElement> Uncompleted();
    }
}