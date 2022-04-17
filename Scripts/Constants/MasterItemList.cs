using System.Collections.Generic;
using Godot;
using ThemedHorrorJam5.Scripts.ItemComponents;

namespace ThemedHorrorJam5.Scripts.Constants
{
    public static class MasterItemList
    {

        public static readonly List<Item> Items = new List<Item>(){
        new Item(){
            Name = "Foo's Glasses",
            Description= "These are some thick lenses",
            ImagePath = ItemConstants.FlashlightImagePath},
        new Item(){
            Name = "Flashlight",
            Description= "Essential for seeing in the dark.",
            ImagePath = ItemConstants.FlashlightImagePath},
        new Item(){
            Name = "Gas Mask",
            Description= "Allows me to survive tear gas",
            ImagePath = ItemConstants.FlashlightImagePath},
        new Item(){
            Name = "Broken cell phone",
            Description= "Maybe it can be fixed",
            ImagePath = ItemConstants.FlashlightImagePath},
        new Item(){
            Name = "Carton of Milk",
            Description= "For use with treating pepper spray",
            ImagePath = ItemConstants.FlashlightImagePath},
        new Item(){
            Name = "Survival Manual",
            Description= "Basics on surviving.",
            ImagePath = ItemConstants.FlashlightImagePath},
        new Item(){
            Name = "HealthKit",
            Description= "Heals damage",
            ImagePath = ItemConstants.FlashlightImagePath},
        new Item(){
            Name = "KeyA",
            Description= "Opens 'A' doors",
            ImagePath = ItemConstants.KeyImagePath},
    };

        public static Item GetItemByName(string name)
        {
            var result = Items.Find(i => i.Name == name);
            if (result == null)
            {
                GD.PrintErr($"GetItemByName({name}) not found");
                throw new ItemNotFoundException(name);
            }
            return result;
        }
    }
}
