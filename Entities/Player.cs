using System.Collections.Generic;
using Godot;
using ThemedHorrorJam5.Entities.Components;
using ThemedHorrorJam5.Scripts.Extensions;
using ThemedHorrorJam5.Scripts.GDUtils;
using ThemedHorrorJam5.Scripts.ItemComponents;
using ThemedHorrorJam5.Scripts.Mission;

namespace ThemedHorrorJam5.Entities
{
    public class Player : KinematicBody2D, IDebuggable<Node>
    {
        [Export]
        public bool IsDebugging { get; set; } = false;

        [Export]
        public float MoveSpeed { get; set; } = 50f;

        [Export]
        public float PushSpeed { get; set; } = 20f;

        [Export]
        public float MoveMultiplier { get; set; } = 1.5f;

        public bool IsDebugPrintEnabled() => IsDebugging;

        public Sprite ExaminableNotification { get; set; }

        public PauseMenu PauseMenu { get; set; }

        public MissionManager MissionManager { get; set; } = new MissionManager();

        public Inventory Inventory { get; set; } = new Inventory();

        private Hurtbox HurtBox { get; set; }

        public bool IsHidden = false;

        public bool CanInteract { get; set; } = false;

        public bool CanMove = true;

        public bool IsRunning = false;

        private void RegisterExaminable(List<Examinable> examinableCollection)
        {
            try
            {
                this.Print("Examinable count = ", examinableCollection.Count);
                if (!examinableCollection.IsNullOrEmpty())
                {
                    examinableCollection.ForEach(e => e.Connect(nameof(Examinable.PlayerInteracting), this, nameof(OnExaminablePlayerInteracting)));
                    examinableCollection.ForEach(e => e.Connect(nameof(Examinable.PlayerInteractingComplete), this, nameof(OnExaminablePlayerInteractingComplete)));
                    examinableCollection.ForEach(e => e.Connect(nameof(Examinable.PlayerInteractingAvailable), this, nameof(OnExaminablePlayerInteractionAvailable)));
                    examinableCollection.ForEach(e => e.Connect(nameof(Examinable.PlayerInteractingUnavailable), this, nameof(OnExaminablePlayerInteractionUnavailable)));
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        private void RegisterLockedDoors(List<LockedDoor> lockedDoorCollection)
        {
            try
            {
                this.Print("lockedDoorCollection count = ", lockedDoorCollection.Count);
                if (!lockedDoorCollection.IsNullOrEmpty())
                {
                    lockedDoorCollection.ForEach(e =>
                        e.Connect(nameof(LockedDoor.DoorInteraction), this, nameof(OnDoorInteraction)));
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        private void RegisterHurtBoxSignals()
        {
            if (!HurtBox.TryConnectSignal("area_entered", this, nameof(OnHurtboxAreaEntered)))
            {
                this.Print("Attempt to register Hurtbox's signal area_entered to player failed");
            }

            if (!HurtBox.TryConnectSignal(nameof(HurtBox.OnHurtboxInvincibilityStarted), this, nameof(OnHurtboxInvincibilityStarted)))
            {
                this.Print($"Attempt to register Hurtbox's signal {nameof(HurtBox.OnHurtboxInvincibilityStarted)} to player failed");
            }

            if (!HurtBox.TryConnectSignal(nameof(HurtBox.OnHurtboxInvincibilityEnded), this, nameof(OnHurtboxInvincibilityEnded)))
            {
                this.Print($"Attempt to register Hurtbox's signal {nameof(HurtBox.OnHurtboxInvincibilityEnded)} to player failed");
            }
        }

        private void RegisterSignals()
        {
            this.PrintCaller();

            this.Print("Start Register Signal");
            this.Print("================");
            RegisterExaminable(this.GetTree().GetExaminableCollection());
            RegisterLockedDoors(this.GetTree().GetLockedDoorCollection());
            RegisterHurtBoxSignals();
            this.Print<Node>("================");
            this.Print<Node>("End Register Signal");
        }

        public override void _Ready()
        {
            ExaminableNotification = GetNode<Sprite>("ExaminableNotification");
            HurtBox = GetNode<Hurtbox>("Hurtbox");
            RegisterSignals();
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
            this.Print("Locking Movement");
            this.CanMove = false;
        }

        private void UnlockMovement()
        {
            this.Print("Unlocking Movement");
            this.CanMove = true;
        }

        public void AddMission(MissionElement mission)
        {
            this.Print($"AddMission called with mission : {mission}");
            MissionManager.AddIfDNE(mission);
        }

        public void RemoveMission(MissionElement mission)
        {
            this.Print($"RemoveMission called with mission : {mission}");
            MissionManager.Remove(mission);
        }

        public void EvaluateMissions()
        {
            MissionManager.EvaluateMissionsState(this);
            RefreshUI();
        }

        public void AddItem(string name, int amt = 1)
        {
            this.Print($"AddItem called with name:{name}, amt:{amt} ");
            Inventory.Add(name, amt);
            RefreshUI();
        }

        public void RemoveItem(string name)
        {
            this.Print($"RemoveItem called with name:{name}");
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
            examinable.CanInteract = false;
            this.PrintCaller();
            this.LockMovement();
        }

        public void OnExaminablePlayerInteractingComplete(Examinable examinable)
        {
            examinable.CanInteract = true;
            this.PrintCaller();
            this.UnlockMovement();
        }

        public void OnDoorInteraction(LockedDoor lockedDoor)
        {
            this.PrintCaller();
            this.Print($"Does player have key {lockedDoor.RequiredKey}", HasKey(lockedDoor.RequiredKey));
            this.Print($"lockedDoor.CurrentDoorState  = {lockedDoor.CurrentDoorState}");

            if (HasKey(lockedDoor.RequiredKey) && lockedDoor.CurrentDoorState == DoorState.Locked)
            {
                lockedDoor.CurrentDoorState = DoorState.Closed;
            }
        }

        private void CheckBoxCollision(Vector2 motion)
        {
            this.PrintCaller();
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

        public void OnHurtboxAreaEntered(Node body)
        {
            this.PrintCaller();
        }

        public void OnHurtboxInvincibilityStarted()
        {
            this.PrintCaller();
        }

        public void OnHurtboxInvincibilityEnded()
        {
            this.PrintCaller();
        }
    }
}