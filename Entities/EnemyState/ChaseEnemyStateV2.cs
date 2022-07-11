using System.Collections.Generic;
using Godot;
using ThemedHorrorJam5.Scripts.Enum;
using ThemedHorrorJam5.Scripts.Extensions;
using ThemedHorrorJam5.Scripts.GDUtils;
using ThemedHorrorJam5.Scripts.Patterns.StateMachine;

namespace ThemedHorrorJam5.Entities.EnemyState;

public class ChaseEnemyStateV2 : State
{
    private float Threshold { get; set; } = 16f;
    private EnemyV4 Enemy { get; }
    private PlayerV2 PlayerRef { get; }


    private Stack<Vector2> Paths { get; set; } = new Stack<Vector2>();

    public ChaseEnemyStateV2(EnemyV4 enemy)
    {
        this.Name = EnemyBehaviorStates.ChasePlayer.GetDescription();
        Enemy = enemy;
        (var hasPlayer, PlayerRef) = Enemy.GetTree().GetPlayerNode();
        if (hasPlayer)
        {
            Paths = GetTargetPath(PlayerRef.GlobalPosition);
        }

        this.OnEnter += OnEnterState;
        this.OnExit += OnExitState;
        this.OnFrame += OnPhysicsProcess;
    }

    private void OnEnterState()
    {
        this.Logger.Debug("ChaseEnemyState OnEnter called");
    }

    private void OnExitState()
    {
        this.Logger.Debug("ChaseEnemyState Exit called");
    }

    void OnPhysicsProcess(float delta)
    {
        if (Paths.Count > 0)
        {
            MoveToTarget(delta);
        }
    }

    private void MoveToTarget(float delta)
    {
        if (Enemy.GlobalPosition.DistanceTo(Paths.Peek()) < Threshold)
        {
            Paths.Pop();
        }

        var dir = Enemy.GlobalPosition.DirectionTo(Paths.Peek());
        Enemy.Velocity = dir * Enemy.MaxSpeed;
        Enemy.Move(delta);
    }

    private Stack<Vector2> GetTargetPath(Vector2 targetPosition)
    {
        var temp = new Stack<Vector2>(this.Nav.GetSimplePath(Enemy.GlobalPosition, targetPosition, false));
        return temp;
    }
}