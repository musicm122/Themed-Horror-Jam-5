using Godot;
using ThemedHorrorJam5.Scripts.GDUtils;
using ThemedHorrorJam5.Scripts.ItemComponents;

public class LockedDoor : Examinable
{
    private const string ClosedAnimation = "Close";
    private const string OpenAnimation = "Open";
    private const string IdleAnimation = "Idle";

    [Signal]
    public delegate void DoorInteraction(LockedDoor lockedDoor);

    [Export]
    public Key RequiredKey { get; set; }

    [Export]
    public DoorState CurrentDoorState { get; set; } = DoorState.Locked;

    public AnimatedSprite Sprite { get; set; }

    public CollisionShape2D Collider { get; set; }

    public void OpenDoor()
    {
        this.Print($"Opening Door");
        Sprite.Play(OpenAnimation);
        CurrentDoorState = DoorState.Opened;
        this.Collider.Disabled = true;
        this.Print($"Door is now opened");
    }

    public void CloseDoor()
    {
        this.Print($"Closing Door");
        Sprite.Play(ClosedAnimation);
        CurrentDoorState = DoorState.Closed;
        this.Collider.Disabled = false;
        this.Print($"Door is now closed");
    }

    public void StartLockedDoorInteraction()
    {
        this.Print($"Door is locked and requires {RequiredKey}");
    }

    public override void _Ready()
    {
        Sprite = GetNode<AnimatedSprite>("AnimatedSprite");
        Sprite.Connect("animation_finished", this, nameof(OnAnimationFinished));
        Collider = GetNode<CollisionShape2D>("Collider/ColliderShape2d");
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
            Sprite.Frame = 3;
        }
    }

    public void OnDoorBodyEntered(Node2D body)
    {
        if (body.Name.ToLower() == "player")
        {
            //PlayerRef = (Player)body;
            this.CanInteract = true;
            //PlayerRef.ShowExamineNotification();
        }
    }

    public void OnDoorBodyExited(Node2D body)
    {
        if (body.Name.ToLower() == "player")
        {
            //PlayerRef = (Player)body;
            this.CanInteract = false;
            //PlayerRef.HideExamineNotification();
        }
    }

    private bool IsInteracting() => InputUtils.IsInteracting();

    protected override void OnInteract()
    {
        base.OnInteract();
        this.Print($"LockedDoor.OnOnInteract called with the current door state set to {CurrentDoorState}");
        EmitSignal(nameof(DoorInteraction), this);
        switch (CurrentDoorState)
        {
            case DoorState.Locked:
                //this.Print($"PlayerRef.HasKey({RequiredKey})", PlayerRef.HasKey(RequiredKey));
                //if (PlayerRef.HasKey(RequiredKey))
                //{
                //    OpenDoor();
                //}
                //else
                //{
                //    StartLockedDoorInteraction();
                //}
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
            this.Print("Interacting with door...");
            //this.Print("PlayerRef != null = ", PlayerRef != null);
            this.Print("CanInteract= ", CanInteract);
        }
        //if (PlayerRef != null && CanInteract && IsInteracting())
        //{
        //    this.Print("Interacting with door...");
        //    OnInteract();
        //}
    }
}