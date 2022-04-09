using Godot;
using System.Collections.Generic;
using System.Linq;

namespace ThemedHorrorJam5.Scripts.ItemComponents
{
    public class MissionManager
    {
        public delegate void AddingMissionHandler(object sender, MissionManagerEventArgs args);

        public event AddingMissionHandler AddMissionEvent;

        public delegate void RemovingMissionHandler(object sender, MissionManagerEventArgs args);

        public event RemovingMissionHandler RemoveMissionEvent;

        protected virtual void RaiseAddingMission(Mission mission)
        {
            AddMissionEvent?.Invoke(this, new MissionManagerEventArgs(mission));
        }
        protected virtual void RaiseRemovingMission(Mission mission)
        {
            RemoveMissionEvent?.Invoke(this, new MissionManagerEventArgs(mission));
        }

        [Signal]
        public delegate void RemovingMission(Mission mission);

        private List<Mission> Missions { get; set; } = new List<Mission>();

        public bool HasMission(string name) => Missions.Any(item => item.Title == name);

        public void AddIfDNE(Mission mission)
        {
            if (!Missions.Contains(mission))
            {
                RaiseAddingMission(mission);
                Missions.Add(mission);
            }
        }

        public void Remove(Mission mission)
        {
            if (Missions.Contains(mission))
            {
                RaiseRemovingMission(mission);
                Missions.Remove(mission);
            }
        }

        public string Display()
        {
            var retval = "Points of Interest:\r\n================\r\n";
            if (Missions.Count() > 0)
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

        public IEnumerable<Mission> Completed() => Missions.Where(m => m.IsComplete == true);

        public IEnumerable<Mission> Uncompleted() => Missions.Where(m => m.IsComplete == false);

        public int Count() => Missions.Count();

        public void EvaluateMissionsState(Player player)
        {
            for (int i = 0; i < Missions.Count(); i++)
            {
                if (Missions[i].EvaluateCompletionState(player))
                {
                    Missions[i].IsComplete = true;
                }
            }
        }
    }
}