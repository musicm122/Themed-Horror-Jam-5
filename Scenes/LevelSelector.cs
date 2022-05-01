using Godot;
using System;
using ThemedHorrorJam5.Scripts.Extensions;

namespace ThemedHorrorJam5.Scenes
{
    public class LevelSelector : Node
    {
        [Export(PropertyHint.File, "*.tscn")]
        public string PlayerInteractionMechanicScene { get; set; } = "res://Scenes/TestPuzzleMechanics.tscn";

        [Export(PropertyHint.File, "*.tscn")]
        public string EnemyAIScene { get; set; } = "res://Scenes/Level1.tscn";

        public override void _Ready()
        {
            Button interaction = GetNode<Button>("RootControl/VBoxContainer/PlayerInteractionMechanics");
            interaction.ConnectButtonPressed(this, nameof(OnInteractionButtonPressed));

            Button ai = GetNode<Button>("RootControl/VBoxContainer/EnemyAI");
            ai.ConnectButtonPressed(this, nameof(OnEnemyAIButtonPressed));

            Button quit = GetNode<Button>("RootControl/VBoxContainer/Quit");
            quit.ConnectButtonPressed(this, nameof(OnQuitButtonPressed));

            interaction.GrabFocus();
        }

        public void OnInteractionButtonPressed()
        {
            GetTree().ChangeScene(PlayerInteractionMechanicScene);
        }

        public void OnEnemyAIButtonPressed()
        {
            GetTree().ChangeScene(EnemyAIScene);
        }


        public void OnQuitButtonPressed()
        {
            GetTree().Quit();
        }
    }
}