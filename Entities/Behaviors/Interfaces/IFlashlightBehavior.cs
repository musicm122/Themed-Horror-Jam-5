using Godot;

namespace ThemedHorrorJam5.Entities.Components
{
    public interface IFlashlightBehavior
    {
        Light2D Flashlight { get; set; }
        bool HasFlashlight { get; }
        bool IsDebugging { get; set; }
        PlayerDataStore DataStore { get; set; }

        void Init(PlayerDataStore dataStore);
        bool IsDebugPrintEnabled();
        void _PhysicsProcess(float delta);
        void _Ready();
    }
}