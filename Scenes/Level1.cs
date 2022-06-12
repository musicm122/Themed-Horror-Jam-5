using Godot;
using ThemedHorrorJam5.Entities;
using ThemedHorrorJam5.Scripts.Extensions;
using ThemedHorrorJam5.Scripts.GDUtils;

namespace ThemedHorrorJam5.Scenes
{
    public class Level1 : Node2D
    {
        public PlayerV2 Player { get; set; }

        public EnemyV4 Enemy { get; set; }

        public Label MousePosition { get; set; }

        [Export]
        public NodePath PatrolPath { get; set; }

        [Export]
        public NodePath PatrolPathFollow2D { get; set; }

        public Path2D Path { get; set; }

        public PathFollow2D PathFollow2D { get; set; }

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            MousePosition = GetNode<Label>("CanvasLayer/MousePosition");
            Enemy = GetNode<EnemyV4>("Enemy");
            Player = this.GetTree().GetPlayerNode().Item2;

            if (PatrolPath != null)
            {
                Path = (Path2D)GetNode(PatrolPath);
            }
            else
            {
                GD.PushWarning("PatrolPath could not be found");
            }
            if (PatrolPathFollow2D != null)
            {
                PathFollow2D = (PathFollow2D)GetNode(PatrolPathFollow2D);
            }
            else
            {
                GD.PushWarning("PatrolPathFollow2D could not be found");
            }
        }

        public Vector2 GetPathDirection(Vector2 position)
        {
            var offset = Path.Curve.GetClosestOffset(position);
            PathFollow2D.Offset = offset;
            return PathFollow2D.Transform.x;
        }

        //  // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(float delta)
        {
            
            var mousePos = GetGlobalMousePosition();
            MousePosition.Text = "-----------------------------------------------------------\r\n";
            MousePosition.Text += $"Mouse: X:{mousePos.x} : Y:{mousePos.y}\r\n";
            MousePosition.Text += "-----------------------------------------------------------\r\n";
            MousePosition.Text += $"Player : X : {Player.Position.x} : Y : {Player.Position.y}\r\n";
            MousePosition.Text += $"Player Global : X:{Player.GlobalPosition.x} : Y : {Player.GlobalPosition.y}\r\n";
            MousePosition.Text += "-----------------------------------------------------------\r\n";
            MousePosition.Text += $"Enemy : X : {Enemy.Position.x} : Y : {Enemy.Position.y}\r\n";
            MousePosition.Text += $"Enemy Global: X :{Enemy.GlobalPosition.x} :  Y : {Enemy.GlobalPosition.y}\r\n";
            MousePosition.Text += "-----------------------------------------------------------\r\n";
        }
    }
}