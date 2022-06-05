using System;
using Godot;
using ThemedHorrorJam5.Scripts.GDUtils;

namespace ThemedHorrorJam5.Entities.Components
{
    public class PlayerMovableBehavior : BaseMovableBehavior
    {
        public override Vector2 GetMovementSpeed(bool isRunning, Vector2 direction) =>
                isRunning ?
                    InputUtils.GetTopDownWithDiagMovementInput(MoveSpeed * MoveMultiplier) :
                    InputUtils.GetTopDownWithDiagMovementInput(MoveSpeed);

        public override void _PhysicsProcess(float delta)
        {
            if (CanMove)
            {
                IsRunning = Input.IsActionPressed(InputAction.Run);
                var movement = GetMovementSpeed(IsRunning, Velocity);
                MoveAndSlide(movement);
                if (GetSlideCount() > 0)
                {
                    this.HandleMovableObstacleCollision(movement);
                }
            }
        }
    }
}