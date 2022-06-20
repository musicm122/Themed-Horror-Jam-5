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
        
        public static Vector2 GetTopDownWithDiagMovementInputStrength(float speed = 1f)
        {
            var velocity = Vector2.Zero;
            velocity.x = Input.GetActionStrength(InputAction.Right) - Input.GetActionStrength(InputAction.Left);
            velocity.y = Input.GetActionStrength(InputAction.Down) - Input.GetActionStrength(InputAction.Up);
            return velocity.Normalized() * speed;
        }
        
        public static Vector2 GetTopDownWithDiagMovementInputStrengthVector()
        {
            var velocity = Vector2.Zero;
            velocity.x = Input.GetActionStrength(InputAction.Right) - Input.GetActionStrength(InputAction.Left);
            velocity.y = Input.GetActionStrength(InputAction.Down) - Input.GetActionStrength(InputAction.Up);
            return velocity.Normalized();
        }
        
        public static Vector2 GetCameraMovementInput(float speed = 1f)
        {
            var velocity = Vector2.Zero;

            if (Input.IsActionPressed(InputAction.CameraRight) && Input.IsActionPressed(InputAction.CameraUp))
            {
                velocity.x += 0.5f;
                velocity.y -= 0.5f;
            }

            if (Input.IsActionPressed(InputAction.CameraRight) && Input.IsActionPressed(InputAction.CameraDown))
            {
                velocity.x += 0.5f;
                velocity.y += 0.5f;
            }

            if (Input.IsActionPressed(InputAction.CameraLeft) && Input.IsActionPressed(InputAction.CameraUp))
            {
                velocity.x -= 0.5f;
                velocity.y -= 0.5f;
            }

            if (Input.IsActionPressed(InputAction.CameraLeft) && Input.IsActionPressed(InputAction.CameraDown))
            {
                velocity.x -= 0.5f;
                velocity.y += 0.5f;
            }

            if (Input.IsActionPressed(InputAction.CameraRight))
            {
                velocity.x += 1f;
            }

            if (Input.IsActionPressed(InputAction.CameraLeft))
            {
                velocity.x -= 1f;
            }

            if (Input.IsActionPressed(InputAction.CameraUp))
            {
                velocity.y -= 1f;
            }

            if (Input.IsActionPressed(InputAction.CameraDown))
            {
                velocity.y += 1f;
            }

            return velocity.Normalized() * speed;
        }
        
        public static bool IsCameraReset() => Input.IsActionJustPressed(InputAction.CameraReset);
    }
}