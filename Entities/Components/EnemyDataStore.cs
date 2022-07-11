using System;
using System.Globalization;
using Godot;
using ThemedHorrorJam5.Entities.Behaviors.Interfaces;
using ThemedHorrorJam5.Scripts.Patterns.Logger;

namespace ThemedHorrorJam5.Entities.Components
{
    public class EnemyDataStore : Health
    {
        public IVision VisionManager { get; set; }

        public ILogger Logger { get; set; }
        public NodePath PatrolPath { get; private set; }

        public Label Cooldown { get; set; }

        public Label DebugLabel { get; set; }

        public Path2D Path { get; set; }

        public Vector2[] PatrolPoints { get; set; }

        public bool LineOfSight { get; set; }

        public int PatrolIndex { get; set; } = 0;

        public float CurrentCoolDownCounter { get; set; }

        public float MaxCoolDownTime { get; set; } = 10f;

        public void Init(NodePath patrolPath)
        {
            PatrolPath = patrolPath;
        }

        public override void _PhysicsProcess(float delta)
        {
            base._PhysicsProcess(delta);

            if (CurrentCoolDownCounter > 0f)
            {
                this.Print("CurrentCoolDownCounter = ", CurrentCoolDownCounter);
                CurrentCoolDownCounter -= delta;
                Cooldown.Text =
                    $"Cooling Down in {CurrentCoolDownCounter.ToString(CultureInfo.InvariantCulture)} seconds";
            }
            else
            {
                Cooldown.Text = String.Empty;
            }
        }
    }
}