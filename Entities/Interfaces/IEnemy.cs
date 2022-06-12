using Godot;
using ThemedHorrorJam5.Entities.Components;

namespace ThemedHorrorJam5.Entities
{
    public interface IEnemy
    {
        Label Cooldown { get; set; }
        IDamagableBehavior Damagable { get; }
        Node2D ObstacleAvoidance { get; set; }
        NodePath PatrolPath { get; set; }
        EnemyStatus Status { get; set; }

        void Init();
        void _PhysicsProcess(float delta);
        void _Ready();
    }
}