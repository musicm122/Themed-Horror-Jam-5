using Godot;
using ThemedHorrorJam5.Scripts.GDUtils;
using ThemedHorrorJam5.Scripts.ItemComponents;

namespace ThemedHorrorJam5.Entities.Components
{
    public class MovableBehavior : Node2D, IDebuggable<Node>
    {
        [Export]
        public float MoveSpeed { get; set; } = 50f;

        [Export]
        public bool IsDebugging { get; set; } = false;

        [Export]
        public float PushSpeed { get; set; } = 20f;

        [Export]
        public float MoveMultiplier { get; set; } = 1.5f;

        public bool CanMove = true;

        public bool IsRunning = false;
        
        [Export]
        public KinematicBody2D MovableTarget { get; set; }

        public bool IsDebugPrintEnabled() => IsDebugging;

        private void HandleMovableObstacleCollision(Vector2 motion)
        {
            this.PrintCaller();
            motion = motion.Normalized();

            if (MovableTarget.GetSlideCollision(0).Collider is PushBlock box && box.CanBePushed)
            {
                box.Push(PushSpeed * motion);
            }
        }

        public override void _Ready()
        {

        }

        public virtual Vector2 GetMovementSpeed(bool isRunning = false) =>
                isRunning ?
                    InputUtils.GetTopDownWithDiagMovementInput(MoveSpeed * MoveMultiplier) :
                    InputUtils.GetTopDownWithDiagMovementInput(MoveSpeed);

        public override void _PhysicsProcess(float delta)
        {
            if (CanMove)
            {
                IsRunning = Input.IsActionPressed(InputAction.Run);
                var movement = GetMovementSpeed(IsRunning);
                MovableTarget.MoveAndSlide(movement);
                if (MovableTarget.GetSlideCount() > 0)
                {
                    HandleMovableObstacleCollision(movement);
                }
            }
        }

        public void MoveAndSlide(Vector2 force) => MovableTarget.MoveAndSlide(force);

        public void MoveAndCollide(Vector2 force) => MovableTarget.MoveAndCollide(force);

        public void Init(KinematicBody2D movableTarget)
        {
            this.MovableTarget = movableTarget;
        }
    }
}