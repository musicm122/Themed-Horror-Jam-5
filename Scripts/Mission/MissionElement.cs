using System;
using ThemedHorrorJam5.Entities;

namespace ThemedHorrorJam5.Scripts.Mission
{
    public abstract class MissionElement
    {
        public bool IsComplete { get; set; } = false;

        public string Title { get; set; }
        public string Details { get; set; }

        public Func<Player, bool> EvaluateCompletionState;

        public override string ToString() =>
            $@"Mission:
        Title: {Title}
        Details: {Details}
        IsComplete: {IsComplete}";
    }
}