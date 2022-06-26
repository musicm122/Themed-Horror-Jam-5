using Godot;

namespace ThemedHorrorJam5.Entities.Behaviors
{
    public interface IMovableCamera
    {
        void SetZoom(float amount);
        void SetPan(Vector2 newOffset);
        void ResetCamera();
    }
}