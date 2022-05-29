using Godot;
using ThemedHorrorJam5.Scripts.Enum;
using ThemedHorrorJam5.Scripts.Patterns.StateMachine;
using ThemedHorrorJam5.Scripts.GDUtils;
using System.Collections.Generic;

namespace ThemedHorrorJam5.Entities
{
    public class ChaseEnemyState : State
    {
        private EnemyV4 Enemy { get; set; }

        public ChaseEnemyState(EnemyV4 enemy)
        {
            this.Name = EnemyBehaviorStates.ChasePlayer.GetDescription();
            Enemy = enemy;
            this.OnEnter += () => this.Logger.Debug("ChaseEnemyState OnEnter called");
            this.OnExit += () => this.Logger.Debug("ChaseEnemyState Exit called");
            this.OnFrame += ChasePlayer;
        }

        private void ChasePlayer(float delta)
        {

            if (Enemy.Owner.HasNode("Line2D"))
            {
                Enemy.Status.Line = (Line2D)Enemy.Owner.GetNode("Line2D");
            }
            if (Enemy.Owner.HasNode("Navigation2D"))
            {
                Enemy.Status.Navigation2D = (Navigation2D)Enemy.Owner.GetNode("Navigation2D");
                var paths = Enemy.Status.Navigation2D.GetSimplePath(Enemy.Movable.Position, Enemy.Status.Target.Position);

                Enemy.Status.NavPath = new Stack<Vector2>(paths);

                if (Enemy.Status.Line != null)
                {
                    Enemy.Status.Line.Points = Enemy.Status.NavPath.ToArray();
                }

                var distance_to_walk = Enemy.Movable.MoveSpeed * delta;
                while (distance_to_walk > 0 && Enemy.Status.NavPath.Count > 0)
                {
                    var distance_to_next_point = Enemy.Movable.Position.DistanceTo(Enemy.Status.NavPath.Peek());
                    if (distance_to_walk <= distance_to_next_point)
                    {
                        var newPosition = Enemy.Movable.Position.DirectionTo(Enemy.Status.NavPath.Peek()) * distance_to_walk;
                        Enemy.Status.UpdateVisionConeLocation(newPosition.Normalized());
                        Enemy.Movable.Position += newPosition;
                    }
                    else
                    {
                        var newPosition = Enemy.Status.NavPath.Pop();
                        Enemy.Status.UpdateVisionConeLocation(newPosition.Normalized());
                        if (Enemy.Movable.GetSlideCount() > 0)
                        {
                            Enemy.Movable.HandleMovableObstacleCollision(newPosition);
                        }
                        Enemy.Movable.Position = newPosition;
                    }
                    distance_to_walk -= distance_to_next_point;
                }
            }
            else
            {
                Logger.Error("Navigation2D not found");
            }
            if (Enemy.Status.CurrentCoolDownCounter > 0)
            {
                Enemy.Status.CurrentCoolDownCounter -= delta;
            }
        }

    }

}