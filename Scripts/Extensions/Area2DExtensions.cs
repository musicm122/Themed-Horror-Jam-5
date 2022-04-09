using Godot;

public static class Area2DExtensions
{
    public static bool ConnectBodyEntered(this Area2D area2D, string methodName)
    {
        return Godot.Error.Ok == area2D.Connect("body_entered", area2D, methodName);
    }

    public static bool ConnectBodyExited(this Area2D area2D, string methodName)
    {
        return Godot.Error.Ok == area2D.Connect("body_exited", area2D, methodName);
    }

    public static void DisconnectBodyEntered(this Area2D area2D, string methodName)
    {
        area2D.Disconnect("body_entered", area2D, methodName);
    }

    public static void DisconnectBodyExited(this Area2D area2D, string methodName)
    {
        area2D.Disconnect("body_exited", area2D, methodName);
    }
}

