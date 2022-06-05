using System.Linq;
using Godot;
using System;
using System.Collections.Generic;
using ThemedHorrorJam5.Entities.Components;
using ThemedHorrorJam5.Scripts.Enum;
using ThemedHorrorJam5.Scripts.Extensions;
using ThemedHorrorJam5.Scripts.Patterns.Logger;
using ThemedHorrorJam5.Scripts.Patterns.StateMachine;
using ThemedHorrorJam5.Scripts.GDUtils;

namespace ThemedHorrorJam5.Entities
{
    public class IdleEnemyState : State
    {
        private EnemyV4 Enemy { get; set; }

        public IdleEnemyState(EnemyV4 enemy)
        {
            this.Name = EnemyBehaviorStates.Idle.GetDescription();
            Enemy = enemy;
            this.OnEnter += () => this.Logger.Debug("IdleEnemyState OnEnter called");
            this.OnExit += () => this.Logger.Debug("IdleEnemyState Exit called");
            this.OnFrame += (delta) => Enemy.Velocity = Vector2.Zero;
        }
    }
}