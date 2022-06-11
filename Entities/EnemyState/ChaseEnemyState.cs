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
        private Navigation2D Nav { get; set; }
        private EnemyV4 Enemy { get; set; }
        private PlayerV2 PlayerRef { get; set; }

        public ChaseEnemyState(EnemyV4 enemy)
        {
            this.Name = EnemyBehaviorStates.ChasePlayer.GetDescription();
            Enemy = enemy;
            (var hasPlayer, PlayerRef) = Enemy.GetTree().GetPlayerNode();
            if (!hasPlayer)
            {
                Logger.Error("Player ref not found on scene tree");
            }

            (var hasNav, var navNodes) = Enemy.GetTree().GetNavigation2dNodes();
            if (hasNav && navNodes != null)
            {
                Nav = navNodes[0];
            }

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
            if (Nav!=null)
            {
                var from = Nav.ToLocal(Enemy.GlobalPosition);
                var to = Nav.ToLocal(PlayerRef.GlobalPosition);
                var paths = Nav.GetSimplePath(from, to);

                Enemy.Status.NavPath = new Stack<Vector2>(paths);

                var distance_to_walk = Enemy.MoveSpeed * delta;
                while (distance_to_walk > 0f && paths.Length > 0f)
                {
                    //var distance_to_next_point = Enemy.Position.DistanceTo(Enemy.Status.NavPath.Peek());
                    var distance_to_next_point = Enemy.GlobalPosition.DistanceTo(Nav.ToGlobal(Enemy.Status.NavPath.Peek()));
                    var next_point = Nav.ToGlobal(Enemy.Status.NavPath.Peek());
                    var global_direction = Enemy.GlobalPosition.DirectionTo(next_point);
                    var global_distance = Enemy.GlobalPosition.DistanceTo(next_point);
                    if (distance_to_walk <= global_distance)
                    {
                        var global_displacement = global_direction * distance_to_walk;
                        var global_new_position = Enemy.GlobalPosition + global_displacement;
                        var local_new_position = Enemy.ToLocal(global_new_position);
                        Enemy.Status.VisionManager.UpdateFacingDirection(local_new_position);
                        //Enemy.GlobalPosition = global_new_position;
                        Enemy.MoveAndSlide(global_displacement / delta);

                    }
                    else
                    {
                        _ = Enemy.Status.NavPath.Pop();
                        var global_new_position = next_point;
                        var local_new_position = Enemy.ToLocal(global_new_position);
                        Enemy.Status.VisionManager.UpdateFacingDirection(local_new_position);
                        if (Enemy.GetSlideCount() > 0)
                        {
                            Enemy.HandleMovableObstacleCollision(global_direction);
                        }
                        Enemy.GlobalPosition = global_new_position;
                    }
                    distance_to_walk -= global_distance;





                    // //var distance_to_next_point = Enemy.GlobalPosition.DistanceTo(Enemy.ToLocal(Nav.ToGlobal(paths.)))
                    // if (distance_to_walk <= distance_to_next_point)
                    // {
                    //     var newPosition = Enemy.Position.DirectionTo(Enemy.Status.NavPath.Peek()) * distance_to_walk;
                    //     Enemy.Status.VisionManager.UpdateFacingDirection(newPosition.Normalized());
                    //     Enemy.Position += newPosition;
                    // }
                    // else
                    // {
                    //     var newPosition = Enemy.Status.NavPath.Pop();
                    //     Enemy.Status.VisionManager.UpdateFacingDirection(newPosition.Normalized());
                    //     if (Enemy.GetSlideCount() > 0)
                    //     {
                    //         Enemy.HandleMovableObstacleCollision(newPosition);
                    //     }
                    //     Enemy.Position = newPosition;
                    // }
                    // distance_to_walk -= distance_to_next_point;
                }
                if (Enemy.IsDebugging)
                {
                    Enemy.Status.DebugLabel.Text =
                   @$"
                    |-----------------------------------------------------------
                    | Enemy Global Position: {Enemy.GlobalPosition}
                    | Enemy Local Position: {Enemy.Position}
                    |----------------------------------------------------------
                    | Target Global Position: {Enemy.Status.Target.GlobalPosition}
                    | Target Local Position: {Enemy.Status.Target.Position}
                    |-----------------------------------------------------------
                    | From {from}
                    | To {to}
                    |-----------------------------------------------------------";
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