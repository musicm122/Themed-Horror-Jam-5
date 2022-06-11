using System;
using System.Collections.Generic;
using ThemedHorrorJam5.Scripts.Enum;

namespace ThemedHorrorJam5.Scripts.ItemComponents
{
    public interface IInventory
    {
        event EventHandler<InventoryEventArgs> AddItemEvent;
        event EventHandler<InventoryEventArgs> RemoveItemEvent;

        void Add(string name, int amt);
        int Count();
        int CountOfType(string name);
        IEnumerable<string> Descriptions();
        string Display();
        Item GetItem(string itemName);
        bool HasItem(string itemName);
        bool HasItemInInventory(string itemName);
        bool HasItemWithAtLeast(string itemName, int amt);
        bool HasKey(Key key);
        bool HasResource(string name);
        IEnumerable<string> ImagePaths();
        IEnumerable<string> Names();
        void Remove(string name);
        void RemoveAmount(string name, int amt = 1);
        bool RemoveItemIfExists(string itemName);
    }
}