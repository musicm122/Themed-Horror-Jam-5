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


                //Enemy.Status.Navigation2D = GetLevelNavigation();
                //var nav = (Navigation2D)Enemy.Owner.GetNode("Navigation2D");
                //Enemy.Status.Navigation2D = (Navigation2D)Enemy.Owner.GetNode("Navigation2D");


                //var from = Enemy.Position;
                //var from = Enemy.Status.Navigation2D.ToLocal(Enemy.Position);
                var from = Enemy.GlobalPosition;

                //var to = Enemy.Status.Navigation2D.ToLocal(PlayerRef.Position);
                var to = PlayerRef.GlobalPosition;
                //var to = Enemy.Status.Target.Position;
                //var to = Enemy.Status.Target.ToLocal(Enemy.Status.Target.Position);

                //Enemy.DrawLine(from, to, new Color(255, 255, 255), 3);

                //var paths = Enemy.Status.Navigation2D.GetSimplePath(from, to);

                var paths = Nav.GetSimplePath(from, to);

                Enemy.Status.NavPath = new Stack<Vector2>(paths);

                if (Enemy.Status.Line != null)
                {
                    Enemy.Status.Line.Points = Enemy.Status.NavPath.ToArray();
                }

                var distance_to_walk = Enemy.MoveSpeed * delta;

                while (distance_to_walk > 0f && Enemy.Status.NavPath.Count > 0f)
                {
                    var distance_to_next_point = Enemy.Position.DistanceTo(Enemy.Status.NavPath.Peek());
                    if (distance_to_walk <= distance_to_next_point)
                    {
                        var newPosition = Enemy.Position.DirectionTo(Enemy.Status.NavPath.Peek()) * distance_to_walk;
                        Enemy.Status.VisionManager.UpdateFacingDirection(newPosition.Normalized());
                        Enemy.Position += newPosition;
                    }
                    else
                    {
                        var newPosition = Enemy.Status.NavPath.Pop();
                        Enemy.Status.VisionManager.UpdateFacingDirection(newPosition.Normalized());
                        if (Enemy.GetSlideCount() > 0)
                        {
                            Enemy.HandleMovableObstacleCollision(newPosition);
                        }
                        Enemy.Position = newPosition;
                    }
                    distance_to_walk -= distance_to_next_point;
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