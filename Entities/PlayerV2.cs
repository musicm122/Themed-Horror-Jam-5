using Godot;
using ThemedHorrorJam5.Entities.Components;
using ThemedHorrorJam5.Scripts.ItemComponents;
using ThemedHorrorJam5.Scripts.Patterns.Logger;

namespace ThemedHorrorJam5.Entities
{
    public class PlayerV2 : PlayerMovableBehavior
    {
        public PlayerState State { get; set; }

        public IDamagableBehavior Damagable { get; private set; }

        public IInteractableBehavior Interactable { get; private set; }

        public IUiBehavior Ui { get; private set; }

        public IFlashlightBehavior Flashlight { get; private set; }

        public Health PlayerStatus { get; set; }

        public override void _Ready()
        {

            PlayerStatus = GetNode<Health>("Health");

            State = new PlayerState
            {
                PlayerStatus = PlayerStatus,
                Inventory = new Inventory(),
                MissionManager = new MissionManager()
            };

            Damagable = GetNode<DamagableBehavior>("Behaviors/Damagable");
            Damagable.Init(PlayerStatus);

            Interactable = GetNode<InteractableBehavior>("Behaviors/Interactable");
            Interactable.Init(State);


            Flashlight = GetNode<FlashlightBehavior>("Behaviors/Flashlight");
            Flashlight.Init(State);

            Ui = GetNode<UiBehavior>("UI");
            Ui.Init(State);


            Interactable.InteractingCallback += (e) => CanMove = false;
            Interactable.InteractingCompleteCallback += (e) => CanMove = true;

            Damagable.OnTakeDamage += OnTakeDamage;
        }

        private void OnTakeDamage(Node sender, Vector2 damageForce) 
        {
            MoveAndSlide(damageForce);
            Ui.RefreshUI();
        }

        public void AddItem(string name, int amt)
        {
            Ui.AddItem(name, amt);
        }

        public void RemoveItems(string name, int amt)
        {
            Ui.RemoveItems(name, amt);
        }

        public void AddMission(string title)
        {
            Ui.AddMission(title);
        }
    }
}