using Godot;
using ThemedHorrorJam5.Entities.Components;
using ThemedHorrorJam5.Scripts.Patterns.Logger;

namespace ThemedHorrorJam5.Scripts.UI
{
    public class Hud : Control, IDebuggable<Node>
    {
        [Export]
        public bool IsDebugging { get; set; } = false;

        private string HealthBarDisplayPath { get; set; } = "./HealthPanel/HealthBar";
        private string GameOverPanelPath { get; set; } = "./GameOverPanel";

        public Label HealthBarDisplay { get; set; }

        public PanelContainer GameOverPanel { get; set; }

        public bool IsDebugPrintEnabled() => IsDebugging;

        public override void _Ready()
        {
            HealthBarDisplay = GetNode<Label>(HealthBarDisplayPath);
            GameOverPanel = GetNode<PanelContainer>(GameOverPanelPath);
            GameOverPanel.Hide();
        }

        public void RefreshUI(Status status)
        {
            string maxHealthVal = "Health: ";
            for (int i = 0; i < status.MaxHealth; i++)
            {
                if (i < status.CurrentHealth)
                {
                    maxHealthVal += "|X|";
                }
                else
                {
                    maxHealthVal += "| |";
                }
            }
            HealthBarDisplay.Text = $"{maxHealthVal}";
            if (status.CurrentHealth <= 0)
            {
                GameOverPanel.Show();
            }
        }
    }
}