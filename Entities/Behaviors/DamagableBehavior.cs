using System;
using Godot;
using ThemedHorrorJam5.Scripts.GDUtils;
using ThemedHorrorJam5.Scripts.ItemComponents;

namespace ThemedHorrorJam5.Entities.Components
{
    public class DamagableBehavior : Node2D, IDebuggable<Node>
    {
        public bool IsDead = false;
        public Status Status { get; set; }

        [Export]
        public bool IsDebugging { get; set; } = false;

        private Hurtbox HurtBox { get; set; }

        public Action<Node,Vector2> OnTakeDamage { get; set; }
        public Action EmptyHealthBarCallback { get; set; }
        public Action HurtboxInvincibilityStartedCallback { get; set; }
        public Action HurtboxInvincibilityEndedCallback { get; set; }
        public Action<int> HealthChangedCallback { get; set; }
        public Action<int> MaxHealthChangedCallback { get; set; }

        public bool IsDebugPrintEnabled() => IsDebugging;

        
        public void OnHurtboxAreaEntered(Node body)
        {
            this.Print($"OnHurtboxAreaEntered({body.Name})");
            if (body.Name == "HitBox" || body.Name == "Spikes")
            {
                
                this.HurtBox.StartInvincibility(0.6f);

                var hitBox = (HitBox)body;
                var force = (this.Position - hitBox.Position) * hitBox.EffectForce;
                OnTakeDamage?.Invoke(body, force);
                //this.MoveAndSlide(force);
                Status.CurrentHealth -= hitBox.Damage;
                
                this.Print("Current Health =", Status.CurrentHealth);
                //TODO: Add Hit effect visual and sound
                //Hurtbox.create_hit_effect()
                //var playerHurtSound = PlayerHurtSound.instance()
                //get_tree().current_scene.add_child(playerHurtSound)
            }
        }

        public void OnEmptyHealthBar()
        {
            this.PrintCaller();
            this.IsDead = true;
            //RefreshUI();
            //IsDead = true;
            //LockMovement();
            EmptyHealthBarCallback?.Invoke();
        }

        public void OnHurtboxInvincibilityStarted()
        {
            //this.PrintCaller();
            HurtboxInvincibilityStartedCallback?.Invoke();
        }

        public void OnHurtboxInvincibilityEnded()
        {
            this.PrintCaller();
            HurtboxInvincibilityEndedCallback?.Invoke();
        }

        public void OnHealthChanged(int health)
        {
            //RefreshUI();
            HealthChangedCallback?.Invoke(health);
        }

        public void OnMaxHealthChanged(int health)
        {
            //RefreshUI();
            MaxHealthChangedCallback?.Invoke(health);
        }

        private void RegisterHealthSignals()
        {
            Status.Connect(nameof(Status.NoHealth), this, nameof(OnEmptyHealthBar));
            Status.Connect(nameof(Status.HealthChanged), this, nameof(OnHealthChanged));
            Status.Connect(nameof(Status.MaxHealthChanged), this, nameof(OnMaxHealthChanged));
        }

        private void RegisterHurtBoxSignals()
        {
            if (!HurtBox.TryConnectSignal("area_entered", this, nameof(OnHurtboxAreaEntered)))
            {
                var arg = $"TryConnectSignal('area_entered', {this.Name}, {nameof(OnHurtboxAreaEntered)})";
                this.Print($"Attempt to register Hurtbox's signal with args {arg} to player failed");
            }

            if (!HurtBox.TryConnectSignal(nameof(Hurtbox.InvincibilityStarted), this, nameof(OnHurtboxInvincibilityStarted)))
            {
                var arg = $"TryConnectSignal({nameof(Hurtbox.InvincibilityStarted)}, {this.Name}, {nameof(OnHurtboxInvincibilityStarted)})";
                this.Print($"Attempt to register Hurtbox's signal with args {arg} to player failed");
            }

            if (!HurtBox.TryConnectSignal(nameof(Hurtbox.InvincibilityEnded), this, nameof(OnHurtboxInvincibilityEnded)))
            {
                var arg = $"TryConnectSignal({nameof(Hurtbox.InvincibilityEnded)}, {this.Name}, {nameof(OnHurtboxInvincibilityEnded)})";
                this.Print($"Attempt to register Hurtbox's signal with args {arg} to player failed");
            }
        }

        public override void _Ready()
        {
            HurtBox = GetNode<Hurtbox>("Hurtbox");
            Status = GetNode<Status>("PlayerStats");
            RegisterHealthSignals();
            RegisterHurtBoxSignals();
        }
    }
}