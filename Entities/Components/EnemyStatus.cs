using System;
using System.Collections.Generic;
using Godot;
using ThemedHorrorJam5.Entities.Behaviors.Interfaces;

namespace ThemedHorrorJam5.Entities.Components
{
    public class EnemyStatus : Health
    {
        public IVision VisionManager { get; set; }

        public Action TargetVisibleCallback {get;set;}

        public NodePath PatrolPath { get; private set; }
        
        public Label Cooldown { get; set; }

        public Label DebugLabel { get; set; }

        public Stack<Vector2> NavPath { get; set; }

        public Navigation2D Navigation2D { get; set; }

        public Path2D Path { get; set; }


        public Vector2[] PatrolPoints { get; set; }

        public bool LineOfSight { get; set; }

        public int PatrolIndex { get; set; } = 0;
        
        public float CurrentCoolDownCounter { get; set; }
        
        public float MaxCoolDownTime { get; set; } = 10f;

        public void Init(NodePath patrolPath){
            PatrolPath = patrolPath;
        }
    }
}