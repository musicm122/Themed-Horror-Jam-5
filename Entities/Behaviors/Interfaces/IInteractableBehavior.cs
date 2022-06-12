using Godot;
using System;
using System.Collections.Generic;
using ThemedHorrorJam5.Scripts.Enum;
using ThemedHorrorJam5.Scripts.ItemComponents;

namespace ThemedHorrorJam5.Entities.Components
{
    public interface IInteractableBehavior
    {

        Action<Examinable> InteractingCallback { get; set; }
        Action<Examinable> InteractingCompleteCallback { get; set; }
        Action<Examinable> InteractingAvailableCallback { get; set; }
        Action<Examinable> InteractingUnavailableCallback { get; set; }

        bool CanInteract { get; set; }
        Sprite ExaminableNotification { get; set; }
        bool IsDebugging { get; set; }
        PlayerState State { get; set; }

        bool HasItem(string itemName);
        bool HasKey(Key key);
        void HideExamineNotification();
        void Init(PlayerState state);
        bool IsDebugPrintEnabled();
        void OnDoorInteraction(LockedDoor lockedDoor);
        void OnDoorInteractionComplete(LockedDoor lockedDoor);
        void OnInteractingComplete(Examinable examinable);
        void OnInteractionAvailable(Examinable examinable);
        void OnInteractionBegin(Examinable examinable);
        void OnInteractionUnavailable(Examinable examinable);
        void RegisterExaminable(List<Examinable> examinableCollection);
        void ShowExamineNotification();
        void _Ready();
    }
}