using Godot;
using ThemedHorrorJam5.Scripts.Enum;
using ThemedHorrorJam5.Scripts.Patterns.StateMachine;
using ThemedHorrorJam5.Scripts.GDUtils;
using System.Collections.Generic;
using ThemedHorrorJam5.Entities.Components;
using ThemedHorrorJam5.Entities.Behaviors.Interfaces;

namespace ThemedHorrorJam5.Entities
{

    public class ChaseEnemyState : State
    {
        private Navigation2D Nav { get; set; }
        private EnemyV4 Enemy { get; set; }
        private PlayerV2 PlayerRef { get; set; }
        private EnemyStatus Status { get => Enemy.Status; }
        private IVision VisionManager { get => Enemy.Status.VisionManager; }

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
            if (Nav!=null)
            {
                var from = Nav.ToLocal(Enemy.GlobalPosition);
                var to = Nav.ToLocal(PlayerRef.GlobalPosition);
                var paths = Nav.GetSimplePath(from, to);

                var enemyNavPath = new Stack<Vector2>(paths);
                var distance_to_walk = Enemy.MoveSpeed * delta;
                
                while (distance_to_walk > 0f && enemyNavPath.Count > 0)
                {
                    var next_point = Nav.ToGlobal(enemyNavPath.Peek());
                    var global_direction = Enemy.GlobalPosition.DirectionTo(next_point);
                    var global_distance = Enemy.GlobalPosition.DistanceTo(next_point);
                    if (distance_to_walk <= global_distance)
                    {
                        var global_displacement = global_direction * distance_to_walk;
                        var global_new_position = Enemy.GlobalPosition + global_displacement;
                        var local_new_position = Enemy.ToLocal(global_new_position);
                        Enemy.Status.VisionManager.UpdateFacingDirection(local_new_position);
                        Enemy.MoveAndSlide(global_displacement / delta);

                    }
                    else
                    {
                        _ = enemyNavPath.Pop();
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
                }
            }
            else
            {
                Logger.Error("Navigation2D not found");
            }
            if (Status.CurrentCoolDownCounter > 0)
            {
                Status.CurrentCoolDownCounter -= delta;
            }
        }
    }
}