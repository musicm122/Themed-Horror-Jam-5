using Godot;

namespace ThemedHorrorJam5.Scripts.UI
{
    public class Title : Node
    {
        [Export(PropertyHint.File, "*.tscn")]
        public string NewGameScene { get; set; } = "res://Scenes/Level1.tscn";

        public AudioStreamPlayer TitleSongAudio { get; set; }
        public AudioStreamPlayer ButtonAudio { get; set; }

        public override void _Ready()
        {
            TitleSongAudio =
                GetNode<AudioStreamPlayer>(new NodePath("Music"));

            ButtonAudio =
                GetNode<AudioStreamPlayer>(new NodePath("ButtonSound"));

            var startButton = (Button)FindNode("StartButton");
            if (startButton != null)
            {
                startButton.GrabFocus();
            }
            else
            {
                GD.PushWarning("startButton is null ");
            }
        }

        public void OnStartButtonPressed()
        {
            GetTree().ChangeScene(NewGameScene);
        }

        public void OnQuitButtonPressed()
        {
            GetTree().Quit();
        }

        public void OnVBoxFocusEntered()
        {
            ButtonAudio.Play();
        }

        //https://opengameart.org/content/jazzy-vibes-78-jazz-guitar-medley
    }
}