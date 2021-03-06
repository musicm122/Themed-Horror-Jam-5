using System;
using ThemedHorrorJam5.Entities;

namespace ThemedHorrorJam5.Scripts.Mission
{
    [Serializable]
    public class MissionElement
    {
        public MissionElement() { }

        public MissionElement(string title, string details, bool isComplete = false)
        {
            this.Title = title;
            this.Details = details;
            IsComplete = isComplete;
        }

        public MissionElement(string title, string details, Func<PlayerDataStore, bool> evalCondition, bool isComplete = false)
        {
            this.Title = title;
            this.Details = details;
            IsComplete = isComplete;
            EvaluateCompletionState = evalCondition;
        }

        public bool IsComplete { get; set; } = false;

        public string Title { get; set; }
        public string Details { get; set; }

        public Func<PlayerDataStore, bool> EvaluateCompletionState;

        public override string ToString() =>
            $@"Mission:
        Title: {Title}
        Details: {Details}
        IsComplete: {IsComplete}";
    }
}