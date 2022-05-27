using Godot;
using ThemedHorrorJam5.Scripts.Patterns.Logger;

namespace ThemedHorrorJam5.Entities
{
    public class PushBlock : KinematicBody2D, IDebuggable<Node>
    {
        [Export]
        public bool IsDebugging { get; set; }

        [Export]
        public bool CanBePushed {get;set;} = true;

        public bool IsDebugPrintEnabled() => IsDebugging;

        public void Push(Vector2 velocity)
        {
            MoveAndSlide(velocity);
        }
    }
}