using Godot;
using ThemedHorrorJam5.Scripts.Patterns.Logger;

namespace ThemedHorrorJam5.Entities.Components
{
    public class HitBox : Area2D, IDebuggable<Node>, IHitBox
    {
        [Export]
        public bool IsDebugging { get; set; } = false;

        public bool IsDebugPrintEnabled() => IsDebugging;

        [Export]
        public int Damage { get; set; } = 1;

        [Export]
        public float EffectForce { get; set; } = 50f;
    }
}