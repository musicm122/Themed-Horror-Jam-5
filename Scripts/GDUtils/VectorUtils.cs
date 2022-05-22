using System;
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

        public static void RotateCounterClockwiseByAngle(this Vector2 vector, float angle) 
        {
            //x' = x cos θ − y sin θ
            //y' = x sin θ + y cos θ
            //newX = oldX * cos(angle) - oldY * sin(angle)
            //newY = oldX * sin(angle) + oldY * cos(angle)
            var newX = (vector.x * Mathf.Cos(angle)) - (vector.y * Mathf.Sin(angle));
            var newY = (vector.x * Mathf.Sin(angle)) + (vector.y * Mathf.Cos(angle));
            vector = new Vector2(newX, newY);
        }
        public static Vector2 RotatedCounterClockwiseByAngle(this Vector2 vector, float angle)
        {
            //x' = x cos θ − y sin θ
            //y' = x sin θ + y cos θ
            //newX = oldX * cos(angle) - oldY * sin(angle)
            //newY = oldX * sin(angle) + oldY * cos(angle)
            var newX = (vector.x * Mathf.Cos(angle)) - (vector.y * Mathf.Sin(angle));
            var newY = (vector.x * Mathf.Sin(angle)) + (vector.y * Mathf.Cos(angle));
            return new Vector2(newX, newY);
        }
    }
}