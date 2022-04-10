using System.Collections.Generic;

public static class ItemConstants
{
    public const string PizzaImagePath = "res://Assets/Art/Food/pizzaslice.png";
    public const string FlashlightImagePath = "res://Assets/Art/Item/Flashlight.png";
    public const string KeyImagePath = "res://Assets/Art/Item/Keychain.png";

    public static readonly Dictionary<string, string> ItemImagePaths = new Dictionary<string, string>
    {
        ["pizza"] = PizzaImagePath,
        ["flashlight"] = FlashlightImagePath,
        ["keya"] = KeyImagePath,
    };
}