using ThemedHorrorJam5.Scripts.Mission;

namespace ThemedHorrorJam5.Entities.Components
{
    public interface IUiBehavior
    {
        bool IsDebugging { get; set; }
        PauseMenu PauseMenu { get; set; }
        PlayerState State { get; set; }

        void AddItem(string name, int amt = 1);
        void AddMission(string title);
        void EvaluateMissions(PlayerState playerState);
        void Init(PlayerState state);
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