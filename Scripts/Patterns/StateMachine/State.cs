using ThemedHorrorJam5.Scripts.Patterns.Logger;

namespace ThemedHorrorJam5.Scripts.Patterns.StateMachine
{
    // A single state.
    [System.Serializable]
    public class State
    {
        public ILogger Logger { get;set; } = new GDLogger();

        // The state's visible name. Also used to identify the state to the
        // state machine.
        public string Name;

        // Called every frame while the state is active.
        public System.Action<float> OnFrame;

        // Called when the state is transitioned to from another state.
        public System.Action OnEnter;

        // Called when the state is transitioning to another state.
        public System.Action OnExit;

        public override string ToString()
        {
            return Name;
        }
    }
}