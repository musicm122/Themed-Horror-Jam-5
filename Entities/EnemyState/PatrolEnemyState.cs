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

        private EnemyStatus Status => this.Enemy.Status;

        public PatrolEnemyState(EnemyV4 enemy)
        {
            this.Logger.Level = Scripts.Patterns.Logger.LogLevelOutput.Debug;
            this.Name = EnemyBehaviorStates.Patrol.GetDescription();
            Enemy = enemy;

            this.OnEnter += () => this.Logger.Debug("PatrolEnemyState OnEnter called");
            this.OnExit += () => this.Logger.Debug("PatrolEnemyState Exit called");
            this.OnFrame += Patrol;

            if (Status.PatrolPath != null)
            {
                Status.Path = Enemy.GetNode<Path2D>(Status.PatrolPath);
                Status.PatrolPoints = Status.Path.Curve.GetBakedPoints();
            }
            else
            {
                Status.DebugLabel.Text += "Status.PatrolPath is null";
            }
        }

        private void Patrol(float delta)
        {
            if (Status.PatrolPath == null || !Enemy.CanMove) return;

            var target = Status.PatrolPoints[Status.PatrolIndex];

            if (Enemy.Position.DistanceTo(target) <= 1)
            {
                Status.PatrolIndex = Mathf.Wrap(Status.PatrolIndex + 1, 0, Status.PatrolPoints.Length);
                target = Status.PatrolPoints[Status.PatrolIndex];
            }

            Enemy.Velocity = (target - Enemy.Position).Normalized() * Enemy.MoveSpeed;

            if (Enemy.GetSlideCount() > 0)
            {
                Enemy.HandleMovableObstacleCollision(Enemy.Velocity);
            }

            Enemy.Velocity = Enemy.MoveAndSlide(Enemy.Velocity);
            Status.VisionManager.UpdateFacingDirection(Enemy.Velocity);
        }
    }
}