using ThemedHorrorJam5.Scripts.Mission;

namespace ThemedHorrorJam5.Entities.Components
{
    public interface IUiBehavior
    {
        bool IsDebugging { get; set; }
        PauseMenu PauseMenu { get; set; }
        PlayerDataStore DataStore { get; set; }

        void AddItem(string name, int amt = 1);
        void AddMission(string title);
        void EvaluateMissions(PlayerDataStore playerDataStore);
        void Init(PlayerDataStore dataStore);
        bool IsDebugPrintEnabled();
        void RefreshUI();
        void RemoveItem(string name);
        void RemoveItems(string name, int amt);
        void RemoveMission(MissionElement mission);
        void RemoveMissionByTitle(string missionTitle);
        void _Process(float delta);
        void _Ready();
    }
}