using Godot;
using ThemedHorrorJam5.Scripts.Patterns.Logger;

namespace ThemedHorrorJam5.Entities.Components
{
    public class Hurtbox : Area2D, IDebuggable<Node>, IHurtbox
    {
        [Export]
        public bool IsDebugging { get; set; } = false;

        [Signal]
        public delegate void InvincibilityStarted();

        [Signal]
        public delegate void InvincibilityEnded();

        public bool IsInvincible { get; set; }
        public Timer Timer { get; set; }
        public CollisionShape2D CollisionShape { get; set; }

        public void SetInvincibility(bool hasInvincibility)
        {
            IsInvincible = hasInvincibility;
            if(IsInvincible)
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
            SetInvincibility(true);
            Timer.Start(duration);
        }

        public void OnTimerTimeout() => SetInvincibility(false);

        public void OnHurtboxInvincibilityStarted() => CollisionShape.SetDeferred("disabled", true);

        public void OnHurtboxInvincibilityEnded() => CollisionShape.Disabled = false;

        public override void _Ready()
        {
            Timer = GetNode<Timer>("Timer");
            Timer.Connect("timeout", this, nameof(OnTimerTimeout));

            CollisionShape = GetNode<CollisionShape2D>("CollisionShape");
            this.Connect(nameof(Hurtbox.InvincibilityStarted), this, nameof(OnHurtboxInvincibilityStarted));
            this.Connect(nameof(Hurtbox.InvincibilityEnded), this, nameof(OnHurtboxInvincibilityEnded));
        }

        public bool IsDebugPrintEnabled() => this.IsDebugging;
    }
}