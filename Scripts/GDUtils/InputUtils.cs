using Godot;

namespace ThemedHorrorJam5.Scripts.GDUtils
{
    public static class InputUtils
    {

        public static bool IsAnyKeyPressed() =>
            Input.IsActionJustPressed(InputAction.ToggleFlashlight) ||
            Input.IsActionJustPressed(InputAction.Run) ||
            Input.IsActionJustPressed(InputAction.Right) ||
            Input.IsActionJustPressed(InputAction.Up) ||
            Input.IsActionJustPressed(InputAction.Left) ||
            Input.IsActionJustPressed(InputAction.Down) ||
            Input.IsActionJustPressed(InputAction.Interact) ||
            Input.IsActionJustPressed(InputAction.Pause);


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