using System.Collections.Generic;
using Godot;
using ThemedHorrorJam5.Entities.Behaviors.Interfaces;
using ThemedHorrorJam5.Entities.Components;
using ThemedHorrorJam5.Scripts.Enum;
using ThemedHorrorJam5.Scripts.Extensions;
using ThemedHorrorJam5.Scripts.GDUtils;
using ThemedHorrorJam5.Scripts.Patterns.StateMachine;

namespace ThemedHorrorJam5.Entities.EnemyState
{
    public class ChaseEnemyState : State
    {
        private Navigation2D Nav { get; }
        private EnemyV4 Enemy { get; }
        private PlayerV2 PlayerRef { get; }
        private EnemyDataStore DataStore => Enemy.EnemyDataStore;
        private IVision VisionManager => Enemy.EnemyDataStore.VisionManager;

        public ChaseEnemyState(EnemyV4 enemy)
        {
            this.Name = EnemyBehaviorStates.ChasePlayer.GetDescription();
            Enemy = enemy;
            (var hasPlayer, PlayerRef) = Enemy.GetTree().GetPlayerNode();
            if (!hasPlayer)
            {
                Logger.Error("Player ref not found on scene tree");
            }

            var (hasNav, navNodes) = Enemy.GetTree().GetNavigation2dNodes();
            if (hasNav && navNodes != null)
            {
                Nav = navNodes[0];
            }

            this.OnEnter += OnEnterState;
            this.OnExit += OnExitState;
            this.OnFrame += ChasePlayer;
        }

        private void OnEnterState()
        {
            this.Logger.Debug("ChaseEnemyState OnEnter called");
        }

        private void OnExitState()
        {
            this.Logger.Debug("ChaseEnemyState Exit called");
        }

        private void ChasePlayer(float delta)
        {
            if (Nav != null)
            {
                var from = Nav.ToLocal(Enemy.GlobalPosition);
                var to = Nav.ToLocal(PlayerRef.GlobalPosition);
                var paths = Nav.GetSimplePath(from, to);

                var enemyNavPath = new Stack<Vector2>(paths);
                var distanceToWalk = Enemy.MaxSpeed * delta;

                while (distanceToWalk > 0f && enemyNavPath.Count > 0)
                {
                    var nextPoint = Nav.ToGlobal(enemyNavPath.Peek());
                    var globalDirection = Enemy.GlobalPosition.DirectionTo(nextPoint);
                    var globalDistance = Enemy.GlobalPosition.DistanceTo(nextPoint);
                    if (distanceToWalk <= globalDistance)
                    {
                        var globalDisplacement = globalDirection * distanceToWalk;
                        var globalNewPosition = Enemy.GlobalPosition + globalDisplacement;
                        var localNewPosition = Enemy.ToLocal(globalNewPosition);
                        VisionManager.UpdateFacingDirection(localNewPosition);
                        Enemy.MoveAndSlide(globalDisplacement / delta);
                    }
                    else
                    {
                        _ = enemyNavPath.Pop();
                        var globalNewPosition = nextPoint;
                        var localNewPosition = Enemy.ToLocal(globalNewPosition);
                        VisionManager.UpdateFacingDirection(localNewPosition);
                        if (Enemy.GetSlideCount() > 0)
                        {
                            Enemy.HandleMovableObstacleCollision(globalDirection);
                        }

                        Enemy.GlobalPosition = globalNewPosition;
                    }

                    distanceToWalk -= globalDistance;
                }
            }
            else
            {
                Logger.Error("Navigation2D not found");
            }

            if (DataStore.CurrentCoolDownCounter > 0)
            {
                DataStore.CurrentCoolDownCounter -= delta;
            }
        }
    }
}