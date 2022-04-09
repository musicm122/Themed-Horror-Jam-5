using Godot;
using ThemedHorrorJam5.Scripts.GDUtils;
using ThemedHorrorJam5.Scripts.ItemComponents;

public class Player : KinematicBody2D, IDebuggable<Node>
{
    [Export]
    public bool IsDebugging { get; set; } = false;

    public bool IsDebugPrintEnabled() => IsDebugging;

    public Sprite ExaminableNotification { get; set; }

    public PauseMenu PauseMenu { get; set; }

    public MissionManager MissionManager { get; set; } = new MissionManager();

    public Inventory Inventory { get; set; } = new Inventory();

    public bool IsHidden = false;

    public bool CanInteract { get; set; } = false;

    [Export]
    public float MoveSpeed { get; set; } = 50f;

    [Export]
    public float PushSpeed { get; set; } = 20f;

    [Export]
    public float MoveMultiplier { get; set; } = 1.5f;

    public bool CanMove = true;
    public bool IsRunning = false;

    private void RegisterSignals()
    {
        this.PrintCaller();

        this.Print("Start Register Signal");
        this.Print("================");
        var examinableCollection = this.GetTree().GetExaminableCollection();
        var lockedDoorCollection = this.GetTree().GetLockedDoorCollection();

        this.Print("Examinable count = ", examinableCollection.Count);
        if (!examinableCollection.IsNullOrEmpty())
        {
            examinableCollection.ForEach(e => e.Connect(CustomSignals.PlayerInteracting, this, nameof(OnExaminablePlayerInteracting)));
            examinableCollection.ForEach(e => e.Connect(CustomSignals.PlayerInteractingComplete, this, nameof(OnExaminablePlayerInteractingComplete)));
            examinableCollection.ForEach(e => e.Connect(CustomSignals.PlayerInteractingAvailable, this, nameof(OnExaminablePlayerInteractionAvailable)));
            examinableCollection.ForEach(e => e.Connect(CustomSignals.PlayerInteractingUnavailable, this, nameof(OnExaminablePlayerInteractionUnavailable)));
        }

        if (!lockedDoorCollection.IsNullOrEmpty())
        {
            lockedDoorCollection.ForEach(e => e.Connect(CustomSignals.DoorInteraction, this, nameof(OnDoorInteraction)));
        }

        this.Print<Node>("================");
        this.Print<Node>("End Register Signal");
    }

    public override void _Ready()
    {
        RegisterSignals();
        ExaminableNotification = GetNode<Sprite>("ExaminableNotification");
        ExaminableNotification.Hide();
        MissionManager.AddMissionEvent += UpdateMissions;
        MissionManager.RemoveMissionEvent += UpdateMissions;
        PauseMenu = GetNode<PauseMenu>("PauseMenu");
        PauseMenu.Hide();
        RefreshUI();
    }

    private void UpdateMissions(object sender, MissionManagerEventArgs args)
    {
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
        this.Print<Node>("Locking Movement");
        this.CanMove = false;
    }

    private void UnlockMovement()
    {
        this.Print<Node>("Unlocking Movement");
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
    }

    public void RemoveMission(Mission mission)
    {
        MissionManager.Remove(mission);
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

    public void OnExaminablePlayerInteractionAvailable(Examinable examinable)
    {
        this.PrintCaller();
        examinable.CanInteract = true;
        ShowExamineNotification();
    }

    public void OnExaminablePlayerInteractionUnavailable(Examinable examinable)
    {
        this.PrintCaller();
        examinable.CanInteract = false;
        HideExamineNotification();
    }

    public void OnExaminablePlayerInteracting(Examinable examinable)
    {
        this.PrintCaller();
        this.LockMovement();
    }

    public void OnExaminablePlayerInteractingComplete(Examinable examinable)
    {
        this.PrintCaller();
        this.UnlockMovement();
    }

    public void OnDoorInteraction(LockedDoor lockedDoor)
    {
        if (HasKey(lockedDoor.RequiredKey) && lockedDoor.CurrentDoorState == DoorState.Locked)
        {
            lockedDoor.CurrentDoorState = DoorState.Closed;
        }
    }

    private void CheckBoxCollision(Vector2 motion)
    {
        motion = motion.Normalized();
        if (GetSlideCollision(0).Collider is PushBlock box)
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