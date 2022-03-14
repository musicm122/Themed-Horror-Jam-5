using Godot;

/// <summary>
/// This is a wip port of this:
/// https://kidscancode.org/godot_recipes/ai/context_map/
/// </summary>
public class Enemy2 : KinematicBody2D
{
    private bool firstPass = false;

    [Export]
    public bool IsDebug { get; set; } = false;

    [Export]
    public float MoveSpeed { get; set; } = 50f;

    [Export()]
    public float MoveMultiplier { get; set; } = 1.5f;

    [Export]
    public NodePath PatrolPath { get; set; }

    [Export]
    public NodePath PatrolPathFollow2D { get; set; }

    public Path2D Path { get; set; }

    public PathFollow2D PathFollow2D { get; set; }

    public float SteerForce = 1f;
    public float LookAhead = 100;
    public int RayCount = 16;

    private Vector2[] RayDirections;
    private float[] Interest;
    private float[] Danger;

    private Vector2 ChosenDirection = Vector2.Zero;
    private Vector2 Velocity = Vector2.Zero;

    public EnemyBehaviorStates CurrentState { get; set; } = EnemyBehaviorStates.Idle;

    public Player player { get; set; }

    public bool CanMove = true;

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
        //Path = (Path2D)GetNode("Enemy2_Path2D");
        //PathFollow2D = (PathFollow2D)GetNode("Enemy2_Path2D/Enemy2_PathFollow2D");
        Interest = new float[RayCount];
        Danger = new float[RayCount];
        RayDirections = new Vector2[RayCount];
        for (int i = 0; i < RayCount; i++)
        {
            var angle = i * 2 * Mathf.Pi / RayCount;
            RayDirections[i] = Vector2.Right.Rotated(angle);
        }
    }

    /// <summary>
    /// Set interest in each slot based on world direction. If no world path, use default interest.
    /// </summary>
    private void SetInterest()
    {
        //Set interest in each slot based on world direction
        if (Owner != null && Owner.HasMethod("GetPathDirection"))
        {
            var pathDir = (Vector2)Owner.Call("GetPathDirection", Position);
            for (int i = 0; i < RayCount; i++)
            {
                var direction = RayDirections[i].Rotated(Rotation).Dot(pathDir);
                Interest[i] = Mathf.Max(0f, direction);

                if (!firstPass)
                {
                    GD.PrintT($"Interest[{i}]= ", Interest[i]);
                }
            }
            if (!firstPass)
            {
                firstPass = true;
            }
        }
        else
        {
            //If no world path, use default interest
            SetDefaultInterest();
        }
    }

    private void SetDefaultInterest()
    {
        GD.Print("Using SetDefaultInterest");
        for (int i = 0; i < RayCount; i++)
        {
            var direction = RayDirections[i].Rotated(Rotation).Dot(Transform.x);
            Interest[i] = Mathf.Max(0f, direction);
        }
    }

    private void SetDanger()
    {
        var spaceState = GetWorld2d().DirectSpaceState;
        for (int i = 0; i < RayCount; i++)
        {
            var to = Position + RayDirections[i].Rotated(Rotation) * LookAhead;
            var except = new Godot.Collections.Array(this);
            var result = spaceState.IntersectRay(Position, to, except);
            Danger[i] = result != null ? 1f : 0.0f;
        }
    }

    private void ChooseDirection()
    {
        for (int i = 0; i < RayCount; i++)
        {
            if (Danger[i] > 0.0f)
            {
                Interest[i] = 0.0f;
            }
        }
        ChosenDirection = Vector2.Zero;
        for (int i = 0; i < RayCount; i++)
        {
            ChosenDirection += RayDirections[i] * Interest[i];
        }
        ChosenDirection = ChosenDirection.Normalized();
    }

    public override void _PhysicsProcess(float delta)
    {
        this.Velocity = Vector2.Zero;
        SetInterest();

        SetDanger();
        ChooseDirection();
        var desiredVelocity = ChosenDirection.Rotated(Rotation) * MoveSpeed;
        if (desiredVelocity != Vector2.Zero)
        {
            GD.PrintT("desiredVelocity =", desiredVelocity);
        }
        Velocity = Velocity.LinearInterpolate(desiredVelocity, SteerForce);
        Rotation = Velocity.Angle();

        MoveAndCollide(Velocity * delta);
    }

    public override void _Draw()
    {
        if (IsDebug && RayDirections != null && RayDirections.Length > 0)
        {
            for (var i = 0; i < RayDirections.Length; i++)
            {
                var from = RayDirections[i];
                var to = Position + RayDirections[0].Rotated(Rotation) * LookAhead;

                DrawLine(from, to, new Color(255, 255, 255, 1f));
            }
        }
    }

    private void OnVisionRadiusBodyEntered(Node body)
    {
        if (body.Name.ToLower() == "player")
        {
            this.player = (Player)body;
            this.CurrentState = EnemyBehaviorStates.ChasePlayer;
        }
    }

    private void OnVisionRadiusBodyExit(Node body)
    {
        if (body.Name.ToLower() == "player")
        {
            this.player = null;
            this.CurrentState = EnemyBehaviorStates.Patrol;
        }
    }
}