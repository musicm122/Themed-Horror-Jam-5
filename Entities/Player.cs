using Godot;
using ThemedHorrorJam5.Scripts.GDUtils;

public class Player : KinematicBody2D
{
    public Inventory Inventory { get; set; } = new Inventory();

    public bool IsHidden = false;

    [Export]
    public float MoveSpeed { get; set; } = 50f;

    [Export]
    public float MoveMultiplier { get; set; } = 1.5f;

    public bool CanMove = true;
    public bool IsRunning = false;

    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    private void LockMovement()
    {
        GD.Print("Locking Movement");
        this.CanMove = false;
    }

    private void UnlockMovement()
    {
        GD.Print("Unlocking Movement");
        this.CanMove = true;
    }

    public void OnExaminablePlayerInteracting()
    {
        GD.Print("OnExaminablePlayerInteracting");
        this.LockMovement();
    }

    public void OnExaminablePlayerInteractingComplete()
    {
        GD.Print("OnExaminablePlayerInteractingComplete");
        this.UnlockMovement();
    }

    public void AddItem(string name, int amt = 1)
    {
        Inventory.AddItem(name, amt);
    }

    public void RemoveItem(string name, int amt = 1)
    {
        Inventory.RemoveItem(name);
    }

    private Vector2 MoveCheck(bool isRunning = false) =>
        isRunning ?
            InputUtils.GetTopDownWithDiagMovementInput(MoveSpeed * MoveMultiplier) :
            InputUtils.GetTopDownWithDiagMovementInput(MoveSpeed);

    public override void _PhysicsProcess(float delta)
    {
        IsRunning = Input.IsActionPressed(InputAction.Run);

        if (CanMove)
        {
            this.MoveAndSlide(MoveCheck(IsRunning));
        }
    }
}