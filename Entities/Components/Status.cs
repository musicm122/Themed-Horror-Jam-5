using Godot;
using ThemedHorrorJam5.Scripts.ItemComponents;

namespace ThemedHorrorJam5.Entities.Components
{
    public class Status : Node, IDebuggable<Node>
    {
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
                EmitSignal(nameof(HealthChanged), currentHealth);
                if (currentHealth <= 0)
                {
                    EmitSignal(nameof(NoHealth));
                }
            }
        }

        [Export]
        public bool IsDebugging { get; set; } = false;

        public bool IsDebugPrintEnabled() => IsDebugging;

        public override void _Ready()
        {

        }
    }
}