using System;
using Godot;
using ThemedHorrorJam5.Scripts.GDUtils;
using ThemedHorrorJam5.Scripts.ItemComponents;

namespace ThemedHorrorJam5.Entities.Components
{

    public interface IMovableBehavior
    {
        float MoveSpeed { get; set; }
        bool IsDebugging { get; set; }
        float PushSpeed { get; set; }
        float MoveMultiplier { get; set; }
        
        Vector2 Velocity { get; set; }
        
        KinematicBody2D MovableTarget { get; set; }
        Vector2 GetMovementSpeed(bool isRunning, Vector2 direction);
        void Init(KinematicBody2D movableTarget);
        bool IsDebugPrintEnabled();
        KinematicCollision2D MoveAndCollide(Vector2 force);
        Vector2 MoveAndSlide(Vector2 force);
        
        int GetSlideCount();

        void _PhysicsProcess(float delta);
    }
}