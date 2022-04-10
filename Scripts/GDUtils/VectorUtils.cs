using Godot;

namespace ThemedHorrorJam5.Scripts.GDUtils
{
    public static class VectorUtils
    {
        public static Vector2 GetPathDirection(Path2D path2d, PathFollow2D pathFollow2D, Vector2 position)
        {
            var offset = path2d.Curve.GetClosestOffset(position);
            pathFollow2D.Offset = offset;
            return pathFollow2D.Transform.x;
        }
    }
}