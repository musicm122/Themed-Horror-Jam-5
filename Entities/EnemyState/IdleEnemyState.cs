using Godot;
using ThemedHorrorJam5.Scripts.Enum;
using ThemedHorrorJam5.Scripts.GDUtils;
using ThemedHorrorJam5.Scripts.Patterns.StateMachine;

namespace ThemedHorrorJam5.Entities.EnemyState
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