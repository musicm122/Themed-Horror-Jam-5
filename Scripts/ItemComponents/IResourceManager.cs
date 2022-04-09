using System.Collections.Generic;

public interface IResourceManager<T>
{
    List<T> Items { get; set; }
    
    bool HasResource(string name);
    bool HasKey(Key key);
    string InventoryDisplay();
    void RemoveItem(string name, int amt = 1);
}