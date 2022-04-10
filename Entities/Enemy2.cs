using Godot;
using ThemedHorrorJam5.Scripts.GDUtils;
using ThemedHorrorJam5.Scripts.ItemComponents;

namespace ThemedHorrorJam5.Entities
{

    /// <summary>
    /// This is a wip port of this:
    /// https://kidscancode.org/godot_recipes/ai/context_map/
    /// </summary>
    public class Enemy2 : KinematicBody2D, IDebuggable<Node>
    {
        [Export]
        public bool IsDebugging { get; set; }

        public bool IsDebugPrintEnabled() => IsDebugging;

        [Export]
        public bool Enable { get; set; }

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

        [Export]
        public float LookAhead = 100;

        [Export]
        public int RayCount = 16;

        private Vector2[] RayDirections;
        private float[] Interest;
        private float[] Danger;

        private Vector2 ChosenDirection = Vector2.Zero;
        private Vector2 Velocity = Vector2.Zero;

        public EnemyBehaviorStates CurrentState { get; set; } = EnemyBehaviorStates.Idle;

        public Player Player { get; set; }

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
            Interest = new float[RayCount];
            Danger = new float[RayCount];
            RayDirections = new Vector2[RayCount];
            for (int i = 0; i < RayCount; i++)
            {
                //(2 * Mathf.Pi)/ RayCount
                var angle = i * 2 * Mathf.Pi / RayCount;
                //var angle = degrees * i;
                //var angle = degrees / (i + 1);
                //RayDirections[i] = Vector2.Zero.Rotated(angleToDrawAt);
                //RayDirections[i] = Vector2.Right.Rotated(Mathf.Deg2Rad(angleToDrawAt));

                //var angle = i * 2 * PI / num_rays

                //ray_directions[i] = Vector2.RIGHT.rotated(angle)

                RayDirections[i] = Vector2.Right.Rotated(angle * 180 / Mathf.Pi);
                //RayDirections[i] = Vector2.Left.Rotated(Mathf.Deg2Rad(angle * 180 / Mathf.Pi));
                //RayDirections[i] = Vector2.Right.Rotated(Mathf.Deg2Rad(angle * 180 / Mathf.Pi));
            }
        }

        /// <summary>
        /// Set interest in each slot based on world direction. If no world path, use default interest.
        /// </summary>
        private void SetInterest()
        {
            //Set interest in each slot based on world direction
            if (Owner?.HasMethod("GetPathDirection") == true)
            {
                var pathDir = (Vector2)Owner.Call("GetPathDirection", Position);
                for (int i = 0; i < RayCount; i++)
                {
                    float direction;
                    if (Player != null)
                    {
                        direction = RayDirections[i].Dot(Player.Position);
                    }
                    else
                    {
                        //direction = RayDirections[i].Rotated(Rotation).Dot(pathDir);
                        direction = RayDirections[i].Dot(pathDir);
                    }
                    if (direction > 0)
                    {
                        GD.Print("Interest = ", direction);
                    }
                    Interest[i] = direction;

                    //Interest[i] = Mathf.Max(0f, direction);

                    //if (!firstPass)
                    //{
                    //    GD.PrintT($"Interest[{i}]= ", Interest[i]);
                    //}
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
                if (Player != null)
                {
                    direction = RayDirections[i].Rotated(Rotation).Dot(Player.Position);
                }
                Interest[i] = Mathf.Max(0f, direction);
            }
        }

        private void SetDanger()
        {
            var spaceState = GetWorld2d().DirectSpaceState;
            for (int i = 0; i < RayCount; i++)
            {
                var to = Position + (RayDirections[i].Rotated(Rotation) * LookAhead);
                var except = new Godot.Collections.Array(this, Player);
                var result = spaceState.IntersectRay(Position, to, except);
                Danger[i] = result != null ? 1f : 0.0f;
            }
        }

        private void ChooseDirection()
        {
            ChosenDirection = Vector2.Zero;

            for (int i = 0; i < RayCount; i++)
            {
                if (Danger[i] > 0.0f)
                {
                    Interest[i] = 0.0f;
                }
            }
            for (int i = 0; i < RayCount; i++)
            {
                //GD.Print($"RayDirection[{i}]:{RayDirections[i]} * Interest[{i}]:{Interest[i]}=", RayDirections[i] * Interest[i]);
                ChosenDirection += RayDirections[i] * Interest[i];
                GD.Print($"ChosenDirection ", ChosenDirection);
            }

            ChosenDirection = ChosenDirection.Normalized();
        }

        private float Degree { get; } = 360;

        public override void _PhysicsProcess(float delta)
        {
            Velocity = Vector2.Zero;
            if (!Enable) return;
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
            //Rotate(Mathf.Deg2Rad(Degree));
            //Degree--;
            //Degree = (Degree <= 0) ? 360 : Degree;
        }

        public override void _Draw()
        {
            if (IsDebug && RayDirections?.Length > 0)
            {
                for (var i = 0; i < RayDirections.Length; i++)
                {
                    var from = RayDirections[i];
                    var to = Position + (RayDirections[i].Rotated(Rotation) * LookAhead);
                    DrawLine(from, to, new Color(255, 0, 0, 1f));
                }
            }
        }

        private void OnVisionRadiusBodyEntered(Node body)
        {
            if (body.IsPlayer())
            {
                Player = (Player)body;
                CurrentState = EnemyBehaviorStates.ChasePlayer;
            }
        }

        private void OnVisionRadiusBodyExit(Node body)
        {
            if (body.IsPlayer())
            {
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