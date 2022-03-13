using Godot;

public class Enemy1 : KinematicBody2D
{
    public EnemyBehaviorStates CurrentState { get; set; } = EnemyBehaviorStates.Idle;

    [Export]
    public float MoveSpeed { get; set; } = 50f;

    [Export]
    public float MoveMultiplier { get; set; } = 1.5f;

    public Path2D Path { get; set; }

    public Player player { get; set; }

    [Export]
    public NodePath PatrolPath { get; set; }

    public bool CanMove = true;
    public bool IsRunning = false;
    public Vector2 Velocity { get; set; } = Vector2.Zero;

    private Vector2[] PatrolPoints { get; set; }
    private int PatrolIndex { get; set; } = 0;

    public override void _Ready()
    {
        if (PatrolPath != null)
        {
            //patrol_points = get_node(patrol_path).curve.get_baked_points()
            Path = (Path2D)GetNode(PatrolPath);
            PatrolPoints = Path.Curve.GetBakedPoints();
            CurrentState = EnemyBehaviorStates.Patrol;
        }
    }

    private void Patrol()
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

    private void ChasePlayer()
    {
        if (player != null)
        {
            Velocity = Position.DirectionTo(player.Position) * MoveSpeed;
            Velocity = MoveAndSlide(Velocity);
        }
        else
        {
            CurrentState = EnemyBehaviorStates.Patrol;
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        this.Velocity = Vector2.Zero;
        switch (CurrentState)
        {
            case EnemyBehaviorStates.Patrol:
                Patrol();
                break;

            case EnemyBehaviorStates.ChasePlayer:
                ChasePlayer();
                break;

            case EnemyBehaviorStates.Idle:
            default:
                this.Velocity = Vector2.Zero;
                break;
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