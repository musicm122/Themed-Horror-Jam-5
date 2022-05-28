using Godot;
using ThemedHorrorJam5.Entities.Components;
using ThemedHorrorJam5.Scripts.ItemComponents;
using ThemedHorrorJam5.Scripts.Patterns.Logger;

namespace ThemedHorrorJam5.Entities
{
    public class PlayerV2 : KinematicBody2D, IDebuggable<Node>
    {
        public PlayerState State {get;set;}

        public DamagableBehavior Damagable { get; private set; }

        public PlayerMovableBehavior Movable { get; private set; }

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

            Movable = GetNode<PlayerMovableBehavior>("Behaviors/Movable");
            Movable.Init(this);

            Damagable = GetNode<DamagableBehavior>("Behaviors/Damagable");
            Damagable.Init(PlayerStatus);

            Interactable = GetNode<InteractableBehavior>("Behaviors/Interactable");
            Interactable.Init(State);


            Flashlight = GetNode<FlashlightBehavior>("Behaviors/Flashlight");
            Flashlight.Init(State);

            Ui = GetNode<UiBehavior>("UI");
            Ui.Init(State);


            Interactable.InteractingCallback += (e) => Movable.CanMove = false;
            Interactable.InteractingCompleteCallback += (e) => Movable.CanMove = true;

            Damagable.OnTakeDamage += (obj, force) => {
                Movable.MoveAndSlide(force);
                Ui.RefreshUI();
            };
        }

        public void AddItem(string name, int amt){
            Ui.AddItem(name, amt);
        }

        public void RemoveItems(string name, int amt) {
            Ui.RemoveItems(name, amt);
        }

        public void AddMission(string title)
        {
            Ui.AddMission(title);
        }
    }
}