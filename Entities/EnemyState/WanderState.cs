using System;
using System.Globalization;
using Godot;
using ThemedHorrorJam5.Scripts.Enum;
using ThemedHorrorJam5.Scripts.GDUtils;
using ThemedHorrorJam5.Scripts.Patterns.StateMachine;

namespace ThemedHorrorJam5.Entities.EnemyState
{
    public class WanderState : State
    {
        private float WanderAngle { get; set; }

        private const int CircleRadius = 8;

        private const float Randomness = 0.2f;

        private EnemyV4 Agent { get; set; }

        public WanderState(EnemyV4 enemy)
        {
            Agent = enemy;
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
            if (desiredVelocity == Vector2.Zero) return Vector2.Zero;
            WanderAngle = desiredVelocity.Angle();
            return desiredVelocity - Agent.Velocity;
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

            Agent.Move(delta);

            if (Agent.IsDebugging)
            {
                Agent.EnemyDataStore.DebugLabel.Text =
                    Agent.EnemyDataStore.DebugLabel.Text =
                        @$"
                    |-----------------------------------------------------------
                    | Agent Position : {Agent.Position.ToString()}
                    | Agent Global Position : {Agent.GlobalPosition.ToString()}
                    | Velocity : {Agent.Velocity.ToString()}
                    |-----------------------------------------------------------
                    |-----------------------------------------------------------
                    | EnclosureZone : {Agent.EnclosureZone.ToString()}
                    | EnclosureZone Position  : {Agent.EnclosureZone.Position.ToString()}
                    | EnclosureZone Relative Position: {Agent.ToLocal(Agent.EnclosureZone.Position).ToString()}
                    | EnclosureZone Width : {Agent.EnclosureZone.Size.x.ToString(CultureInfo.InvariantCulture)}
                    | EnclosureZone Height : {Agent.EnclosureZone.Size.y.ToString(CultureInfo.InvariantCulture)}
                    |-----------------------------------------------------------
                    | In Enclosure Zone Global : {Agent.EnclosureZone.HasPoint(Agent.GlobalPosition).ToString()}
                    | In Enclosure Zone Local : {Agent.EnclosureZone.HasPoint(Agent.Position).ToString()}
                    |-----------------------------------------------------------
                    |-----------------------------------------------------------
                    | Steering Vector : {steering.ToString()}
                    | enclosureSteeringVector {enclosureSteeringVector.ToString()}
                    | wanderSteeringVector {wanderSteeringVector.ToString()}
                    | avoidObstaclesSteeringVector {avoidObstaclesSteeringVector.ToString()}
                    | clampedSteering {clampedSteering.ToString()}
                    |-----------------------------------------------------------";
            }
        }

        private Vector2 AvoidObstaclesSteering()
        {
            Agent.ObstacleAvoidance.Rotation = Agent.Velocity.Angle();
            var raycasts = Agent.ObstacleAvoidance.GetChildren();
            for (var i = 0; i < raycasts.Count; i++)
            {
                var raycast = (RayCast2D)raycasts[i];
                if (!raycast.IsColliding()) continue;
                var obstacle = (PhysicsBody2D)raycast.GetCollider();
                return (Agent.Position + Agent.Velocity - obstacle.Position).Normalized() * Agent.AvoidForce;
            }

            return Vector2.Zero;
        }
    }
}