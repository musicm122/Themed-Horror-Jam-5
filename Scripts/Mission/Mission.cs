﻿using System;

public abstract class Mission
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