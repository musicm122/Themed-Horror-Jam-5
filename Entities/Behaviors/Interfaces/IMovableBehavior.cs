using System;
using Godot;
using ThemedHorrorJam5.Scripts.GDUtils;
using ThemedHorrorJam5.Scripts.ItemComponents;

namespace ThemedHorrorJam5.Entities.Components
{
    public interface IMovableBehavior
    {
        float MaxSpeed { get; set; }

        float PushSpeed { get; set; }
        float MoveMultiplier { get; set; }

        Vector2 Velocity { get; set; }

        Vector2 GetMovementVelocity(Vector2 movementVector, Vector2 currentVelocity, float delta);
        Vector2 GetMovementVelocity(Vector2 currentVelocity, float delta);
        void HandleMovableObstacleCollision(Vector2 motion);
        Vector2 GetAcceleration(Vector2 movementVector, Vector2 currentVelocity, float delta);
        Vector2 GetFriction(Vector2 currentVelocity, float delta);
        void Move(float delta);
    }
}