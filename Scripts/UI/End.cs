using Godot;

namespace ThemedHorrorJam5.Scripts.UI
{
    public class End : Node
    {
        public string TitleScene { get; set; } = "res://Scenes/Title.tscn";

        [Export]
        public float Cooldown { get; set; } = 2f;

        public float RemainingCooldownTime { get; set; }

        // Declare member variables here. Examples:
        // private int a = 2;
        // private string b = "text";

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            RemainingCooldownTime = Cooldown;
        }

        //  // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(float delta)
        {
            if (RemainingCooldownTime <= 0)
            {
                GetTree().ChangeScene(TitleScene);
            }
            RemainingCooldownTime -= delta;
        }
    }
}

//https://opengameart.org/content/barn-owl-screech