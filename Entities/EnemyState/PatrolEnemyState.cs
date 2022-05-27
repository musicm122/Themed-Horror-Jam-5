using Godot;
using ThemedHorrorJam5.Scripts.Enum;
using ThemedHorrorJam5.Scripts.Patterns.StateMachine;
using ThemedHorrorJam5.Scripts.GDUtils;

namespace ThemedHorrorJam5.Entities
{
    public class PatrolEnemyState : State
    {
        private EnemyV4 Enemy { get; set; }

        public PatrolEnemyState(EnemyV4 enemy)
        {
            this.Name = EnemyBehaviorStates.Idle.GetDescription();
            Enemy = enemy;
            this.OnEnter += () => this.Logger.Debug("IdleEnemyState OnEnter called");
            this.OnExit += () => this.Logger.Debug("IdleEnemyState Exit called");
            this.OnFrame += Patrol;
        }
        
        private void Patrol(float delta)
        {
            if (Enemy.Status.PatrolPath == null || !Enemy.Movable.CanMove) return;
            var target = Enemy.Status.PatrolPoints[Enemy.Status.PatrolIndex];
            if (Enemy.Movable.Position.DistanceTo(target) <= 1)
            {
                Enemy.Status.PatrolIndex = Mathf.Wrap(Enemy.Status.PatrolIndex + 1, 0, Enemy.Status.PatrolPoints.Length);
                target = Enemy.Status.PatrolPoints[Enemy.Status.PatrolIndex];
            }
            Enemy.Movable.Velocity = (target - Enemy.Movable.Position).Normalized() * Enemy.Movable.MoveSpeed;
            if (Enemy.Movable.GetSlideCount() > 0)
            {
                Enemy.Movable.HandleMovableObstacleCollision(Enemy.Movable.Velocity);
            }

            Enemy.Movable.Velocity = Enemy.Movable.MoveAndSlide(Enemy.Movable.Velocity);

            Enemy.Status.UpdateVisionConeLocation(Enemy.Movable.Velocity);
        }
    }
}