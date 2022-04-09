using Godot;
using System.Collections.Generic;
using ThemedHorrorJam5.Scripts.ItemComponents;

public class Enemy3 : KinematicBody2D, IDebuggable<Node>
{
    [Export]
    public bool IsDebugging { get; set; } = false;
    public bool IsDebugPrintEnabled() => IsDebugging;
    
    [Export]
    public bool Enable { get; set; }

    public bool CanMove = true;

    [Export]
    public float MaxCoolDownTime { get; set; } = 10f;

    public Area2D VisionRadius { get; set; }

    private float CurrentCoolDownCounter { get; set; } = 0;

    public Label Cooldown { get; set; }

    [Export]
    public float MoveSpeed { get; set; } = 50f;

    [Export]
    public float MoveMultiplier { get; set; } = 1.5f;

    private Stack<Vector2> NavPath { get; set; }

    public Navigation2D Navigation2D { get; set; }

    [Export]
    public NodePath PatrolPath { get; set; }

    public EnemyBehaviorStates CurrentState { get; set; } = EnemyBehaviorStates.Idle;

    public Path2D Path { get; set; }

    public Player player { get; set; }

    public Line2D Line { get; set; }

    public bool IsRunning = false;

    public Vector2 Velocity { get; set; } = Vector2.Zero;

    private Vector2[] PatrolPoints { get; set; }
    private int PatrolIndex { get; set; } = 0;

    public override void _Ready()
    {
        Cooldown = (Label)GetNode("Cooldown");
        VisionRadius = (Area2D)GetNode("VisionRadius");
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

    private void ChasePlayer(float delta)
    {
        if (player == null || !CanMove)
        {
            CurrentState = EnemyBehaviorStates.Patrol;
        }
        else
        {
            if (Owner.HasNode("Line2D"))
            {
                Line = (Line2D)Owner.GetNode("Line2D");
            }
            if (Owner.HasNode("Navigation2D"))
            {
                Navigation2D = (Navigation2D)Owner.GetNode("Navigation2D");
                NavPath = new Stack<Vector2>(Navigation2D.GetSimplePath(Position, this.player.Position));

                if (Line != null)
                {
                    Line.Points = NavPath.ToArray();
                }

                var distance_to_walk = MoveSpeed * delta;
                while (distance_to_walk > 0 && NavPath.Count > 0)
                {
                    var distance_to_next_point = Position.DistanceTo(NavPath.Peek());
                    if (distance_to_walk <= distance_to_next_point)
                    {
                        Position += Position.DirectionTo(NavPath.Peek()) * distance_to_walk;
                    }
                    else
                    {
                        Position = NavPath.Pop();
                    }
                    distance_to_walk -= distance_to_next_point;
                }
            }
            else
            {
                GD.Print("Navigation2D not found");
            }
            if (CurrentCoolDownCounter > 0)
            {
                CurrentCoolDownCounter -= delta;
            }
            if (CurrentCoolDownCounter <= 0f)
            {
                CurrentState = EnemyBehaviorStates.Patrol;
                this.player = null;
                Cooldown.Text = "";
            }
            else
            {
                Cooldown.Text = $"Cooling Down in {CurrentCoolDownCounter} seconds";
            }
            if (IsPlayerInSight())
            {
                CurrentCoolDownCounter = MaxCoolDownTime;
            }

            //Velocity = Position.DirectionTo(player.Position) * MoveSpeed;
            //Velocity = MoveAndSlide(Velocity);
        }
    }

    private bool IsPlayerInSight()
    {
        var bodies = VisionRadius.GetOverlappingBodies();
        if (bodies == null || bodies.Count == 0) return false;
        for (int i = 0; i < bodies.Count; i++)
        {
            var body = (Node)bodies[i];
            if (body.Name.ToLower() == "player")
            {
                return true;
            }
        }
        return false;
    }

    public override void _PhysicsProcess(float delta)
    {
        this.Velocity = Vector2.Zero;
        if (Enable == false) return;

        switch (CurrentState)
        {
            case EnemyBehaviorStates.Patrol:
                Patrol();
                break;

            case EnemyBehaviorStates.ChasePlayer:
                ChasePlayer(delta);
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
            CurrentCoolDownCounter = MaxCoolDownTime;
        }
    }

    private void OnVisionRadiusBodyExit(Node body)
    {
        if (body.Name.ToLower() == "player")
        {
            GD.Print("OnVisionRadiusBodyExit");
            CurrentCoolDownCounter = MaxCoolDownTime;
            //this.player = null;
            //this.CurrentState = EnemyBehaviorStates.Patrol;
        }
    }

    public void OnExaminablePlayerInteracting()
    {
        GD.Print($"OnExaminablePlayerInteracting called for {this.Name}");
        this.LockMovement();
    }

    public void OnExaminablePlayerInteractingComplete()
    {
        GD.Print($"OnExaminablePlayerInteractingComplete called for {this.Name}");
        this.UnlockMovement();
    }

    private void LockMovement()
    {
        GD.Print($"Locking Movement for {this.Name}");
        this.CanMove = false;
    }

    private void UnlockMovement()
    {
        GD.Print($"Unlocking Movement for {this.Name}");
        this.CanMove = true;
    }
}