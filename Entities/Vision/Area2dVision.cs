using System;
using Godot;
using ThemedHorrorJam5.Entities.Behaviors.Interfaces;
using ThemedHorrorJam5.Scripts.Extensions;
using ThemedHorrorJam5.Scripts.Patterns.Logger;

namespace ThemedHorrorJam5.Entities.Behaviors
{
    public class Area2dVision : Area2D, IDebuggable<Node2D>, IVision
    {
        public Action<Node2D> OnTargetSeen { get; set; }
        public Action<Node2D> OnTargetOutOfSight { get; set; }

        public Node2D Target { get; set; }

        [Export]
        public bool IsDebugging { get; set; }

        public bool LineOfSight = false;

        public override void _Ready()
        {
            this.ConnectBodyEntered(this, nameof(OnVisionRadiusBodyEntered));
            this.ConnectBodyExited(this, nameof(OnVisionRadiusBodyExit));
        }

        private void OnVisionRadiusBodyEntered(Node body)
        {
            if (body.Name.ToLower().Contains("player"))
            {
                this.PrintCaller();
                Target = (Node2D)body;
                if (HasLineOfSight(Target.Position))
                {
                    OnTargetSeen?.Invoke(Target);
                    //CurrentState = AggroBehavior;
                    //CurrentCoolDownCounter = MaxCoolDownTime;
                }
            }
        }

        private void OnVisionRadiusBodyExit(Node body)
        {
            if (body.Name.ToLower().Contains("player"))
            {
                Target = (Node2D)body;
                this.PrintCaller();
                //CurrentCoolDownCounter = MaxCoolDownTime;
                if (!HasLineOfSight(Target.Position))
                {
                    OnTargetOutOfSight?.Invoke(Target);
                    //CurrentState = EnemyBehaviorStates.Patrol;
                    //CurrentCoolDownCounter = MaxCoolDownTime;
                }
                else
                {
                    OnTargetSeen?.Invoke(Target);
                    //CurrentCoolDownCounter = MaxCoolDownTime;
                }
                //this.player = null;
                //this.CurrentState = EnemyBehaviorStates.Patrol;
            }
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
            //if (!CanCheckFrame()) return LineOfSight;
            var spaceState = GetWorld2d().DirectSpaceState;
            var result = spaceState.IntersectRay(GlobalTransform.origin, point, null, CollisionMask);
            LineOfSight = result?.Count > 0;
            return LineOfSight;
        }

        public void UpdateFacingDirection(Vector2 newVelocity)
        {
            this.Rotation = this.Position.AngleToPoint(newVelocity);

            // if (newVelocity.x < 0)
            // {
            //     Scale = new Vector2(-1, Scale.y);
            // }
            // else
            // {
            //     Scale = new Vector2(1, Scale.y);
            // }
            // if (newVelocity.y < 0)
            // {
            //     Scale = new Vector2(1, Scale.y);
            // }
        }


        public bool IsPlayerInSight()
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
    }
}