using System;
using Godot;
using ThemedHorrorJam5.Entities.Behaviors.Interfaces;
using ThemedHorrorJam5.Scripts.Extensions;
using ThemedHorrorJam5.Scripts.Patterns.Logger;

namespace ThemedHorrorJam5.Entities.Vision
{
    public class Area2dVision : Area2D, IDebuggable<Node2D>, IVision
    {
        public Action<Node2D> OnTargetSeen { get; set; }
        public Action<Node2D> OnTargetOutOfSight { get; set; }


        [Export] public bool IsDebugging { get; set; }

        public Node2D OldTarget { get; set; }
        public Node2D NewTarget { get; set; }

        private bool LineOfSight { get; set; }

        public override void _Ready()
        {
            this.ConnectBodyEntered(this, nameof(OnVisionRadiusBodyEntered));
            this.ConnectBodyExited(this, nameof(OnVisionRadiusBodyExit));
        }

        private void OnVisionRadiusBodyEntered(Node body)
        {
            if (!body.Name.ToLower().Contains("player")) return;

            this.PrintCaller();
            NewTarget = (Node2D)body;
            LineOfSight = HasLineOfSight(NewTarget.Position);
            if (!LineOfSight) return;
            OnTargetSeen?.Invoke(NewTarget);
            OldTarget = NewTarget;
            NewTarget = null;
        }

        private void OnVisionRadiusBodyExit(Node body)
        {
            if (!body.Name.ToLower().Contains("player")) return;
            this.PrintCaller();
            if (OldTarget == null) return;
            LineOfSight = HasLineOfSight(OldTarget.GlobalPosition);
            if (!LineOfSight) return;
            OnTargetOutOfSight?.Invoke(OldTarget);
        }

        public bool CanSeeTarget()
        {
            var bodies = GetOverlappingBodies();
            if (bodies == null || bodies.Count == 0) return false;
            for (int i = 0; i < bodies.Count; i++)
            {
                var body = (Node)bodies[i];
                if (body.Name.ToLower().Contains("player"))
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasLineOfSight(Vector2 point)
        {
            var spaceState = GetWorld2d().DirectSpaceState;
            var result = spaceState.IntersectRay(GlobalTransform.origin, point, null, CollisionMask);
            LineOfSight = result?.Count > 0;
            return LineOfSight;
        }

        public void UpdateFacingDirection(Vector2 newVelocity)
        {
            this.Rotation = this.Position.AngleToPoint(newVelocity);
        }

        public void LookAtPoint(Vector2 point)
        {
            this.LookAt(point);
        }
    }
}