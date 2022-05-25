﻿using System.Diagnostics;
using Godot;
using ThemedHorrorJam5.Entities.Components;
using ThemedHorrorJam5.Scripts.ItemComponents;

namespace ThemedHorrorJam5.Entities
{
    public class PlayerV2 : KinematicBody2D, IDebuggable<Node>
    {
        public PlayerState State {get;set;}

        public DamagableBehavior Damagable { get; private set; }

        public MovableBehavior Movable { get; private set; }

        public InteractableBehavior Interactable { get; private set; }

        public UiBehavior Ui { get; private set; }

        public FlashlightBehavior Flashlight { get; private set; }

        public Status PlayerStatus { get; set; }

        [Export]
        public bool IsDebugging { get; set; } = false;

        public bool IsDebugPrintEnabled() => IsDebugging;



        public override void _Ready()
        {
            PlayerStatus = GetNode<Status>("PlayerStatus");

            State = new PlayerState {
                PlayerStatus = PlayerStatus,
                Inventory = new Inventory(),
                MissionManager = new MissionManager()
            };

            
            Movable = GetNode<MovableBehavior>("Behaviors/Movable");
            Movable.Init(this);

            Damagable = GetNode<DamagableBehavior>("Behaviors/Damagable");
            Damagable.OnTakeDamage += (obj, force) => Movable.MoveAndSlide(force);

            Interactable = GetNode<InteractableBehavior>("Behaviors/Interactable");
            Interactable.Init(State);

            Flashlight = GetNode<FlashlightBehavior>("Behaviors/Flashlight");
            Flashlight.Init(State);

            Ui = GetNode<UiBehavior>("UI");
            Ui.Init(State);
        }
    }
}