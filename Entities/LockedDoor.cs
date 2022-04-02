using Godot;
using ThemedHorrorJam5.Scripts.GDUtils;

public partial class LockedDoor : Node2D
{
    //[Export]
    //public string RequiredKeyName { get; set; }

    private const string ClosedAnimation = "Close";
    private const string OpenAnimation = "Open";
    private const string IdleAnimation = "Idle";
    public bool CanInteract { get; set; }

    [Export]
    public Key RequiredKey { get; set; }

    [Export]
    public DoorState CurrentDoorState { get; set; } = DoorState.Locked;

    private Player PlayerRef { get; set; }

    public AnimatedSprite Sprite { get; set; }

    public CollisionShape2D Collider { get; set; }

    public void OpenDoor()
    {
        GD.Print($"Opening Door");
        Sprite.Play(OpenAnimation);
        CurrentDoorState = DoorState.Opened;
        this.Collider.Disabled = true;
        GD.Print($"Door is now opened");
    }

    public void CloseDoor()
    {
        GD.Print($"Closing Door");
        Sprite.Play(ClosedAnimation);
        CurrentDoorState = DoorState.Closed;
        this.Collider.Disabled = false;
        GD.Print($"Door is now closed");
    }

    public void StartLockedDoorInteraction()
    {
        GD.Print($"Door is locked and requires {RequiredKey.ToString()}");
    }

    public override void _Ready()
    {
        Sprite = (AnimatedSprite)GetNode("AnimatedSprite");
        Sprite.Connect("animation_finished", this, nameof(OnAnimationFinished));
        Collider = (CollisionShape2D)GetNode("Collider/ColliderShape2d");
        if (CurrentDoorState == DoorState.Closed || CurrentDoorState == DoorState.Locked)
        {
            this.Collider.Disabled = true;
        }
    }

    private void OnAnimationFinished()
    {
        if (Sprite.Animation == OpenAnimation)
        {
            Sprite.Stop();
            Sprite.Frame = 3;
        }
        if (Sprite.Animation == ClosedAnimation)
        {
            Sprite.Stop();
            Sprite.Play(IdleAnimation);
        }
    }

    public void OnDoorBodyEntered(Node2D body)
    {
        if (body.Name.ToLower() == "player")
        {
            PlayerRef = (Player)body;
            this.CanInteract = true;
            PlayerRef.ShowExamineNotification();
        }
    }

    public void OnDoorBodyExited(Node2D body)
    {
        if (body.Name.ToLower() == "player")
        {
            PlayerRef = (Player)body;
            this.CanInteract = false;
            PlayerRef.HideExamineNotification();
        }
    }

    private bool IsInteracting() => InputUtils.IsInteracting();

    private void OnInteract()
    {
        GD.Print($"LockedDoor.OnOnInteract called with the current door state set to {CurrentDoorState}");
        switch (CurrentDoorState)
        {
            case DoorState.Locked:
                GD.Print($"PlayerRef.HasKey({RequiredKey})", PlayerRef.HasKey(RequiredKey));
                if (PlayerRef.HasKey(RequiredKey))
                {
                    OpenDoor();
                }
                else
                {
                    StartLockedDoorInteraction();
                }
                break;

            case DoorState.Closed:
                OpenDoor();
                break;

            case DoorState.Opened:
                CloseDoor();
                break;
        }
    }

    public override void _Process(float delta)
    {
        if (IsInteracting())
        {
            GD.Print("Interacting with door...");
            GD.Print("PlayerRef != null = ", PlayerRef != null);
            GD.Print("CanInteract= ", CanInteract);
        }
        if (PlayerRef != null && CanInteract && IsInteracting())
        {
            GD.Print("Interacting with door...");
            OnInteract();
        }
    }
}