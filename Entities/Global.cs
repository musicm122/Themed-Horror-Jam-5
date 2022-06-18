using Godot;
using ThemedHorrorJam5.Scripts.Libraries;
using ThemedHorrorJam5.Scripts.Patterns.Logger;

namespace ThemedHorrorJam5.Entities
{
    public class Global : Node
    {
        public Global()
        {
            Container.Register<ILogger, GDLogger>().AsSingleton();
        }

        public TinyIoCContainer Container { get; } = new();

        public override void _Ready()
        {
        }
    }
}