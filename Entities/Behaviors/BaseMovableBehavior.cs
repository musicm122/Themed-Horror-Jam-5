using System;
using Godot;
using ThemedHorrorJam5.Scripts.GDUtils;
using ThemedHorrorJam5.Scripts.Patterns.Logger;

namespace ThemedHorrorJam5.Entities.Components
{
    public abstract class BaseMovableBehavior : KinematicBody2D, IDebuggable<Node>, IMovableBehavior
    {
        [Export]
        public float MoveSpeed { get; set; } = 10f;

        [Export]
        public bool IsDebugging { get; set; } = false;

        [Export]
        public float PushSpeed { get; set; } = 20f;

        [Export]
        public float MoveMultiplier { get; set; } = 1.5f;

        public Vector2 Velocity { get; set; }

        public bool CanMove = true;

        public bool IsRunning = false;

        [Export]
        public float MaxSpeed { get; set; } = 10f;


        public bool IsDebugPrintEnabled() => IsDebugging;

        public void HandleMovableObstacleCollision(Vector2 motion)
        {
            this.PrintCaller();
            motion = motion.Normalized();

            if (GetSlideCollision(0).Collider is PushBlock box && box.CanBePushed)
            {
                box.Push(PushSpeed * motion);
            }
        }

        public virtual Vector2 GetMovementSpeed(bool isRunning, Vector2 direction) =>
                isRunning ? direction.Normalized() * MoveSpeed * MoveMultiplier : direction.Normalized() * MoveSpeed;

        public override void _PhysicsProcess(float delta)
        {
            if (CanMove)
            {
                var movement = GetMovementSpeed(IsRunning, Velocity);
                MoveAndSlide(movement);
                if (GetSlideCount() > 0)
                {
                    HandleMovableObstacleCollision(movement);
                }
            }
        }
    }
}