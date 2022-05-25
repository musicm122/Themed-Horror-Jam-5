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
        void MoveAndCollide(Vector2 force);
        void MoveAndSlide(Vector2 force);
        void _PhysicsProcess(float delta);
    }
}