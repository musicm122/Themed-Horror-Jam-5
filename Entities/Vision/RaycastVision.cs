using System;
using Godot;
using ThemedHorrorJam5.Entities.Behaviors.Interfaces;
using ThemedHorrorJam5.Scripts.Extensions;
using ThemedHorrorJam5.Scripts.Patterns.Logger;

namespace ThemedHorrorJam5.Entities.Vision
{
    public class RaycastVision : RayCast2D, IDebuggable<Node2D>, IVision
    {
        [Export] public Vector2 StartDirection { get; set; } = Vector2.Up;

        [Export] public bool IsDebugging { get; set; }

        [Export] public float ConeAngle { get; set; } = Mathf.Deg2Rad(30);

        [Export] public float AngleBetweenRays { get; set; } = Mathf.Deg2Rad(5);

        [Export] public float MaxViewDistance { get; set; } = 100;

        public bool CanSeeTarget() => NewTarget != null;

        public override void _Ready()
        {
            GenerateRaycasts();
        }

        private void GenerateRaycasts()
        {
            var rayCount = ConeAngle / AngleBetweenRays;
            for (int i = 0; i < rayCount; i++)
            {
                var ray = new RayCast2D();
                var angle = AngleBetweenRays * (i - rayCount / 2f);
                ray.CastTo = StartDirection.Rotated(angle) * MaxViewDistance;
                AddChild(ray);
                ray.Enabled = true;
            }
        }

        private Node2D GetPlayerCollider()
        {
            var rays = this.GetChildrenOfType<RayCast2D>();
            for (var i = 0; i < rays.Count; i++)
            {
                if (IsPlayer(rays[i]))
                {
                    return (Node2D)rays[i].GetCollider();
                }
            }

            return null;
        }

        public bool IsDebugPrintEnabled() => IsDebugging;

        public Node2D OldTarget { get; set; }
        public Node2D NewTarget { get; set; }
        public Action<Node2D> OnTargetSeen { get; set; }
        public Action<Node2D> OnTargetOutOfSight { get; set; }

        private void FlushVisionCheck()
        {
            if (!CanCheckFrame()) return;

            // if new target is not null then target is seen
            NewTarget = GetPlayerCollider();
            if (NewTarget != null)
            {
                this.Print("Player found");
                OnTargetSeen?.Invoke(NewTarget);
            }
            // if old target is not null and new target is null then target is out of sight
            else if (OldTarget != null)
            {
                this.Print("Player out of sight");
                OnTargetOutOfSight?.Invoke(OldTarget);
            }

            // set old target to new target and new target to null
            OldTarget = NewTarget;
            NewTarget = null;
        }

        public override void _PhysicsProcess(float delta)
        {
            FlushVisionCheck();
        }

        static bool IsPlayer(RayCast2D ray) =>
            ray.IsColliding() && ((Node2D)ray.GetCollider()).Name.ToLower().Contains("player");

        public bool CanCheckFrame(int interval = 2) => new Random().Next() % interval == 0;

        public void UpdateFacingDirection(Vector2 newVelocity)
        {
            this.Rotation = this.Position.AngleToPoint(newVelocity.Normalized());
        }

        public void LookAtPoint(Vector2 point)
        {
            this.LookAt(point);
        }
    }
}