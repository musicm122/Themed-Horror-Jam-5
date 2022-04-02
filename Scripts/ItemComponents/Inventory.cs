using Godot;
using System.Collections.Generic;
using System.Linq;
using static LockedDoor;

public class Inventory
{
    public List<Item> Items { get; set; } = new List<Item>();

    public bool HasItem(string name) => Items.Any(item => item.Name == name);

    public bool HasKey(Key key) => Items.Any(item => item.Name.ToLower() == key.ToString().ToLower());

    public void AddItem(string name, int amt)
    {
        for (int i = 0; i < amt; i++)
        {
            Items.Add(new Item(name));
        }
    }

    public void RemoveItem(string name, int amt = 1)
    {
        GD.Print($@"Attempting to remove {amt} {name}(s) from inventory.");
        Items.RemoveItemIfExists(name, amt);
        GD.Print($@"Removed {amt} {name}(s) from inventory.");
    }

    public string InventoryDisplay()
    {
        var retval = "Items: \r\n=======\r\n";
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
}