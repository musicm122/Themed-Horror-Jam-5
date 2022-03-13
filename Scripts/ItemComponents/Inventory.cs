using Godot;
using System.Collections.Generic;

public class Inventory
{
    public List<Item> Items { get; set; } = new List<Item>();

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
}