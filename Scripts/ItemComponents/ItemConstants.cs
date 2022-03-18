using System.Collections.Generic;

public class ItemConstants
{
    public const string PizzaImagePath = @"res://Assets/Art/Food/pizzaslice.png";
    public const string FlashlightImagePath = @"res://Assets/Art/Item/Flashlight.png";

    public static readonly Dictionary<string, string> ItemImagePaths = new Dictionary<string, string>
    {
        ["pizza"] = PizzaImagePath,
        ["flashlight"] = FlashlightImagePath
    };
}