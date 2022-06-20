using Godot;

namespace ThemedHorrorJam5.Entities;

public interface IStateMachineAnimator
{
    void Travel(string animation);
}

public class StateMachineAnimator : AnimationPlayer, IStateMachineAnimator
{
    public void Travel(string animation)
    {
        StateMachinePlayback.Travel(animation);
    }

    private AnimationTree AnimationTree { get; set; }
    private AnimationNodeStateMachinePlayback StateMachinePlayback { get; set; } 
    
    public override void _Ready()
    {
        AnimationTree = GetNode<AnimationTree>("AnimationTree");
        StateMachinePlayback = (AnimationNodeStateMachinePlayback)AnimationTree.Get("parameters/playback");
    }
    
    
}