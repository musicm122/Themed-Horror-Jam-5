using Godot;

/// <summary>
/// This was put together using a combination of
/// https://kidscancode.org/godot_recipes/ai/path_follow/
/// https://kidscancode.org/godot_recipes/ai/chase/
/// https://kidscancode.org/godot_recipes/ai/changing_behaviors/
/// </summary>
public class Enemy1 : KinematicBody2D
{
    [Export]
    public float MoveSpeed { get; set; } = 50f;

    [Export]
    public float MoveMultiplier { get; set; } = 1.5f;

    [Export]
    public NodePath PatrolPath { get; set; }

    public EnemyBehaviorStates CurrentState { get; set; } = EnemyBehaviorStates.Idle;

    public Path2D Path { get; set; }

    public Player player { get; set; }

    public bool CanMove = true;

    public bool IsRunning = false;

    public Vector2 Velocity { get; set; } = Vector2.Zero;

    private Vector2[] PatrolPoints { get; set; }
    private int PatrolIndex { get; set; } = 0;

    public override void _Ready()
    {
        if (PatrolPath != null)
        {
            Path = (Path2D)GetNode(PatrolPath);
            PatrolPoints = Path.Curve.GetBakedPoints();
            CurrentState = EnemyBehaviorStates.Patrol;
        }
        else
        {
            CurrentState = EnemyBehaviorStates.Idle;
        }
    }

    private void Patrol()
    {
        if (PatrolPath == null || !CanMove) return;
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
        if (player == null || !CanMove)
        {
            CurrentState = EnemyBehaviorStates.Idle;
        }
        else
        {
            Velocity = Position.DirectionTo(player.Position) * MoveSpeed;
            Velocity = MoveAndSlide(Velocity);
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
            GD.Print("OnVisionRadiusBodyEntered");
            this.player = (Player)body;
            this.CurrentState = EnemyBehaviorStates.ChasePlayer;
        }
    }

    private void OnVisionRadiusBodyExit(Node body)
    {
        if (body.Name.ToLower() == "player")
        {
            GD.Print("OnVisionRadiusBodyExit");
            this.player = null;
            this.CurrentState = EnemyBehaviorStates.Patrol;
        }
    }
}