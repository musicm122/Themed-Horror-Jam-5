using Godot;
using System;

public class PushBlock : KinematicBody2D
{
    public void Push(Vector2 velocity) 
    {
        MoveAndSlide(velocity);
    }
    
}
