using System;
using System.Collections.Generic;
using Godot;
using ThemedHorrorJam5.Scripts.Enum;
using ThemedHorrorJam5.Scripts.Extensions;
using ThemedHorrorJam5.Scripts.GDUtils;
using ThemedHorrorJam5.Scripts.ItemComponents;

namespace ThemedHorrorJam5.Entities.Components
{
    public class InteractableBehavior : Node2D, IDebuggable<Node>
    {

        public PlayerState State { get; set; }

        public Action<Examinable> InteractingCallback;
        public Action<Examinable> InteractingCompleteCallback;
        public Action<Examinable> InteractingAvailableCallback;
        public Action<Examinable> InteractingUnavailableCallback;

        public bool HasKey(Key key) => State.Inventory.HasKey(key);
        public bool HasItem(string itemName) => State.Inventory.HasItem(itemName);

        [Export]
        public bool IsDebugging { get; set; } = false;

        public bool IsDebugPrintEnabled() => IsDebugging;

        public Sprite ExaminableNotification { get; set; }
        public bool CanInteract { get; set; } = false;

        public void RegisterExaminable(List<Examinable> examinableCollection)
        {
            try
            {
                this.Print("Examinable count = ", examinableCollection.Count);
                if (!examinableCollection.IsNullOrEmpty())
                {
                    examinableCollection.ForEach(e => e.Connect(nameof(Examinable.PlayerInteracting), this, nameof(OnInteractionBegin)));
                    examinableCollection.ForEach(e => e.Connect(nameof(Examinable.PlayerInteractingComplete), this, nameof(OnInteractingComplete)));
                    examinableCollection.ForEach(e => e.Connect(nameof(Examinable.PlayerInteractingAvailable), this, nameof(OnInteractionAvailable)));
                    examinableCollection.ForEach(e => e.Connect(nameof(Examinable.PlayerInteractingUnavailable), this, nameof(OnInteractionUnavailable)));
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

        public override void _Ready()
        {
            ExaminableNotification = GetNode<Sprite>("ExaminableNotification");
            RegisterExaminable(this.GetTree().GetExaminableCollection());
            RegisterLockedDoors(this.GetTree().GetLockedDoorCollection());
            ExaminableNotification.Hide();
        }

        public void ShowExamineNotification()
        {
            ExaminableNotification.Show();
        }

        public void HideExamineNotification()
        {
            ExaminableNotification.Hide();
        }

        public void OnInteractionAvailable(Examinable examinable)
        {
            this.PrintCaller();
            CanInteract = true;
            examinable.CanInteract = true;
            ShowExamineNotification();
        }

        public void OnInteractionUnavailable(Examinable examinable)
        {
            this.PrintCaller();
            CanInteract = false;
            examinable.CanInteract = false;
            HideExamineNotification();
        }

        public void OnInteractionBegin(Examinable examinable)
        {
            this.PrintCaller();
            examinable.CanInteract = false;
            CanInteract = false;
            this.InteractingCallback?.Invoke(examinable);
        }

        public void OnInteractingComplete(Examinable examinable)
        {
            examinable.CanInteract = true;
            CanInteract = true;
            this.InteractingCompleteCallback?.Invoke(examinable);
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

        public void Init(PlayerState state)
        {
            this.State = state;
        }
    }
}