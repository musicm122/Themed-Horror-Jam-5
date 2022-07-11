using Godot;
using ThemedHorrorJam5.Entities.Components;
using ThemedHorrorJam5.Scripts.Enum;
using ThemedHorrorJam5.Scripts.GDUtils;
using ThemedHorrorJam5.Scripts.Patterns.StateMachine;

namespace ThemedHorrorJam5.Entities.EnemyState
{
    public class PatrolEnemyState : State
    {
        private EnemyV4 Enemy { get; set; }

        private EnemyDataStore DataStore => this.Enemy.EnemyDataStore;

        public PatrolEnemyState(EnemyV4 enemy)
        {
            this.Logger.Level = Scripts.Patterns.Logger.LogLevelOutput.Debug;
            this.Name = EnemyBehaviorStates.Patrol.GetDescription();
            Enemy = enemy;

            this.OnEnter += () => this.Logger.Debug("PatrolEnemyState OnEnter called");
            this.OnExit += () => this.Logger.Debug("PatrolEnemyState Exit called");
            this.OnFrame += Patrol;

            if (DataStore.PatrolPath != null)
            {
                DataStore.Path = Enemy.GetNode<Path2D>(DataStore.PatrolPath);
                DataStore.PatrolPoints = DataStore.Path.Curve.GetBakedPoints();
            }
            else
            {
                DataStore.DebugLabel.Text += "Status.PatrolPath is null";
            }
        }

        private void Patrol(float delta)
        {
            if (DataStore.PatrolPath == null || !Enemy.CanMove) return;

            var target = DataStore.PatrolPoints[DataStore.PatrolIndex];

            if (Enemy.Position.DistanceTo(target) <= 1)
            {
                DataStore.PatrolIndex = Mathf.Wrap(DataStore.PatrolIndex + 1, 0, DataStore.PatrolPoints.Length);
                target = DataStore.PatrolPoints[DataStore.PatrolIndex];
            }

            Enemy.Velocity = (target - Enemy.Position).Normalized() * Enemy.MaxSpeed;

            if (Enemy.GetSlideCount() > 0)
            {
                Enemy.HandleMovableObstacleCollision(Enemy.Velocity);
            }

            Enemy.Velocity = Enemy.MoveAndSlide(Enemy.Velocity);
        }
    }
}