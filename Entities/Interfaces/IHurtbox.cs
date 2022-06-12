using Godot;

namespace ThemedHorrorJam5.Entities.Components
{
    public interface IHurtbox
    {
        CollisionShape2D CollisionShape { get; set; }
        bool IsInvincible { get; set; }
        bool IsDebugging { get; set; }
        Timer Timer { get; set; }

        bool IsDebugPrintEnabled();
        void OnHurtboxInvincibilityEnded();
        void OnHurtboxInvincibilityStarted();
        void OnTimerTimeout();
        void SetInvincibility(bool hasInvincibility);
        void StartInvincibility(float duration);
        void _Ready();
    }
}