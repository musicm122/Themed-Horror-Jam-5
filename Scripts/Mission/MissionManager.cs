using Godot;
using System.Collections.Generic;
using System.Linq;
using ThemedHorrorJam5.Entities;
using ThemedHorrorJam5.Scripts.Mission;

namespace ThemedHorrorJam5.Scripts.ItemComponents
{
    public class MissionManager
    {
        public delegate void AddingMissionHandler(object sender, MissionManagerEventArgs args);

        public event System.EventHandler<MissionManagerEventArgs> AddMissionEvent;

        public delegate void RemovingMissionHandler(object sender, MissionManagerEventArgs args);

        public event System.EventHandler<MissionManagerEventArgs> RemoveMissionEvent;

        protected virtual void RaiseAddingMission(MissionElement mission)
        {
            AddMissionEvent?.Invoke(this, new MissionManagerEventArgs(mission));
        }

        protected virtual void RaiseRemovingMission(MissionElement mission)
        {
            RemoveMissionEvent?.Invoke(this, new MissionManagerEventArgs(mission));
        }

        [Signal]
        public delegate void RemovingMission(MissionElement mission);

        private List<MissionElement> Missions { get; } = new List<MissionElement>();

        public bool HasMission(string name) => Missions.Any(item => item.Title == name);

        public void AddIfDNE(MissionElement mission)
        {
            if (!Missions.Contains(mission))
            {
                RaiseAddingMission(mission);
                Missions.Add(mission);
            }
        }

        public void Remove(MissionElement mission)
        {
            if (Missions.Contains(mission))
            {
                RaiseRemovingMission(mission);
                Missions.Remove(mission);
            }
        }

        public void RemoveByTitle(string missionTitle)
        {
            var missionToRemove = Missions.Find(m => m.Title == missionTitle);
            RaiseRemovingMission(missionToRemove);
            Missions.Remove(missionToRemove);
        }

        public string Display()
        {
            var retval = "Points of Interest:\r\n================\r\n";
            if (Missions.Count > 0)
            {
                for (int i = 0; i < Missions.Count; i++)
                {
                    if (Missions[i].IsComplete)
                    {
                        retval += $"{Missions[i].Title} : (Complete)\r\n";
                    }
                    else
                    {
                        retval += $"{Missions[i].Title} \r\n";
                    }
                    retval += Missions[i].Details + "\r\n";
                    retval += Missions[i].IsComplete;
                }
            }
            else
            {
                retval += "Empty";
            }
            return retval;
        }

        public IEnumerable<string> Titles() => Missions.Select(m => m.Title);

        public IEnumerable<string> Details() => Missions.Select(m => m.Details);

        public IEnumerable<MissionElement> Completed() => Missions.Where(m => m.IsComplete);

        public IEnumerable<MissionElement> Uncompleted() => Missions.Where(m => !m.IsComplete);

        public int Count() => Missions.Count;

        public void EvaluateMissionsState(PlayerState playerState)
        {
            for (int i = 0; i < Missions.Count; i++)
            {
                if (Missions[i].EvaluateCompletionState(playerState))
                {
                    Missions[i].IsComplete = true;
                }
            }
        }
    }
}