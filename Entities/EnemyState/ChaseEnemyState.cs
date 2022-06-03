using Godot;
using ThemedHorrorJam5.Scripts.Enum;
using ThemedHorrorJam5.Scripts.Patterns.StateMachine;
using ThemedHorrorJam5.Scripts.GDUtils;
using System.Collections.Generic;
using System.Linq;

namespace ThemedHorrorJam5.Entities
{
    public class ChaseEnemyState : State
    {
        private Navigation2D? GetLevelNavigation()
        {
            var nodeTuples = Enemy.GetTree().GetNavigation2dNodes();
            if (nodeTuples.Item1) return nodeTuples.Item2[0];
            return null;
        }

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
            if (Enemy.IsDebugging && Enemy.HasNode("Line2D"))
            {
                Enemy.Status.Line = (Line2D)Enemy.GetNode("Line2D");

            }
            if (Enemy.Owner.HasNode("Navigation2D"))
            {
                if (Enemy.IsDebugging)
                {
                    Enemy.Status.DebugLabel.Text =
                   @$"
                    |-----------------------------------------------------------
                    | Enemy Global Position: {Enemy.GlobalPosition}
                    | Enemy Local Position: {Enemy.Position}
                    |----------------------------------------------------------
                    | Enemy Movable Global Position: {Enemy.Movable.GlobalPosition}
                    | Enemy Movable Local Position: {Enemy.Movable.Position}
                    |----------------------------------------------------------
                    | Target Global Position: {Enemy.Status.Target.GlobalPosition}
                    | Target Local Position: {Enemy.Status.Target.Position}
                    |-----------------------------------------------------------";
                }

                //Enemy.Status.Navigation2D = GetLevelNavigation();
                Enemy.Status.Navigation2D = (Navigation2D)Enemy.Owner.GetNode("Navigation2D");

                var from = Enemy.Movable.Position;
                var to = Enemy.Status.Target.Position;
                //var to = Enemy.Status.Target.ToLocal(Enemy.Status.Target.Position);
                
                //Enemy.DrawLine(from, to, new Color(255, 255, 255), 3);

                var paths = Enemy.Status.Navigation2D.GetSimplePath(from, to);

                Enemy.Status.NavPath = new Stack<Vector2>(paths);

                if (Enemy.Status.Line != null)
                {
                    Enemy.Status.Line.Points = Enemy.Status.NavPath.ToArray();
                }

                var distance_to_walk = Enemy.Movable.MoveSpeed * delta;

                while (distance_to_walk > 0f && Enemy.Status.NavPath.Count > 0f)
                {
                    var distance_to_next_point = Enemy.Movable.Position.DistanceTo(Enemy.Status.NavPath.Peek());
                    if (distance_to_walk <= distance_to_next_point)
                    {
                        var newPosition = Enemy.Movable.Position.DirectionTo(Enemy.Status.NavPath.Peek()) * distance_to_walk;
                        Enemy.Status.VisionManager.UpdateFacingDirection(newPosition.Normalized());
                        Enemy.Movable.Position += newPosition;
                    }
                    else
                    {
                        var newPosition = Enemy.Status.NavPath.Pop();
                        Enemy.Status.VisionManager.UpdateFacingDirection(newPosition.Normalized());
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