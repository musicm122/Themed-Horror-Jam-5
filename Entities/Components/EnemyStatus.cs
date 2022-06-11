using System;
using System.Collections.Generic;
using ThemedHorrorJam5.Scripts.Extensions;
using Godot;
using ThemedHorrorJam5.Scripts.Patterns.Logger;
using ThemedHorrorJam5.Entities.Behaviors.Interfaces;

namespace ThemedHorrorJam5.Entities.Components
{
    public class EnemyStatus : Health
    {
        public IVision VisionManager { get; set; }

        public Action TargetVisibleCallback {get;set;}

        public NodePath PatrolPath { get; set; }
        
        public Label Cooldown { get; set; }

        public Label DebugLabel { get; set; }

        public Stack<Vector2> NavPath { get; set; }

        public Navigation2D Navigation2D { get; set; }

        public Path2D? Path { get; set; }

        public Node2D? Target { get; set; }

        public Line2D Line { get; set; }

        public Vector2[] PatrolPoints { get; set; }

        public bool LineOfSight = false;

        public int PatrolIndex { get; set; } = 0;
        
        public float CurrentCoolDownCounter { get; set; } = 0;
        public float MaxCoolDownTime { get; set; } = 10f;

        public void Init(NodePath patrolPath){
            PatrolPath = patrolPath;
            // VisionRadius.ConnectBodyEntered(this, nameof(VisionRadius.OnVisionRadiusBodyEntered));
            // VisionRadius.ConnectBodyExited(this, nameof(OnVisionRadiusBodyExit));
        }
    }
}