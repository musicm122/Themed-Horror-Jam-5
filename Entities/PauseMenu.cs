using Godot;
using ThemedHorrorJam5.Scripts.GDUtils;
using ThemedHorrorJam5.Scripts.ItemComponents;

namespace ThemedHorrorJam5.Entities
{
    public class PauseMenu : Control, IDebuggable<Node>
    {
        [Export]
        public bool IsPauseOptionEnabled { get; set; } = true;

        [Export]
        public bool IsDebugging { get; set; } = false;

        public bool IsDebugPrintEnabled() => IsDebugging;

        [Export]
        private string InventoryDisplayPath { get; set; } = "./InventoryPanel/InventoryDisplay";

        [Export]
        private string MissionManagerDisplayPath { get; set; } = "./MissionPanel/MissionDisplay";

        [Export]
        private string TitleDisplayPath { get; set; } = "./TitlePanel/Title";

        public Label InventoryDisplay { get; set; }
        public Label MissionDisplay { get; set; }
        public Label TitleDisplay { get; set; }

        public bool IsHidden = false;

        [Export]
        private readonly float PauseToggleCooldownWaitTime = 1.0f;

        private float AccumulatorPauseToggleCooldown = 0.0f;
        private bool CanTogglePause = true;

        // Declare member variables here. Examples:
        // private int a = 2;
        // private string b = "text";

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            TitleDisplay = GetNode<Label>(TitleDisplayPath);
            InventoryDisplay = GetNode<Label>(InventoryDisplayPath);
            MissionDisplay = GetNode<Label>(MissionManagerDisplayPath);
            if (TitleDisplay == null)
            {
                this.Print("TitleDisplay is null");
            }
            if (InventoryDisplay == null)
            {
                this.Print("InventoryDisplay is null");
            }
            if (MissionDisplay == null)
            {
                this.Print("MissionDisplay is null");
            }
        }

        public override void _Process(float delta)
        {
            if (!IsPauseOptionEnabled) return;
            if (Input.IsActionPressed(InputAction.Pause))
            {
                if (CanTogglePause)
                {
                    GD.Print("Can toggle pause yet, time left ");
                    CanTogglePause = false;
                    TogglePauseMenu();
                    AccumulatorPauseToggleCooldown = PauseToggleCooldownWaitTime;
                }
                else
                {
                    GD.Print("Cannot toggle pause yet, time left ", AccumulatorPauseToggleCooldown);
                }
            }
            if (AccumulatorPauseToggleCooldown > 0)
            {
                AccumulatorPauseToggleCooldown -= delta;
            }
            else
            {
                CanTogglePause = true;
            }
        }

        private void TogglePauseMenu()
        {
            this.Print("Toggling Pause Menu");
            this.TogglePause();
            if (this.IsPaused())
            {
                this.Print("Should be showing Pause Menu");
                Show();
            }
            else
            {
                this.Print("Should be hiding Pause Menu");
                Hide();
            }
        }

        public void RefreshUI(Player player)
        {
            InventoryDisplay.Text = player.Inventory.Display();
            MissionDisplay.Text = player.MissionManager.Display();
        }
    }
}