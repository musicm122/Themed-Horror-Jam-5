using Godot;
using System;
using ThemedHorrorJam5.Scripts.ItemComponents;

namespace ThemedHorrorJam5.Entities.Components
{
    public class RaycastVision : RayCast2D, IDebuggable<Node2D>
    {
        public Action<Player> OnPlayerSeen;
        public Action<Player> OnPlayerOutOfSight;

        [Export]
        public float ConeAngle { get; set; } = Mathf.Deg2Rad(30);
        public float MaxViewDistance { get; set; } = 100;

        [Export]
        public float AngleBetweenRays { get; set; } = Mathf.Deg2Rad(5);

        public void GenerateRaycasts()
        {
            var rayCount = ConeAngle / AngleBetweenRays;
            for (int i = 0; i < rayCount; i++)
            {
                var ray = new RayCast2D();
                var angle = AngleBetweenRays * (i - (rayCount / 2f));
                ray.CastTo = Vector2.Up.Rotated(angle) * MaxViewDistance;
                AddChild(ray);
                ray.Enabled = true;
            }
        }

        [Export]
        public bool IsDebugging { get; set; }

        public bool IsDebugPrintEnabled() => IsDebugging;

        public Player Target { get; set; }

        public bool CanSeePlayer() => Target != null;

        public override void _PhysicsProcess(float delta)
        {
            if (IsColliding())
            {
                if (GetCollider() is Player player)
                {
                    this.Print("Player found");
                    Target = player;
                    OnPlayerSeen?.Invoke(player);
                }
                if (Target != null)
                {
                    this.Print("Player lost");
                    OnPlayerOutOfSight(Target);
                    Target = null;
                }
            }
        }
    }
}
