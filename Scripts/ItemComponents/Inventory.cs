using Godot;
using System.Collections.Generic;
using System.Linq;
using ThemedHorrorJam5.Scripts.Enum;
using ThemedHorrorJam5.Scripts.Extensions;

namespace ThemedHorrorJam5.Scripts.ItemComponents
{
    public class Inventory
    {
        public delegate void AddingItemHandler(object sender, InventoryEventArgs args);

        public event System.EventHandler<InventoryEventArgs> AddItemEvent;

        public delegate void RemovingItemHandler(object sender, InventoryEventArgs args);

        public event System.EventHandler<InventoryEventArgs> RemoveItemEvent;

        protected virtual void RaiseAddingItem(Item item)
        {
            AddItemEvent?.Invoke(this, new InventoryEventArgs(item));
        }

        protected virtual void RaiseRemovingItem(Item item)
        {
            RemoveItemEvent?.Invoke(this, new InventoryEventArgs(item));
        }
        protected virtual void RaiseRemovingItem(Item item, int amt)
        {
            RemoveItemEvent?.Invoke(this, new InventoryEventArgs(item, amt));
        }

        private List<Item> Items { get; } = new List<Item>();

        public bool HasResource(string name) => Items.Any(item => item.Name == name);

        public bool HasKey(Key key) => Items.Any(item => item.Name.ToLower() == (key.ToString().ToLower()));

        public void Add(string name, int amt)
        {
            for (int i = 0; i < amt; i++)
            {
                var item = new Item(name);
                RaiseAddingItem(item);
                Items.Add(item);
            }
        }

        public void RemoveAmount(string name, int amt = 1)
        {
            Items.RemoveAmt(i => i.Name == name, amt);
            RaiseRemovingItem(new Item(name), amt);
        }

        public void Remove(string name)
        {
            Items.RemoveOne(i => i.Name == name);
            RaiseRemovingItem(new Item(name));
        }

        public IEnumerable<string> Names() =>
            Items.Select(i => i.Name);

        public IEnumerable<string> Descriptions() =>
            Items.Select(i => i.Description);

        public IEnumerable<string> ImagePaths() =>
            Items.Select(i => i.ImagePath);

        public int Count() =>
            Items.Count;

        public int CountOfType(string name) =>
            Items.Count(i => i.Name.Equals(name));

        public string Display()
        {
            var retval = "Items:\r\n=======\r\n";
            if (Items.Count > 0)
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    retval += Items[i].Name + "\r\n";
                }
            }
            else
            {
                retval += "Empty\r\n";
            }
            return retval;
        }

        public bool HasItemWithAtLeast(string itemName, int amt)
            => Items.Count(i => i.Name.Trim().Equals(itemName.Trim())) >= amt;

        public bool HasItemInInventory(string itemName)
            => Items.Any(i => i.Name.Trim().Equals(itemName.Trim()));

        public Item GetItem(string itemName) =>
            Items.First(i => i.Name.Trim().Equals(itemName.Trim()));

        public bool HasItem(string itemName) =>
            Items.Any(i => i.Name.Trim().Equals(itemName.Trim()));

        public bool RemoveItemIfExists(string itemName)
        {
            if (Items.Any(i => i.Name.Trim().Equals(itemName.Trim())))
            {
                var item = Items.First(i => i.Name == itemName);
                RaiseRemovingItem(item);
                Items.Remove(item);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}