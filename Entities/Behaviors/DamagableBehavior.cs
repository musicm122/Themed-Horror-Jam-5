using System;
using Godot;
using ThemedHorrorJam5.Scripts.GDUtils;
using ThemedHorrorJam5.Scripts.Patterns.Logger;

namespace ThemedHorrorJam5.Entities.Components
{
    public class DamagableBehavior : Node2D, IDebuggable<Node>, IDamagableBehavior
    {
        public bool IsDead = false;

        private Health Status { get; set; }

        [Export]
        public bool IsDebugging { get; set; } = false;

        private Hurtbox HurtBox { get; set; }

        public Action<Node, Vector2> OnTakeDamage { get; set; }
        public Action EmptyHealthBarCallback { get; set; }
        public Action HurtboxInvincibilityStartedCallback { get; set; }
        public Action HurtboxInvincibilityEndedCallback { get; set; }
        public Action<int> HealthChangedCallback { get; set; }
        public Action<int> MaxHealthChangedCallback { get; set; }

        public bool IsDebugPrintEnabled() => IsDebugging;

        public void OnHurtboxAreaEntered(Node body)
        {
            this.Print($"OnHurtboxAreaEntered({body.Name})");
            if(this.HurtBox.IsInvincible) return ;
            if (body.Name.ToLower() == "hitbox" || body.Name.ToLower() == "spikes")
            {
                this.HurtBox.StartInvincibility(0.6f);

                var hitBox = (HitBox)body;
                var force = (this.GlobalPosition - hitBox.GlobalPosition) * hitBox.EffectForce;
                OnTakeDamage?.Invoke(body, force);
                Status.CurrentHealth -= hitBox.Damage;
                this.Print("Current Health =", Status.CurrentHealth);
            }
        }

        public void OnEmptyHealthBar()
        {
            this.PrintCaller();
            this.IsDead = true;
            EmptyHealthBarCallback?.Invoke();
        }

        public void OnHurtboxInvincibilityStarted()
        {
            this.PrintCaller();
            HurtboxInvincibilityStartedCallback?.Invoke();
        }

        public void OnHurtboxInvincibilityEnded()
        {
            this.PrintCaller();
            HurtboxInvincibilityEndedCallback?.Invoke();
        }

        public void OnHealthChanged(int health)
        {
            this.PrintCaller();
            HealthChangedCallback?.Invoke(health);
        }

        public void OnMaxHealthChanged(int health)
        {
            this.PrintCaller();
            MaxHealthChangedCallback?.Invoke(health);
        }

        private void RegisterHealthSignals()
        {
            this.PrintCaller();
            Status.Connect(nameof(Health.NoHealth), this, nameof(OnEmptyHealthBar));
            Status.Connect(nameof(Health.HealthChanged), this, nameof(OnHealthChanged));
            Status.Connect(nameof(Health.MaxHealthChanged), this, nameof(OnMaxHealthChanged));
        }

        private void RegisterHurtBoxSignals()
        {
            this.PrintCaller();
            if (!HurtBox.TryConnectSignal("area_entered", this, nameof(OnHurtboxAreaEntered)))
            {
                var arg = $"TryConnectSignal('area_entered', {this.Name}, {nameof(OnHurtboxAreaEntered)})";
                this.Print($"Attempt to register Hurtbox's signal with args {arg} failed");
            }

            if (!HurtBox.TryConnectSignal(nameof(Hurtbox.InvincibilityStarted), this, nameof(OnHurtboxInvincibilityStarted)))
            {
                var arg = $"TryConnectSignal({nameof(Hurtbox.InvincibilityStarted)}, {this.Name}, {nameof(OnHurtboxInvincibilityStarted)})";
                this.Print($"Attempt to register Hurtbox's signal with args {arg} failed");
            }

            if (!HurtBox.TryConnectSignal(nameof(Hurtbox.InvincibilityEnded), this, nameof(OnHurtboxInvincibilityEnded)))
            {
                var arg = $"TryConnectSignal({nameof(Hurtbox.InvincibilityEnded)}, {this.Name}, {nameof(OnHurtboxInvincibilityEnded)})";
                this.Print($"Attempt to register Hurtbox's signal with args {arg} failed");
            }
        }

        public void Init(Health status)
        {
            Status = status;
            HurtBox = GetNode<Hurtbox>("Hurtbox");
            RegisterHealthSignals();
            RegisterHurtBoxSignals();
        }

        public override void _Ready()
        {
            HurtBox = GetNode<Hurtbox>("Hurtbox");
        }
    }
}