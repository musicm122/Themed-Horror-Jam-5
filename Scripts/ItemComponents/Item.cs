using System;
using Godot;
using ThemedHorrorJam5.Scripts.Constants;

namespace ThemedHorrorJam5.Scripts.ItemComponents
{
    [Serializable]
    public class Item
    {
        public Item() { }

        public Item(string name)
        {
            var temp = MasterItemList.GetItemByName(name);
            Name = name;
            Description = temp.Description;
            ImagePath = temp.ImagePath;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }

        public string ImagePathLookup(string name) =>
            ItemConstants.ItemImagePaths[name.ToLowerInvariant()];
    }
}