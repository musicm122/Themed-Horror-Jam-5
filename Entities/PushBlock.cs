using Godot;
using ThemedHorrorJam5.Scripts.ItemComponents;

public class PushBlock : KinematicBody2D, IDebuggable<Node>
{
    [Export]
    public bool IsDebugging { get; set; }

    public bool IsDebugPrintEnabled() => IsDebugging;

    public void Push(Vector2 velocity)
    {
        MoveAndSlide(velocity);
    }
}