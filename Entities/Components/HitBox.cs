using Godot;
using ThemedHorrorJam5.Scripts.ItemComponents;

namespace ThemedHorrorJam5.Entities.Components
{
    public class HitBox : Area2D, IDebuggable<Node>
    {
        [Export]
        public bool IsDebugging { get; set; } = false;

        public bool IsDebugPrintEnabled() => IsDebugging;

        [Export]
        public int Damage { get; set; } = 1;

    }
}