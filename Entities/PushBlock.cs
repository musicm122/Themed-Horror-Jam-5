using Godot;
using System;
using ThemedHorrorJam5.Scripts.ItemComponents;

public class PushBlock : KinematicBody2D, IDebuggable<Node>
{
    [Export]
    public bool IsDebugging { get; set; } = false;
    public bool IsDebugPrintEnabled() => IsDebugging;
    public void Push(Vector2 velocity)
    {
        MoveAndSlide(velocity);
    }
}
