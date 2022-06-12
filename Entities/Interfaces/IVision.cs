using Godot;
using System;
using ThemedHorrorJam5.Scripts.Patterns.Logger;

namespace ThemedHorrorJam5.Entities.Behaviors.Interfaces
{
    public interface IVision
    {
        Action<Node2D> OnTargetSeen { get; set; }
        Action<Node2D> OnTargetOutOfSight { get; set; }
        bool IsDebugging { get; set; }
        Node2D Target { get; set; }

        public bool CanCheckFrame(int interval = 2) => new Random().Next() % interval == 0;

        void UpdateFacingDirection(Vector2 newVelocity);
        
        bool CanSeeTarget();
    }
}
