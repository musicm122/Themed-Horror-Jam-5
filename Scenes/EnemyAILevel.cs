using System.Globalization;
using Godot;
using ThemedHorrorJam5.Entities;
using ThemedHorrorJam5.Scripts.Extensions;

namespace ThemedHorrorJam5.Scenes
{
    public class EnemyAILevel : Node2D
    {
        public Navigation2D Navigation { get; set; }
        private PlayerV2 Player { get; set; }

        private EnemyV4 Enemy { get; set; }

        private Label MousePosition { get; set; }

        [Export] public NodePath PatrolPath { get; set; }

        [Export] public NodePath PatrolPathFollow2D { get; set; }

        private Path2D Path { get; set; }

        private PathFollow2D PathFollow2D { get; set; }

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            Navigation = GetNode<Navigation2D>("Navigation2D");
            MousePosition = GetNode<Label>("CanvasLayer/MousePosition");
            Enemy = GetNode<EnemyV4>("Enemies/Enemy");

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
            MousePosition.Text =
                $@"-----------------------------------------------------------\r\n
                Mouse: X:{mousePos.x.ToString(CultureInfo.InvariantCulture)} : Y:{mousePos.y.ToString(CultureInfo.InvariantCulture)}\r\n
                -----------------------------------------------------------\r\n
                Player : X : {Player.Position.x.ToString(CultureInfo.InvariantCulture)} : Y : {Player.Position.y.ToString(CultureInfo.InvariantCulture)}\r\n
                Player Global : X:{Player.GlobalPosition.x.ToString(CultureInfo.InvariantCulture)} : Y : {Player.GlobalPosition.y.ToString(CultureInfo.InvariantCulture)}\r\n
                -----------------------------------------------------------\r\n";
            if (Enemy == null) return;
            MousePosition.Text =
                $@"-----------------------------------------------------------\r\n
                Enemy : X : {Enemy.Position.x.ToString(CultureInfo.InvariantCulture)} : Y : {Enemy.Position.y.ToString(CultureInfo.InvariantCulture)}\r\n
                Enemy Global: X :{Enemy.GlobalPosition.x.ToString(CultureInfo.InvariantCulture)} :  Y : {Enemy.GlobalPosition.y.ToString(CultureInfo.InvariantCulture)}\r\n
                -----------------------------------------------------------\r\n";
        }
    }
}