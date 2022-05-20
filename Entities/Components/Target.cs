using Godot;

namespace ThemedHorrorJam5.Entities.Components
{
    public class Target : Node2D
    {
        public Vector2 GetAimAtPoint() =>
                (GlobalTransform.origin + Vector2.Up) * 1.5f;
    }
}