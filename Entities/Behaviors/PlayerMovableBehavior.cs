using Godot;
using ThemedHorrorJam5.Entities.Components;
using ThemedHorrorJam5.Scripts.GDUtils;

namespace ThemedHorrorJam5.Entities.Behaviors
{
    public class PlayerMovableBehavior : BaseMovableBehavior
    {
        [Export] public float RollSpeed { get; set; } = 120f;
        
        private Vector2 RollVector { get; set; } = Vector2.Down;
        
        public Vector2 Velocity { get; set; } = Vector2.Zero;

        private AnimationTree AnimationTree { get; set; }

        private AnimationNodeStateMachinePlayback StateMachinePlayback { get; set; }


        public override void _Ready()
        {
            base._Ready();
            AnimationTree = GetNode<AnimationTree>("AnimationTree");
            AnimationTree.Active = true;
            StateMachinePlayback = (AnimationNodeStateMachinePlayback)AnimationTree.Get("parameters/playback");
        }

        private Vector2 MoveCheck(Vector2 movementVector, Vector2 currentVelocity, float delta)
        {
            if (movementVector != Vector2.Zero)
            {
                RollVector = movementVector;
                
                StateMachinePlayback.Travel("Walk");
                currentVelocity = IsRunning
                    ? currentVelocity.MoveToward(movementVector * (MaxSpeed * MoveMultiplier), Acceleration * delta)
                    : currentVelocity.MoveToward(movementVector * MaxSpeed, Acceleration * delta);
            }
            else
            {
                currentVelocity = currentVelocity.MoveToward(Vector2.Zero, Friction * delta);
                StateMachinePlayback.Travel("Idle");
            }
            return currentVelocity;
        }
        
        private Vector2 Roll(Vector2 currentVelocity)
        {
            var newVelocity = RollVector * RollSpeed;
            StateMachinePlayback?.Travel("Roll");
            return newVelocity;
        }

        private void Move(Vector2 currentVelocity)
        {
            MoveAndSlide(currentVelocity);
        }

        private void SetAnimationBlendPosition(Vector2 movementVector)
        {
            AnimationTree.Set("parameters/Idle/blend_position", movementVector);
            AnimationTree.Set("parameters/Walk/blend_position", movementVector);
            AnimationTree.Set("parameters/Roll/blend_position", movementVector);
        }
        
        public override void _PhysicsProcess(float delta)
        {
            if (!CanMove || AnimationTree == null) return;
            IsRunning = Input.IsActionPressed(InputAction.Run);
            
            var movementVector = InputUtils.GetTopDownWithDiagMovementInputStrengthVector();
            SetAnimationBlendPosition(movementVector);
            Velocity = MoveCheck(movementVector, Velocity, delta);
            if (Input.IsActionPressed(InputAction.Roll))
            {
                Velocity = Roll(Velocity);
            }

            Move(Velocity);
            if (GetSlideCount() > 0)
            {
                this.HandleMovableObstacleCollision(Velocity);
            }
        }
    }
}