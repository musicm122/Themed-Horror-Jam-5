using Godot;
using ThemedHorrorJam5.Scripts.ItemComponents;

namespace ThemedHorrorJam5.Entities.Components
{
    public class Hurtbox : Area2D, IDebuggable<Node>
    {
        [Export]
        public bool IsDebugging { get; set; } = false;

        [Signal]
        public delegate void InvincibilityStarted();

        [Signal]
        public delegate void InvincibilityEnded();

        public bool Invincible { get; set; }
        public Timer Timer { get; set; }
        public CollisionShape2D CollisionShape { get; set; }

        public void SetInvincibility(bool hasInvincibility)
        {
            Invincible = hasInvincibility;
            if (hasInvincibility)
            {
                EmitSignal(nameof(InvincibilityStarted));
            }
            else
            {
                EmitSignal(nameof(InvincibilityEnded));
            }
        }

        public void StartInvincibility(float duration)
        {
            this.Invincible = true;
            Timer.Start(duration);
        }

        public void OnTimerTimeout() => Invincible = false;

        public void OnHurtboxInvincibilityStarted() => CollisionShape.SetDeferred("disabled", true);

        public void OnHurtboxInvincibilityEnded() => CollisionShape.Disabled = false;

        public override void _Ready()
        {
            Timer = GetNode<Timer>("Timer");
            Timer.Connect("timeout", this, nameof(OnTimerTimeout));

            CollisionShape = GetNode<CollisionShape2D>("CollisionShape");
            this.Connect(nameof(InvincibilityStarted), this, nameof(OnHurtboxInvincibilityStarted));
            this.Connect(nameof(InvincibilityEnded), this, nameof(OnHurtboxInvincibilityEnded));
        }

        public bool IsDebugPrintEnabled() => this.IsDebugging;
    }
}