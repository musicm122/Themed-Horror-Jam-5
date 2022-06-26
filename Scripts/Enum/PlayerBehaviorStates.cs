using System.ComponentModel;

namespace ThemedHorrorJam5.Scripts.Enum
{
    public enum PlayerBehaviorStates
    {
        [Description("Idle")] Idle = 0,
        [Description("Walk")] Walk = 1,
        [Description("Run")] Run,
        [Description("Roll")] Roll,
        [Description("Sneak")] Sneak,
        [Description("Stun")] Stun,
        [Description("Dead")] Dead
    }
}