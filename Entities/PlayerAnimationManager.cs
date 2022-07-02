using Godot;
using ThemedHorrorJam5.Scripts.Patterns.Logger;

namespace ThemedHorrorJam5.Entities
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class PlayerAnimationManager : Node
    {
        private readonly ILogger _logger = new GDLogger(LogLevelOutput.Warning);
        
        private AnimationTree AnimationTree { get; set; }
        private  AnimationPlayer BlinkAnimationPlayer { get; set; }

        private AnimationNodeStateMachinePlayback StateMachinePlayback { get; set; }

        public void UpdateAnimationBlendPositions(Vector2 movementVector)
        {
            _logger.Debug("UpdateAnimationBlendPositions arg:" + movementVector.ToString());
            UpdateAnimationBlendPosition("Idle", movementVector);
            UpdateAnimationBlendPosition("Walk", movementVector);
            UpdateAnimationBlendPosition("Roll", movementVector);
        }

        private void UpdateAnimationBlendPosition(string animationName, Vector2 movementVector)
        {
            AnimationTree.Set($"parameters/{animationName}/blend_position", movementVector);
        }

        public void PlayRollAnimation(Vector2 currVelocity)
        {
            NavToAnimation("Roll");
        }

        public void PlayIdleAnimation(Vector2 currVelocity)
        {
            NavToAnimation("Idle");
        }

        public void PlayWalkAnimation(Vector2 currVelocity)
        {
            NavToAnimation("Walk");
        }

        private void NavToAnimation(string animationName)
        {
            StateMachinePlayback.Travel(animationName);
        }

        public void StartBlinkAnimation()
        {
            BlinkAnimationPlayer.Play("Start");
        }
        
        public void StopBlinkAnimation()
        {
            BlinkAnimationPlayer.Play("Stop");
        }

        public override void _Ready()
        {
            AnimationTree = GetNode<AnimationTree>("AnimationTree");
            AnimationTree.Active = true;
            StateMachinePlayback = (AnimationNodeStateMachinePlayback)AnimationTree.Get("parameters/playback");
            BlinkAnimationPlayer = GetNode<AnimationPlayer>("BlinkAnimationPlayer");
        }
    }
}