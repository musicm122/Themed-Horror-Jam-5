using System;
using Godot;
using ThemedHorrorJam5.Scripts.ItemComponents;

namespace ThemedHorrorJam5.Entities.Components
{
    public class Status : Node, IDebuggable<Node>
    {
        public Action EmptyHealthBarCallback { get; set; }
        public Action<int> HealthChangedCallback { get; set; }
        public Action<int> MaxHealthChangedCallback { get; set; }

        public bool IsDead() => currentHealth<=0;
        
        [Signal]
        public delegate void NoHealth();

        [Signal]
        public delegate void HealthChanged(int val);

        [Signal]
        public delegate void MaxHealthChanged(int val);

        private int maxHealth = 1;
        private int currentHealth = 1;

        [Export]
        public int MaxHealth
        {
            get => maxHealth;
            set
            {
                maxHealth = value;
                this.CurrentHealth = Mathf.Min(maxHealth, CurrentHealth);
                MaxHealthChangedCallback?.Invoke(maxHealth);
                EmitSignal(nameof(MaxHealthChanged), maxHealth);
            }
        }

        [Export]
        public int CurrentHealth
        {
            get => currentHealth;
            set
            {
                currentHealth = value;
                HealthChangedCallback?.Invoke(currentHealth);
                EmitSignal(nameof(HealthChanged), currentHealth);
                if (currentHealth <= 0)
                {
                    EmptyHealthBarCallback?.Invoke();
                    EmitSignal(nameof(NoHealth));
                }
            }
        }

        [Export]
        public bool IsDebugging { get; set; } = false;

        public bool IsDebugPrintEnabled() => IsDebugging;

    }
}