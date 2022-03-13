public class Item
{
    public Item(string name)
    {
        this.Name = name.ToLowerInvariant();
        this.ImagePath = ImagePathLookup(name);
    }

    public string Name { get; set; }
    public string ImagePath { get; set; }

    public string ImagePathLookup(string name) =>
        ItemConstants.ItemImagePaths[name.ToLowerInvariant()];
}
