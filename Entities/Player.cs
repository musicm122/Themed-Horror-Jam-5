using Godot;
using ThemedHorrorJam5.Scripts.GDUtils;

public class Player : KinematicBody2D
{
    public Inventory Inventory { get; set; } = new Inventory();

    public bool IsHidden = false;

    [Export]
    public float MoveSpeed { get; set; } = 50f;

    [Export]
    public float PushSpeed { get; set; } = 20f;

    [Export]
    public float MoveMultiplier { get; set; } = 1.5f;

    public bool CanMove = true;
    public bool IsRunning = false;

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

    public void AddItem(string name, int amt = 1)
    {
        GD.PrintT($"AddItem", name, amt);
        Inventory.AddItem(name, amt);
    }

    public void RemoveItem(string name, int amt = 1)
    {
        GD.PrintT($"RemoveItem", name, amt);
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
            var movement = MoveCheck(IsRunning);
            this.MoveAndSlide(movement);
            if (GetSlideCount() > 0)
            {
                GD.Print("Slide count is greater than 0");
                CheckBoxCollision(movement);
            }
        }
    }

    public void DialogListenerCallback(string val)
    {
        GD.Print("DialogListenerCallback called with arg ", val);
    }

    public void OnExaminablePlayerInteracting()
    {
        GD.Print("----------------------------");
        GD.Print("OnExaminablePlayerInteracting");
        GD.Print("----------------------------");
        //var new_dialog = Dialogic.start('Your Timeline Name Here') add_child(new_dialog)
        this.LockMovement();
    }

    public void OnExaminablePlayerInteractingComplete()
    {
        GD.Print("OnExaminablePlayerInteractingComplete");
        this.UnlockMovement();
    }

    private void CheckBoxCollision(Vector2 motion)
    {
        motion = motion.Normalized();
        //var sumOfAbs = Mathf.Abs(motion.x) + Mathf.Abs(motion.y);
        //if (sumOfAbs > 1) return;
        var box = GetSlideCollision(0).Collider as PushBlock;
        if (box != null)
        {
            box.Push(PushSpeed * motion);
        }
    }
}