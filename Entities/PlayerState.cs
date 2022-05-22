using ThemedHorrorJam5.Entities.Components;
using ThemedHorrorJam5.Scripts.ItemComponents;

namespace ThemedHorrorJam5.Entities
{
    public class PlayerState
    {
        public Status PlayerStatus { get; set; } = new Status();
        public MissionManager MissionManager { get; set; } = new MissionManager();

        public Inventory Inventory { get; set; } = new Inventory();

    }
}