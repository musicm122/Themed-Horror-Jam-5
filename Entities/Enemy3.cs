using System.Linq;
using Godot;
using System;
using System.Collections.Generic;
using ThemedHorrorJam5.Entities.Components;
using ThemedHorrorJam5.Scripts.Enum;
using ThemedHorrorJam5.Scripts.Extensions;
using ThemedHorrorJam5.Scripts.Patterns.Logger;

namespace ThemedHorrorJam5.Entities
{
    public class Enemy3 : KinematicBody2D, IDebuggable<Node>
    {
        public DamagableBehavior Damagable { get; private set; }
        
        public Status EnemyStatus { get; set; }

        public bool LineOfSight = false;
        public bool CheckThisFrame = false;
        
        [Export]
        public float PushSpeed { get; set; } = 40f;

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

        public Node2D Target { get; set; }

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
            if (GetSlideCount() > 0)
            {
                CheckBoxCollision(Velocity);
            }
            Velocity = MoveAndSlide(Velocity);
            UpdateVisionConeLocation(Velocity);
            

        }

        void UpdateVisionConeLocation(Vector2 newVelocity) 
        {
            if (newVelocity.x < 0) 
            {
                VisionRadius.Scale = new Vector2(-1, VisionRadius.Scale.y);
            }
            else 
            {
                VisionRadius.Scale = new Vector2(1, VisionRadius.Scale.y);
            }
        }

        private void ChasePlayer(float delta)
        {
            if (!LineOfSight)
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
                    NavPath = new Stack<Vector2>(Navigation2D.GetSimplePath(Position, Target.Position));

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
                            var newPosition = Position.DirectionTo(NavPath.Peek()) * distance_to_walk;
                            UpdateVisionConeLocation(newPosition.Normalized());
                            Position += newPosition;
                        }
                        else
                        {
                            var newPosition = NavPath.Pop(); 
                            UpdateVisionConeLocation(newPosition.Normalized());
                            if (GetSlideCount() > 0)
                            {
                                CheckBoxCollision(newPosition);
                            }
                            Position = newPosition;

                            
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
                    Target = null;
                    Cooldown.Text = "";
                }
                else
                {
                    Cooldown.Text = $"Cooling Down in {CurrentCoolDownCounter} seconds";
                }
                //if (IsPlayerInSight())
                //{
                //    CurrentCoolDownCounter = MaxCoolDownTime;
                //}
            }
        }

        private bool IsPlayerInSight()
        {
            var bodies = VisionRadius.GetOverlappingBodies();
            if (bodies == null || bodies.Count == 0) return false;
            for (int i = 0; i < bodies.Count; i++)
            {
                var body = (Node)bodies[i];
                if (body.Name.ToLower().Contains("player"))
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsPlayerInShootingRange(PlayerV2 player) =>
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
            if (body.Name.ToLower().Contains("player"))
            {
                this.PrintCaller();
                Target = (Node2D)body;
                if (HasLineOfSight(Target.Position))
                {
                    CurrentState = AggroBehavior;
                    CurrentCoolDownCounter = MaxCoolDownTime;
                }
            }
        }

        public bool HasLineOfSight(Vector2 point)
        {
            //if (!CanCheckFrame()) return LineOfSight;
            var spaceState = GetWorld2d().DirectSpaceState;
            var result = spaceState.IntersectRay(GlobalTransform.origin, point, null, CollisionMask);
            LineOfSight = result?.Count > 0;
            return LineOfSight;
        }

        private bool CanCheckFrame(int interval = 2)
        {
            Random random = new Random();
            return random.Next() % interval == 0;
        }

        private void OnVisionRadiusBodyExit(Node body)
        {
            if (body.Name.ToLower().Contains("player"))
            {
                Target = (Node2D)body;
                this.PrintCaller();
                CurrentCoolDownCounter = MaxCoolDownTime;
                if (!HasLineOfSight(Target.Position))
                {
                    CurrentState = EnemyBehaviorStates.Patrol;
                    CurrentCoolDownCounter = MaxCoolDownTime;
                }
                else
                {
                    CurrentCoolDownCounter = MaxCoolDownTime;
                }
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

        private void CheckBoxCollision(Vector2 motion)
        {
            this.PrintCaller();
            motion = motion.Normalized();
            if (GetSlideCollision(0).Collider is PushBlock box)
            {
                box.Push(PushSpeed * motion);
            }
        }
    }
}