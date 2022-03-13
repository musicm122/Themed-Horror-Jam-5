using System.Collections.Generic;
using System.Linq;

public static class ItemExtensions
{
    public static Item GetItem(this List<Item> items, string itemName) =>
        items.First(i => i.Name.Trim().ToLowerInvariant() == itemName.Trim().ToLowerInvariant());

    public static bool HasItem(this List<Item> items, string itemName) =>
        items.Any(i => i.Name.Trim().ToLowerInvariant() == itemName.Trim().ToLowerInvariant());

    public static void RemoveItem(this List<Item> items, string itemName) =>
        items.Remove(items.GetItem(itemName));

    public static bool RemoveItemIfExists(this List<Item> items, string itemName)
    {
        if (items.Any(i => i.Name.Trim().ToLowerInvariant() == itemName.Trim().ToLowerInvariant()))
        {
            items.Remove(items.GetItem(itemName));
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool HasItemInInventory(this List<Item> items, string itemName)
        => items.Any(i => i.Name.Trim().ToLowerInvariant() == itemName.Trim().ToLowerInvariant());

    public static void RemoveItemIfExists(this List<Item> items, string itemName, int amt)
    {
        while (amt > 0)
        {
            if (items.HasItemInInventory(itemName))
            {
                items.Remove(items.GetItem(itemName));
                amt--;
            }
        }
    }
}