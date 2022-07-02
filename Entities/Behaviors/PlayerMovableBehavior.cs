using System;
using Godot;
using ThemedHorrorJam5.Entities.Components;
using ThemedHorrorJam5.Scripts.GDUtils;
using ThemedHorrorJam5.Scripts.Patterns.Logger;

namespace ThemedHorrorJam5.Entities.Behaviors
{
    public class PlayerMovableBehavior : BaseMovableBehavior
    {
        public Action<Vector2> OnRoll { get; set; }
        public Action<Vector2> OnPhysicsProcessMovement { get; set; }
        
        [Export] public float RollSpeed { get; set; } = 120f;
        
        private Vector2 RollVector { get; set; } = Vector2.Down;
        

        public override void _Ready()
        {
            _logger.Level = LogLevelOutput.Debug;
            base._Ready();
        }

        private Vector2 Roll()
        {
            var newVelocity = RollVector * RollSpeed;
            OnRoll?.Invoke(newVelocity);
            return newVelocity;
        }
        
        public override void Move(float currentVelocity)
        {
            MoveAndSlide(Velocity);
        }

        private Vector2 MoveCheck(Vector2 movementVector, Vector2 currentVelocity, float delta)
        {
            if (movementVector != Vector2.Zero)
            {
                RollVector = movementVector;
                OnPhysicsProcessMovement?.Invoke(movementVector);
                OnMove?.Invoke(movementVector, delta);
                currentVelocity = IsRunning
                    ? currentVelocity.MoveToward(movementVector * (MaxSpeed * MoveMultiplier), Acceleration * delta)
                    : currentVelocity.MoveToward(movementVector * MaxSpeed, Acceleration * delta);
            }
            else
            {
                currentVelocity = currentVelocity.MoveToward(Vector2.Zero, Friction * delta);
                OnIdle?.Invoke(currentVelocity, delta);
                
            }
            return currentVelocity;
        }
        
        
        public override void _PhysicsProcess(float delta)
        {
            if (!CanMove) return;
            IsRunning = Input.IsActionPressed(InputAction.Run);
            
            var movementVector = InputUtils.GetTopDownWithDiagMovementInputStrengthVector();
            Velocity = MoveCheck(movementVector, Velocity, delta);
            if (Input.IsActionPressed(InputAction.Roll))
            {
                Velocity = Roll();
            }

            Move(delta);
            if (GetSlideCount() > 0)
            {
                this.HandleMovableObstacleCollision(Velocity);
            }
        }
    }
}