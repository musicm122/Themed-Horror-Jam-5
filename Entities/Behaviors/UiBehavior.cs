using Godot;
using ThemedHorrorJam5.Scripts.Constants;
using ThemedHorrorJam5.Scripts.GDUtils;
using ThemedHorrorJam5.Scripts.ItemComponents;
using ThemedHorrorJam5.Scripts.Mission;
using ThemedHorrorJam5.Scripts.UI;

namespace ThemedHorrorJam5.Entities.Components
{
    public class UiBehavior : Node2D, IDebuggable<Node>
    {
        public PlayerState State { get; set; }

        public PauseMenu PauseMenu { get; set; }

        private Hud Hud { get; set; }

        [Export]
        public bool IsDebugging { get; set; } = false;

        public bool IsDebugPrintEnabled() => IsDebugging;

        public override void _Ready()
        {
            Hud = GetNode<Hud>("./Camera2D/CanvasLayer/Hud");
            PauseMenu = GetNode<PauseMenu>("./CanvasLayer/PauseMenu");
        }

        private void UpdateMissions(object sender, MissionManagerEventArgs args)
        {
            RefreshUI();
        }

        public void AddMission(string title)
        {
            var mission = MasterMissionList.GetMissionByTitle(title);
            if (mission != null)
            {
                this.Print($"AddMission called with mission title: {title}");

                State.MissionManager.AddIfDNE(mission);
                RefreshUI();
            }
        }

        public void RemoveMission(MissionElement mission)
        {
            this.Print($"RemoveMission called with mission : {mission}");
            State.MissionManager.Remove(mission);
            RefreshUI();
        }

        public void EvaluateMissions(Player player)
        {
            State.MissionManager.EvaluateMissionsState(player);
            RefreshUI();
        }

        public void AddItem(string name, int amt = 1)
        {
            this.Print($"AddItem called with name:{name}, amt:{amt} ");

            State.Inventory.Add(name, amt);
            RefreshUI();
        }

        public void RemoveItem(string name)
        {
            this.Print($"RemoveItem called with name:{name}");
            State.Inventory.Remove(name);
            RefreshUI();
        }

        public override void _Process(float delta)
        {
            if (State.PlayerStatus.IsDead())
            {
                PauseMenu.IsPauseOptionEnabled = false;
                if (InputUtils.IsAnyKeyPressed())
                {
                    this.Print("Reloading Scene");
                    GetTree().ReloadCurrentScene();
                }
            }
        }

        public void RefreshUI()
        {
            Hud.RefreshUI(State.PlayerStatus);
            PauseMenu.RefreshUI(State.Inventory, State.MissionManager);
        }

        public void Init(PlayerState state)
        {
            State = state;
            State.MissionManager.AddMissionEvent += UpdateMissions;
            State.MissionManager.RemoveMissionEvent += UpdateMissions;
            RefreshUI();
        }
    }
}