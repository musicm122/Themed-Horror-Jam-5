using Godot;
using ThemedHorrorJam5.Scripts.ItemComponents;

namespace ThemedHorrorJam5.Entities.Components
{
    public class Status : Node, IDebuggable<Node>
    {
        [Signal]
        public delegate void NoHealth();

        [Signal]
        public delegate void HealthChanged(float val);

        [Signal]
        public delegate void MaxHealthChanged(float val);

        private float maxHealth = 1f;
        private float currentHealth = 1f;

        [Export]
        public float MaxHealth
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
        public float CurrentHealth
        {
            get => currentHealth;
            set
            {
                currentHealth = value;
                EmitSignal(nameof(HealthChanged), currentHealth);
                if (currentHealth <= 0f)
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
            CurrentHealth = MaxHealth;
        }
    }
}
