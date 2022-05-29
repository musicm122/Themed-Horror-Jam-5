using System;
using System.Collections.Generic;
using Godot;
using ThemedHorrorJam5.Scripts.Patterns.Logger;

namespace ThemedHorrorJam5.Entities.Components
{
    public class EnemyStatus : Status
    {
        public Area2D VisionRadius { get; set; }

        public NodePath PatrolPath { get; set; }
        
        public Label Cooldown { get; set; }

        public Label DebugLabel { get; set; }

        public Stack<Vector2> NavPath { get; set; }

        public Navigation2D Navigation2D { get; set; }

        public Path2D Path { get; set; }

        public Node2D Target { get; set; }

        public Line2D Line { get; set; }

        public Vector2[] PatrolPoints { get; set; }
        public bool LineOfSight = false;

        public int PatrolIndex { get; set; } = 0;
        
        public float CurrentCoolDownCounter { get; set; } = 0;

        public void UpdateVisionConeLocation(Vector2 newVelocity)
        {
            if (newVelocity.x < 0)
            {
                VisionRadius.Scale = new Vector2(-1, VisionRadius.Scale.y);
            }
            else
            {
                VisionRadius.Scale = new Vector2(1, VisionRadius.Scale.y);
            }
        }

        public bool IsPlayerInSight()
        {
            var bodies = VisionRadius.GetOverlappingBodies();
            if (bodies == null || bodies.Count == 0) return false;
            for (int i = 0; i < bodies.Count; i++)
            {
                var body = (Node)bodies[i];
                if (body.Name.ToLower().Contains("player"))
                {
                    return true;
                }
            }
            return false;
        }

        public void Init(NodePath patrolPath){
            PatrolPath = patrolPath;
        }
    }
}