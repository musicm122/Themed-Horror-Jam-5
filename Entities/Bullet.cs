using Godot;
using ThemedHorrorJam5.Scripts.Patterns.Logger;

namespace ThemedHorrorJam5.Entities
{
    public class Bullet : KinematicBody2D, IDebuggable<Node>
    {
        [Export]
        public bool IsDebugging { get; set; } = false;

        [Export]
        public float Speed { get; set; }

        public Vector2 Velocity { get; set; } = Vector2.Zero;

        public Timer LifeTime { get; set; }

        public bool IsDebugPrintEnabled() => IsDebugging;

        public override void _Ready()
        {
            LifeTime = GetNode<Timer>("Lifetime");
            LifeTime.Connect("timeout", this, nameof(OnTimeout));
            LifeTime.Start();
        }

        public void Clear()
        {
            QueueFree();
        }

        private void OnTimeout()
        {
            Clear();
        }

        public void Start(Vector2 direction)
        {
            Velocity = direction * Speed;
        }

        //  // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(float delta)
        {
            var collision = MoveAndCollide(Velocity * delta);
            if (collision != null)
            {
                this.LifeTime.Stop();
                Clear();
            }
            /*
            var collision := move_and_collide(velocity * delta)
            if collision:
                timer.stop()
                clear()
                collision.collider.damage(10)
            */
        }
    }
}