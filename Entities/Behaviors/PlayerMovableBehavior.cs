using Godot;
using ThemedHorrorJam5.Entities.Components;
using ThemedHorrorJam5.Scripts.GDUtils;

namespace ThemedHorrorJam5.Entities.Behaviors
{
    public class PlayerMovableBehavior : BaseMovableBehavior
    {
        private AnimationTree AnimationTree { get; set; }

        private AnimationNodeStateMachinePlayback StateMachinePlayback { get; set; }
        

        public override void _Ready()
        {
            base._Ready();
            AnimationTree = GetNode<AnimationTree>("AnimationTree");
            AnimationTree.Active = true;
            StateMachinePlayback = (AnimationNodeStateMachinePlayback)AnimationTree.Get("parameters/playback");
        }

        public override void _PhysicsProcess(float delta)
        {
            if (!CanMove || AnimationTree == null) return;
            IsRunning = Input.IsActionPressed(InputAction.Run);
            var movementVector = InputUtils.GetTopDownWithDiagMovementInputStrengthVector();
            if (movementVector != Vector2.Zero)
            {
                AnimationTree.Set("parameters/Idle/blend_position", movementVector);
                AnimationTree.Set("parameters/Walk/blend_position", movementVector);
                StateMachinePlayback.Travel("Walk");
                var movementSpeed = IsRunning ? movementVector * MoveMultiplier * MoveSpeed : movementVector * MoveSpeed; 
                MoveAndSlide(movementSpeed);
                if (GetSlideCount() > 0)
                {
                    this.HandleMovableObstacleCollision(movementSpeed);
                }
            }
            else
            {
                StateMachinePlayback.Travel("Idle");
            }
        }
    }
}