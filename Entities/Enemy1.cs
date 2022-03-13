using Godot;

public class Enemy1 : KinematicBody2D
{
    [Export]
    public float MoveSpeed { get; set; } = 50f;

    [Export]
    public float MoveMultiplier { get; set; } = 1.5f;

    public Path2D Path { get; set; }

    [Export]
    public NodePath PatrolPath { get; set; }

    public bool CanMove = true;
    public bool IsRunning = false;
    public Vector2 Velocity { get; set; } = Vector2.Zero;

    Vector2[] PatrolPoints { get; set; }
    int PatrolIndex { get; set; } = 0;

    public override void _Ready()
    {
        if (PatrolPath != null) 
        {
            //patrol_points = get_node(patrol_path).curve.get_baked_points()
            Path = (Path2D)GetNode(PatrolPath);
            PatrolPoints = Path.Curve.GetBakedPoints();
        }
    }

    public override void _PhysicsProcess(float delta) 
    {
        if (PatrolPath == null) return;
        var target = PatrolPoints[PatrolIndex];
        if (Position.DistanceTo(target) <= 1) 
        {
            //patrol_index = wrapi(patrol_index + 1, 0, patrol_points.size())
            PatrolIndex = Mathf.Wrap(PatrolIndex + 1, 0, PatrolPoints.Length);
            target = PatrolPoints[PatrolIndex];
        }
        Velocity = (target - Position).Normalized() * MoveSpeed;
        Velocity = MoveAndSlide(Velocity);
    }
}