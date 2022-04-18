using Godot;
using System.Collections.Generic;
using ThemedHorrorJam5.Entities.Components;
using ThemedHorrorJam5.Scripts.Enum;
using ThemedHorrorJam5.Scripts.Extensions;
using ThemedHorrorJam5.Scripts.ItemComponents;

namespace ThemedHorrorJam5.Entities
{
    public class Enemy3 : KinematicBody2D, IDebuggable<Node>
    {
        [Export]
        public bool IsDebugging { get; set; } = false;

        [Export]
        public EnemyBehaviorStates AggroBehavior = EnemyBehaviorStates.RangeAttackBehavior;

        //public Bullet Bullet { get; set; }

        public bool IsDebugPrintEnabled() => IsDebugging;

        public HitBox HitBox { get; set; }

        [Export]
        public float FireRange { get; set; }


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

        public Player Player { get; set; }

        public Line2D Line { get; set; }

        public bool IsRunning = false;

        public Vector2 Velocity { get; set; } = Vector2.Zero;

        private Vector2[] PatrolPoints { get; set; }
        private int PatrolIndex { get; set; } = 0;

        public override void _Ready()
        {
            Cooldown = GetNode<Label>("Cooldown");
            VisionRadius = GetNode<Area2D>("VisionRadius");
            this.HitBox = GetNode<HitBox>("HitBox");
            //this.Bullet = GetNode<Bullet>("Bullet");
            VisionRadius.ConnectBodyEntered(this, nameof(OnVisionRadiusBodyEntered));
            VisionRadius.ConnectBodyExited(this, nameof(OnVisionRadiusBodyExit));
            if (PatrolPath != null)
            {
                Path = GetNode<Path2D>(PatrolPath);
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
                PatrolIndex = Mathf.Wrap(PatrolIndex + 1, 0, PatrolPoints.Length);
                target = PatrolPoints[PatrolIndex];
            }
            Velocity = (target - Position).Normalized() * MoveSpeed;
            Velocity = MoveAndSlide(Velocity);
        }

        private void ChasePlayer(float delta)
        {
            if (Player == null)
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
                    NavPath = new Stack<Vector2>(Navigation2D.GetSimplePath(Position, Player.Position));

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
                    this.Print("Navigation2D not found");
                }
                if (CurrentCoolDownCounter > 0)
                {
                    CurrentCoolDownCounter -= delta;
                }
                if (CurrentCoolDownCounter <= 0f)
                {
                    CurrentState = EnemyBehaviorStates.Patrol;
                    Player = null;
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

            }
        }

        private bool IsPlayerInSight()
        {
            var bodies = VisionRadius.GetOverlappingBodies();
            if (bodies == null || bodies.Count == 0) return false;
            for (int i = 0; i < bodies.Count; i++)
            {
                var body = (Node)bodies[i];
                if (body.Name == "Player")
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsPlayerInShootingRange(Player player) =>
             this.Position.DistanceTo(player.Position) >= this.FireRange;


        public override void _PhysicsProcess(float delta)
        {
            Velocity = Vector2.Zero;
            if (!Enable) return;

            switch (CurrentState)
            {
                case EnemyBehaviorStates.Patrol:
                    Patrol();
                    break;

                case EnemyBehaviorStates.MeleeAttackBehavior:
                case EnemyBehaviorStates.ChasePlayer:
                    ChasePlayer(delta);
                    break;

                case EnemyBehaviorStates.RangeAttackBehavior:
                    Shoot();
                    break;
                case EnemyBehaviorStates.Idle:
                default:
                    Velocity = Vector2.Zero;
                    break;
            }
        }

        public void Shoot()
        {
            //todo: implement shooting
            //var instance = this.Bullet.Instance
        }

        private void OnVisionRadiusBodyEntered(Node body)
        {
            if (body.Name == "Player")
            {
                this.PrintCaller();
                Player = (Player)body;
                CurrentState = AggroBehavior;
                CurrentCoolDownCounter = MaxCoolDownTime;
            }
        }

        private void OnVisionRadiusBodyExit(Node body)
        {
            if (body.Name == "Player")
            {
                this.PrintCaller();
                CurrentCoolDownCounter = MaxCoolDownTime;
                //this.player = null;
                //this.CurrentState = EnemyBehaviorStates.Patrol;
            }
        }

        public void OnExaminablePlayerInteracting()
        {
            this.PrintCaller();
            LockMovement();
        }

        public void OnExaminablePlayerInteractingComplete()
        {
            this.PrintCaller();
            UnlockMovement();
        }

        private void LockMovement()
        {
            this.PrintCaller();
            CanMove = false;
        }

        private void UnlockMovement()
        {
            this.PrintCaller();
            CanMove = true;
        }


    }
}