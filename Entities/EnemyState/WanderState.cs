using System;
using ThemedHorrorJam5.Scripts.Patterns.StateMachine;
using ThemedHorrorJam5.Scripts.GDUtils;
using Godot;
using ThemedHorrorJam5.Scripts.Enum;
using ThemedHorrorJam5.Entities.Components;

namespace ThemedHorrorJam5.Entities
{
    public class WanderState : State
    {
        private float WanderAngle { get; set; } = 0;
        private const int CircleRadius = 8;
        private const float Randomness = 0.2f;

        //const WANDER_RANDOMNESS: float = 0.2
        //var wander_angle: float = 0

        private EnemyV4 Agent { get; set; }


        public WanderState(EnemyV4 enemy)
        {
            Agent = enemy;
            Agent.EnclosureZone = new Rect2(Agent.Position,new Vector2(50,50));
            this.Name = EnemyBehaviorStates.Wander.GetDescription();
            this.OnEnter += () => this.Logger.Debug("WanderState OnEnter called");
            this.OnExit += () => this.Logger.Debug("WanderState Exit called");
            this.OnFrame += Process;
        }

        private Vector2 EnclosureSteering()
        {
            var desiredVelocity = Vector2.Zero;
            if (Agent.Position.x < Agent.EnclosureZone.Position.x)
            {
                desiredVelocity.x += 1;
            }
            else if (Agent.Position.x > Agent.EnclosureZone.Position.x + Agent.EnclosureZone.Size.x)
            {
                desiredVelocity.x -= 1;
            }

            if (Agent.Position.y < Agent.EnclosureZone.Position.y)
            {
                desiredVelocity.y += 1;
            }
            else if (Agent.Position.y > Agent.EnclosureZone.Position.y + +Agent.EnclosureZone.Size.y)
            {
                desiredVelocity.y -= 1;
            }

            desiredVelocity = desiredVelocity.Normalized() * Agent.MaxSpeed;
            if (desiredVelocity != Vector2.Zero)
            {
                WanderAngle = desiredVelocity.Angle();
                return desiredVelocity - Agent.Velocity;
            }
            return Vector2.Zero;
        }

        private Vector2 WanderSteering()
        {
            var rand = new Random();
            WanderAngle = rand.RandomFloat(WanderAngle - Randomness, WanderAngle + Randomness);
            var vectorToCircle = Agent.Velocity.Normalized() * Agent.MaxSpeed;
            var desiredVelocity = vectorToCircle + new Vector2(CircleRadius, 0).Rotated(WanderAngle);
            return desiredVelocity - Agent.Velocity;
        }

        private void Process(float delta)
        {
            var steering = Vector2.Zero;

            var enclosureSteeringVector = EnclosureSteering();
            steering += enclosureSteeringVector;

            var wanderSteeringVector = WanderSteering();
            steering += wanderSteeringVector;

            var avoidObstaclesSteeringVector = AvoidObstaclesSteering();
            steering += avoidObstaclesSteeringVector;

            var clampedSteering = steering.Clamped(Agent.MaxSteering);
            steering = clampedSteering;

            Agent.Velocity += steering;
            Agent.Velocity = Agent.Velocity.Clamped(Agent.MaxSpeed);

            Agent.Velocity = Agent.MoveAndSlide(Agent.Velocity);
            Agent.Status.VisionManager.UpdateFacingDirection(Agent.Velocity);
            if (Agent.IsDebugging)
            {
                Agent.Status.DebugLabel.Text =
                   @$"
                    |-----------------------------------------------------------
                    | Steering Vector : {steering}
                    | enclosureSteeringVector {enclosureSteeringVector}
                    | wanderSteeringVector {wanderSteeringVector}
                    | avoidObstaclesSteeringVector {avoidObstaclesSteeringVector}
                    | clampedSteering {clampedSteering}
                    | Velocity : {Agent.Velocity}
                    |-----------------------------------------------------------";
            }
        }

        private Vector2 AvoidObstaclesSteering()
        {
            Agent.ObstacleAvoidance.Rotation = Agent.Velocity.Angle();
            var raycasts = Agent.ObstacleAvoidance.GetChildren();
            for (int i = 0; i < raycasts.Count; i++)
            {
                var raycast = (RayCast2D)raycasts[i];
                if (raycast.IsColliding())
                {
                    var obstacle = (PhysicsBody2D)raycast.GetCollider();
                    return (Agent.Position + Agent.Velocity - obstacle.Position).Normalized() * Agent.AvoidForce;
                }
            }
            return Vector2.Zero;
        }

    }
}