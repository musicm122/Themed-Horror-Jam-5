using Godot;
using ThemedHorrorJam5.Scripts.Patterns.Logger;
using TinyIoC;

namespace ThemedHorrorJam5.Entities
{
    public class Global : Node
    {
        public Global()
        {
            Container.Register<ILogger, GDLogger>().AsSingleton();
        }

        public TinyIoCContainer Container { get; } = new();
    }
}