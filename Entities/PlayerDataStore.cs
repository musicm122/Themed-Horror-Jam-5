using ThemedHorrorJam5.Entities.Components;
using ThemedHorrorJam5.Scripts.ItemComponents;

namespace ThemedHorrorJam5.Entities
{
    public class PlayerDataStore
    {
        public Health PlayerStatus { get; set; } = new Health();
        
        public MissionManager MissionManager { get; set; } = new MissionManager();

        public Inventory Inventory { get; set; } = new Inventory();

    }
}