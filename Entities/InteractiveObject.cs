using Godot;
using ThemedHorrorJam5.Scripts.GDUtils;
using ThemedHorrorJam5.Scripts.ItemComponents;

public class InteractiveObject : Node2D, IDebuggable<Node>
{
    [Export]
    public bool IsDebugging { get; set; } = false;
    public bool IsDebugPrintEnabled() => IsDebugging;
    public bool CanInteract = false;
    public bool IsInteracting = false;

    //public Sprite Sprite { get; set; }

    public void OnEntered(Node body)
    {
        CanInteract = body.Name == "Player";
    }

    public void OnExited(Node body)
    {
        CanInteract = !(body.Name == "Player");
    }

    public virtual void OnInteract()
    {
        if (!IsInteracting)
        {
            IsInteracting = true;
            GD.Print("Interacting with thing");
            OnInteractComplete();
        }
    }

    public virtual void OnInteractComplete()
    {
        IsInteracting = false;
    }

    public override void _Process(float delta)
    {
        if (CanInteract && InputUtils.IsInteracting())
        {
            OnInteract();
        }
    }
}
