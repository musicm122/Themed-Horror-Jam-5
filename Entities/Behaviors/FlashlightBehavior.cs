using System;
using Godot;
using ThemedHorrorJam5.Scripts.GDUtils;
using ThemedHorrorJam5.Scripts.Patterns.Logger;

namespace ThemedHorrorJam5.Entities.Components
{
    public class FlashlightBehavior : Node2D, IDebuggable<Node>, IFlashlightBehavior
    {
        public PlayerDataStore DataStore { get; set; }

        public void Init(PlayerDataStore dataStore)
        {
            DataStore = dataStore;
        }

        [Export]
        public bool IsDebugging { get; set; }

        public bool IsDebugPrintEnabled() => IsDebugging;

        public bool HasFlashlight =>
            DataStore.Inventory.HasItemInInventory("Flashlight");

        public Light2D Flashlight { get; set; }
        private void ToggleFlashlight()
        {
            this.PrintCaller();
            Flashlight.Enabled = !Flashlight.Enabled;
        }

        public override void _Ready()
        {
            Flashlight = GetNode<Light2D>("Flashlight");
            Flashlight.Enabled = false;
        }

        public override void _PhysicsProcess(float delta)
        {
            if (Input.IsActionJustPressed(InputAction.ToggleFlashlight) && HasFlashlight)
            {
                ToggleFlashlight();
            }
        }

    }
}