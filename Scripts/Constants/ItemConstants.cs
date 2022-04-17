using System.Collections.Generic;

namespace ThemedHorrorJam5.Scripts.Constants
{
    public static class ItemConstants
    {
        public const string PizzaImagePath = "res://Assets/Art/Food/pizzaslice.png";
        public const string FlashlightImagePath = "res://Assets/Art/Item/Flashlight.png";
        public const string HealthkitImagePath = "res://Assets/Art/Item/genericItem_color_102.png";
        public const string KeyImagePath = "res://Assets/Art/Item/genericItem_color_155.png";

        public static readonly Dictionary<string, string> ItemImagePaths = new Dictionary<string, string>
        {
            ["Pizza"] = PizzaImagePath,
            ["Flashlight"] = FlashlightImagePath,
            ["KeyA"] = KeyImagePath,
            ["Healthkit"] = HealthkitImagePath
        };
    }
}