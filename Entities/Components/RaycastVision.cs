using Godot;
using System;
using ThemedHorrorJam5.Scripts.Patterns.Logger;

namespace ThemedHorrorJam5.Entities.Components
{
    public class RaycastVision : RayCast2D, IDebuggable<Node2D>
    {
        public Action<Node2D> OnTargetSeen;
        public Action<Node2D> OnTargetOutOfSight;

        [Export]
        public float ConeAngle { get; set; } = Mathf.Deg2Rad(30);
        public bool CheckThisFrame = false;

       

        public float MaxViewDistance { get; set; } = 100;

        [Export]
        public float AngleBetweenRays { get; set; } = Mathf.Deg2Rad(5);

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
                var angle = AngleBetweenRays * (i - (rayCount / 2f));
                ray.CastTo = Vector2.Up.Rotated(angle) * MaxViewDistance;
                AddChild(ray);
                ray.Enabled = true;
            }
        }

        [Export]
        public bool IsDebugging { get; set; }

        public bool IsDebugPrintEnabled() => IsDebugging;

        public Node2D Target { get; set; }

        public bool CanSeePlayer() => Target != null;

        public override void _PhysicsProcess(float delta)
        {
            if(!CanCheckFrame()){ 
                return;
            }

            if (IsColliding())
            {
                if (GetCollider()!=null)
                {
                    this.Print("Player found");
                    Target = (Node2D)GetCollider();
                    OnTargetSeen?.Invoke(Target);
                }
                if (Target != null)
                {
                    this.Print("Player lost");
                    OnTargetOutOfSight(Target);
                    Target = null;
                }
            }
        }
        private bool CanCheckFrame(int interval = 2)
        {
            Random random = new Random();
            return random.Next() % interval == 0;
        }
    }
}
