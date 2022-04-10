using Godot;
using ThemedHorrorJam5.Scripts.ItemComponents;

namespace ThemedHorrorJam5.Entities
{

    /// <summary>
    /// This was put together using a combination of
    /// https://kidscancode.org/godot_recipes/ai/path_follow/
    /// https://kidscancode.org/godot_recipes/ai/chase/
    /// https://kidscancode.org/godot_recipes/ai/changing_behaviors/
    /// </summary>
    public class Enemy1 : KinematicBody2D, IDebuggable<Node>
    {
        [Export]
        public bool IsDebugging { get; set; }

        public bool IsDebugPrintEnabled() => IsDebugging;

        [Export]
        public bool Enable { get; set; }

        [Export]
        public float MoveSpeed { get; set; } = 50f;

        [Export]
        public float MoveMultiplier { get; set; } = 1.5f;

        [Export]
        public NodePath PatrolPath { get; set; }

        public EnemyBehaviorStates CurrentState { get; set; } = EnemyBehaviorStates.Idle;

        public Path2D Path { get; set; }

        public Player Player { get; set; }

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
            if (Player == null || !CanMove)
            {
                CurrentState = EnemyBehaviorStates.Idle;
            }
            else
            {
                Velocity = Position.DirectionTo(Player.Position) * MoveSpeed;
                Velocity = MoveAndSlide(Velocity);
            }
        }

        public override void _PhysicsProcess(float delta)
        {
            Velocity = Vector2.Zero;
            if (!Enable) return;
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
                    Velocity = Vector2.Zero;
                    break;
            }
        }

        private void OnVisionRadiusBodyEntered(Node body)
        {
            if (string.Equals(body.Name, "player", System.StringComparison.OrdinalIgnoreCase))
            {
                GD.Print("OnVisionRadiusBodyEntered");
                Player = (Player)body;
                CurrentState = EnemyBehaviorStates.ChasePlayer;
            }
        }

        private void OnVisionRadiusBodyExit(Node body)
        {
            if (string.Equals(body.Name, "player", System.StringComparison.OrdinalIgnoreCase))
            {
                GD.Print("OnVisionRadiusBodyExit");
                Player = null;
                CurrentState = EnemyBehaviorStates.Patrol;
            }
        }

        public void OnExaminablePlayerInteracting()
        {
            GD.Print($"OnExaminablePlayerInteracting called for {Name}");
            LockMovement();
        }

        public void OnExaminablePlayerInteractingComplete()
        {
            GD.Print($"OnExaminablePlayerInteractingComplete called for {Name}");
            UnlockMovement();
        }

        private void LockMovement()
        {
            GD.Print($"Locking Movement for {Name}");
            CanMove = false;
        }

        private void UnlockMovement()
        {
            GD.Print($"Unlocking Movement for {Name}");
            CanMove = true;
        }
    }
}