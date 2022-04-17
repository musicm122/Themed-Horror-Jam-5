namespace ThemedHorrorJam5.Scripts.ItemComponents
{
    public class InventoryEventArgs
    {
        public Item Item { get; set; }

        public InventoryEventArgs(Item item) => Item = item;
    }
}