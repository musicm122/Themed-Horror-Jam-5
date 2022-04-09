using System.Xml.Linq;
using Godot;
using System.Collections.Generic;
using System.Linq;
using static LockedDoor;

public class Inventory
{
    public delegate void AddingItemHandler(object sender, InventoryEventArgs args);

    public event AddingItemHandler AddItemEvent;

    public delegate void RemovingItemHandler(object sender, InventoryEventArgs args);

    public event RemovingItemHandler RemoveItemEvent;

    protected virtual void RaiseAddingItem(Item item)
    {
        AddItemEvent?.Invoke(this, new InventoryEventArgs(item));
    }

    protected virtual void RaiseRemovingItem(Item item)
    {
        RemoveItemEvent?.Invoke(this, new InventoryEventArgs(item));
    }

    private List<Item> Items { get; set; } = new List<Item>();

    public bool HasResource(string name) => Items.Any(item => item.Name == name);

    public bool HasKey(Key key) => Items.Any(item => item.Name.ToLower() == key.ToString().ToLower());

    public void Add(string name, int amt)
    {
        for (int i = 0; i < amt; i++)
        {
            var item = new Item(name);
            RaiseAddingItem(item);
            Items.Add(item);
        }
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
        Items.Count();

    public int CountOfType(string name) =>
        Items.Count(i => i.Name.ToLowerInvariant() == name.ToLowerInvariant());

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

    public bool HasItemInInventory(string itemName)
        => Items.Any(i => i.Name.Trim().ToLowerInvariant() == itemName.Trim().ToLowerInvariant());

    private void RemoveItemIfExists(string itemName, int amt)
    {
        while (amt > 0)
        {
            if (HasItemInInventory(itemName))
            {
                var item = Items.Find(i => i.Name == itemName);
                RaiseRemovingItem(item);
                Items.Remove(item);
                amt--;
            }
        }
    }

    public Item GetItem(string itemName) =>
        Items.First(i => i.Name.Trim().ToLowerInvariant() == itemName.Trim().ToLowerInvariant());

    public bool HasItem(string itemName) =>
        Items.Any(i => i.Name.Trim().ToLowerInvariant() == itemName.Trim().ToLowerInvariant());

    public bool RemoveItemIfExists(string itemName)
    {
        if (Items.Any(i => i.Name.Trim().ToLowerInvariant() == itemName.Trim().ToLowerInvariant()))
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

