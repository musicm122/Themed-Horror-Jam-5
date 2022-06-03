using Godot;
using ThemedHorrorJam5.Scripts.Enum;
using ThemedHorrorJam5.Scripts.Patterns.StateMachine;
using ThemedHorrorJam5.Scripts.GDUtils;
using ThemedHorrorJam5.Entities.Components;

namespace ThemedHorrorJam5.Entities
{
    public class PatrolEnemyState : State
    {
        private EnemyV4 Enemy { get; set; }

        private EnemyStatus Status => this.Enemy.Status;

        private EnemyMovableBehavior Movable => this.Enemy.Movable;

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
            if (Status.PatrolPath == null || !Movable.CanMove) return;

            var target = Status.PatrolPoints[Status.PatrolIndex];

            if (Enemy.Position.DistanceTo(target) <= 1)
            {
                Status.PatrolIndex = Mathf.Wrap(Status.PatrolIndex + 1, 0, Status.PatrolPoints.Length);
                target = Status.PatrolPoints[Status.PatrolIndex];
            }

            Movable.Velocity = (target - Enemy.Position).Normalized() * Movable.MoveSpeed;

            if (Movable.GetSlideCount() > 0)
            {
                Movable.HandleMovableObstacleCollision(Movable.Velocity);
            }

            Movable.Velocity = Movable.MoveAndSlide(Movable.Velocity);
            Status.VisionManager.UpdateFacingDirection(Movable.Velocity);
        }
    }
}