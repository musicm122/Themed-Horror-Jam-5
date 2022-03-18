using Godot;
using System;

public class Level1 : Node2D
{
    [Export]
    public NodePath PatrolPath { get; set; }

    [Export]
    public NodePath PatrolPathFollow2D { get; set; }

    public Path2D Path { get; set; }

    public PathFollow2D PathFollow2D { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()  
    {
        if (PatrolPath != null)
        {
            Path = (Path2D)GetNode(PatrolPath);
        }
        else
        {
            GD.PushWarning("PatrolPath could not be found");
        }
        if (PatrolPathFollow2D != null)
        {
            PathFollow2D = (PathFollow2D)GetNode(PatrolPathFollow2D);
        }
        else
        {
            GD.PushWarning("PatrolPathFollow2D could not be found");
        }
    }
    public Vector2 GetPathDirection(Vector2 position)
    {
        var offset = Path.Curve.GetClosestOffset(position);
        PathFollow2D.Offset = offset;
        return PathFollow2D.Transform.x;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //
    //  }
}