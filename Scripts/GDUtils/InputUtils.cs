using Godot;

namespace ThemedHorrorJam5.Scripts.GDUtils
{
    public static class VectorUtils 
    {
        public static Vector2 GetPathDirection(Path2D path2d, PathFollow2D pathFollow2D, Vector2 position) 
        {
            //func get_path_direction(pos):
            var offset = path2d.Curve.GetClosestOffset(position);
            pathFollow2D.Offset = offset;
            return pathFollow2D.Transform.x;
            //var offset = Path2D.curve.get_closest_offset(pos)
            //$Path2D / PathFollow2D.offset = offset
            //return $Path2D / PathFollow2D.transform.x
        }
    }
    public static class InputUtils
    {

        public static bool IsInteracting() => Input.IsActionJustPressed(InputAction.Interact);

        public static Vector2 GetTopDownMovementInput(float speed = 1f)
        {
            var velocity = Vector2.Zero;
            if (Input.IsActionPressed(InputAction.Right))
            {
                velocity.x += 1f;
            }
            if (Input.IsActionPressed(InputAction.Left))
            {
                velocity.x -= 1f;
            }
            if (Input.IsActionPressed(InputAction.Up))
            {
                velocity.y -= 1f;
            }
            if (Input.IsActionPressed(InputAction.Down))
            {
                velocity.y += 1f;
            }
            return velocity.Normalized() * speed;
        }

        public static Vector2 GetTopDownWithDiagMovementInput(float speed = 1f)
        {
            var velocity = Vector2.Zero;
            
            if (Input.IsActionPressed(InputAction.Right) && Input.IsActionPressed(InputAction.Up))
            {
                velocity.x += 0.5f;
                velocity.y -= 0.5f;
            }
            if (Input.IsActionPressed(InputAction.Right) && Input.IsActionPressed(InputAction.Down))
            {
                velocity.x += 0.5f;
                velocity.y += 0.5f;
            }
            if (Input.IsActionPressed(InputAction.Left) && Input.IsActionPressed(InputAction.Up))
            {
                velocity.x -= 0.5f;
                velocity.y -= 0.5f;
            }
            if (Input.IsActionPressed(InputAction.Left) && Input.IsActionPressed(InputAction.Down))
            {
                velocity.x -= 0.5f;
                velocity.y += 0.5f;
            }
            if (Input.IsActionPressed(InputAction.Right))
            {
                velocity.x += 1f;
            }
            if (Input.IsActionPressed(InputAction.Left))
            {
                velocity.x -= 1f;
            }
            if (Input.IsActionPressed(InputAction.Up))
            {
                velocity.y -= 1f;
            }
            if (Input.IsActionPressed(InputAction.Down))
            {
                velocity.y += 1f;
            }
            return velocity.Normalized() * speed;
        }
    }
}