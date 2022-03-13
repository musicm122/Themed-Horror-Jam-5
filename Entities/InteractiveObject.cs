using Godot;
using ThemedHorrorJam5.Scripts.GDUtils;

public class InteractiveObject : Node2D
{
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

    //public override void _Ready()
    //{
    //    Connect("dialog_listener", this, "DialogListenerCallback");
    //}

    //void DialogListenerCallback(string arg) 
    //{
    //    if (arg == "Flashlight") 
    //    {
    //        this.QueueFree();
    //    }
    //}

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
