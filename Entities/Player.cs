using Godot;
using ThemedHorrorJam5.Scripts.GDUtils;
using static LockedDoor;

public class Player : KinematicBody2D
{
    public Sprite ExaminableNotification { get; set; }

    public PauseMenu PauseMenu { get; set; }

    public MissionManager MissionManager { get; set; } = new MissionManager();
    public Inventory Inventory { get; set; } = new Inventory();

    public bool IsHidden = false;

    [Export]
    private float PauseToggleCooldownWaitTime = 1.0f;

    private float AccumulatorPauseToggleCooldown = 0.0f;
    private bool CanTogglePause = true;

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
        ExaminableNotification = GetNode<Sprite>("ExaminableNotification");
        ExaminableNotification.Hide();
        PauseMenu = GetNode<PauseMenu>("PauseMenu");
        PauseMenu.Hide();
        RefreshUI();
    }

    public void ShowExamineNotification()
    {
        ExaminableNotification.Show();
    }

    public void HideExamineNotification()
    {
        ExaminableNotification.Hide();
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
        Inventory.Add(name, amt);
        RefreshUI();
    }

    public void AddMission(Mission mission)
    {
        MissionManager.AddIfDNE(mission);
        RefreshUI();
    }

    public void EvaluateMissions()
    {
        MissionManager.EvaluateMissionsState(this);
        RefreshUI();
    }

    public void RemoveItem(string name, int amt = 1)
    {
        Inventory.Remove(name);
        RefreshUI();
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
                CheckBoxCollision(movement);
            }
        }
    }

    public void DialogListenerCallback(string val)
    {
        GD.Print("Player.DialogListenerCallback called with arg ", val);
    }

    public void OnExaminablePlayerInteracting()
    {
        this.LockMovement();
    }

    public void OnExaminablePlayerInteractingComplete()
    {
        this.UnlockMovement();
    }

    private void CheckBoxCollision(Vector2 motion)
    {
        motion = motion.Normalized();
        var box = GetSlideCollision(0).Collider as PushBlock;
        if (box != null)
        {
            box.Push(PushSpeed * motion);
        }
    }

    public bool HasKey(Key key) => Inventory.HasKey(key);

    private void RefreshUI()
    {
        this.PauseMenu.RefreshUI(this);
    }
}