using System.Collections.Generic;
using Godot;
using ThemedHorrorJam5.Entities.Components;
using ThemedHorrorJam5.Scripts.Constants;
using ThemedHorrorJam5.Scripts.Enum;
using ThemedHorrorJam5.Scripts.Extensions;
using ThemedHorrorJam5.Scripts.GDUtils;
using ThemedHorrorJam5.Scripts.ItemComponents;
using ThemedHorrorJam5.Scripts.Mission;
using ThemedHorrorJam5.Scripts.UI;
using static ThemedHorrorJam5.Entities.Components.Hurtbox;
using static ThemedHorrorJam5.Entities.Components.Status;

namespace ThemedHorrorJam5.Entities
{
    public class Player : KinematicBody2D, IDebuggable<Node>
    {
        public bool IsDead = false;
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

        private Hud Hud { get; set; }

        public Status Status { get; set; }

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

        private void RegisterHealthSignals()
        {
            Status.Connect(nameof(NoHealth), this, nameof(OnEmptyHealthBar));
            Status.Connect(nameof(HealthChanged), this, nameof(OnHealthChanged));
            Status.Connect(nameof(MaxHealthChanged), this, nameof(OnMaxHealthChanged));
        }

        private void RegisterHurtBoxSignals()
        {
            if (!HurtBox.TryConnectSignal("area_entered", this, nameof(OnHurtboxAreaEntered)))
            {
                var arg = $"TryConnectSignal('area_entered', {this.Name}, {nameof(OnHurtboxAreaEntered)})";
                this.Print($"Attempt to register Hurtbox's signal with args {arg} to player failed");
            }

            if (!HurtBox.TryConnectSignal(nameof(InvincibilityStarted), this, nameof(OnHurtboxInvincibilityStarted)))
            {
                var arg = $"TryConnectSignal({nameof(InvincibilityStarted)}, {this.Name}, {nameof(OnHurtboxInvincibilityStarted)})";
                this.Print($"Attempt to register Hurtbox's signal with args {arg} to player failed");
            }

            if (!HurtBox.TryConnectSignal(nameof(InvincibilityEnded), this, nameof(OnHurtboxInvincibilityEnded)))
            {
                var arg = $"TryConnectSignal({nameof(InvincibilityEnded)}, {this.Name}, {nameof(OnHurtboxInvincibilityEnded)})";
                this.Print($"Attempt to register Hurtbox's signal with args {arg} to player failed");
            }
        }

        private void RegisterSignals()
        {
            this.PrintCaller();

            this.Print("Start Register Signal");
            this.Print("================");
            RegisterHealthSignals();
            RegisterExaminable(this.GetTree().GetExaminableCollection());
            RegisterLockedDoors(this.GetTree().GetLockedDoorCollection());
            RegisterHurtBoxSignals();
            this.Print("================");
            this.Print("End Register Signal");
        }

        public override void _Ready()
        {
            ExaminableNotification = GetNode<Sprite>("ExaminableNotification");
            HurtBox = GetNode<Hurtbox>("Hurtbox");
            Status = GetNode<Status>("PlayerStats");
            RegisterSignals();
            ExaminableNotification.Hide();
            MissionManager.AddMissionEvent += UpdateMissions;
            MissionManager.RemoveMissionEvent += UpdateMissions;
            Hud = GetNode<Hud>("Camera2D/Hud");
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

        public void AddMission(string title)
        {
            var mission = MasterMissionList.GetMissionByTitle(title);
            if (mission != null)
            {
                this.Print($"AddMission called with mission title: {title}");
                MissionManager.AddIfDNE(mission);
                RefreshUI();
            }

        }

        public void RemoveMission(MissionElement mission)
        {
            this.Print($"RemoveMission called with mission : {mission}");
            MissionManager.Remove(mission);
            RefreshUI();
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

        public override void _Process(float delta)
        {
            if (IsDead)
            {
                PauseMenu.IsPauseOptionEnabled = false;
                if (InputUtils.IsAnyKeyPressed())
                {
                    this.Print("Reloading Scene");
                    GetTree().ReloadCurrentScene();
                }
            }
        }

        public override void _PhysicsProcess(float delta)
        {
            if (CanMove && !IsDead)
            {
                IsRunning = Input.IsActionPressed(InputAction.Run);
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
        public bool HasItem(string itemName) => Inventory.HasItem(itemName);

        private void RefreshUI()
        {
            Hud.RefreshUI(this);
            this.PauseMenu.RefreshUI(this);
        }

        public void OnHurtboxAreaEntered(Node body)
        {

            this.Print($"OnHurtboxAreaEntered({body.Name})");
            if (body.Name == "HitBox" || body.Name == "Spikes")
            {
                this.HurtBox.StartInvincibility(0.6f);
                var hitBox = (HitBox)body;
                var force = (this.Position - hitBox.Position) * hitBox.EffectForce;
                this.MoveAndSlide(force);
                Status.CurrentHealth -= hitBox.Damage;
                this.Print("Current Health =", Status.CurrentHealth);

                //Hurtbox.create_hit_effect()
                //var playerHurtSound = PlayerHurtSound.instance()
                //get_tree().current_scene.add_child(playerHurtSound)
            }
        }

        public void OnEmptyHealthBar()
        {
            this.PrintCaller();
            RefreshUI();
            IsDead = true;
            LockMovement();
        }

        public void OnHurtboxInvincibilityStarted()
        {
            this.PrintCaller();
        }

        public void OnHurtboxInvincibilityEnded()
        {
            this.PrintCaller();
        }

        public void OnHealthChanged(int health)
        {
            RefreshUI();
        }

        public void OnMaxHealthChanged(int health)
        {
            RefreshUI();
        }

    }
}