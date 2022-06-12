using System;
using Godot;
using ThemedHorrorJam5.Entities.Behaviors.Interfaces;
using ThemedHorrorJam5.Scripts.Patterns.Logger;

namespace ThemedHorrorJam5.Entities.Vision
{
    public class RaycastVision : RayCast2D, IDebuggable<Node2D>, IVision
    {

        [Export]
        public bool IsDebugging { get; set; }

        [Export]
        public float ConeAngle { get; set; } = Mathf.Deg2Rad(30);

        [Export]
        public float AngleBetweenRays { get; set; } = Mathf.Deg2Rad(5);

        private bool CheckThisFrame { get; set; }

        private float MaxViewDistance { get; set; } = 100;

        public override void _Ready()
        {
            CheckThisFrame = CanCheckFrame();
        }

        public void GenerateRaycasts()
        {
            var rayCount = ConeAngle / AngleBetweenRays;
            for (int i = 0; i < rayCount; i++)
            {
                var ray = new RayCast2D();
                var angle = AngleBetweenRays * (i - rayCount / 2f);
                ray.CastTo = Vector2.Up.Rotated(angle) * MaxViewDistance;
                AddChild(ray);
                ray.Enabled = true;
            }
        }

        public bool IsDebugPrintEnabled() => IsDebugging;

        public Node2D Target { get; set; }
        public Action<Node2D> OnTargetSeen { get; set; }
        public Action<Node2D> OnTargetOutOfSight { get; set; }

        public bool CanSeeTarget() => Target != null;


        public override void _PhysicsProcess(float delta)
        {
            if (!CanCheckFrame())
            {
                return;
            }

            if (IsColliding())
            {
                if (GetCollider() != null)
                {
                    this.Print("Player found");
                    Target = (Node2D)GetCollider();
                    OnTargetSeen?.Invoke(Target);
                }
                if (Target != null)
                {
                    this.Print("Player lost");
                    OnTargetOutOfSight?.Invoke(Target);
                    Target = null;
                }
            }
        }

        public bool CanCheckFrame(int interval = 2) => new Random().Next() % interval == 0;

        public void UpdateFacingDirection(Vector2 newVelocity)
        {
            this.Rotation = this.Position.AngleToPoint(newVelocity);
        }
    }
}
