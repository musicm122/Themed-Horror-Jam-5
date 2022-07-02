using Godot;
using ThemedHorrorJam5.Entities.Behaviors;
using ThemedHorrorJam5.Entities.Components;
using ThemedHorrorJam5.Scripts.ItemComponents;
using ThemedHorrorJam5.Scripts.Patterns.Logger;

namespace ThemedHorrorJam5.Entities
{
    public class PlayerV2 : PlayerMovableBehavior
    {
        public PlayerDataStore DataStore { get; set; }

        public IDamagableBehavior Damagable { get; private set; }

        public IInteractableBehavior Interactable { get; private set; }

        public IUiBehavior Ui { get; private set; }

        public IFlashlightBehavior Flashlight { get; private set; }

        public Health PlayerStatus { get; set; }

        public PlayerAnimationManager AnimationManager { get; set; }
        public override void _Ready()
        {
            base._Ready();
            PlayerStatus = GetNode<Health>("Health");
            AnimationManager = GetNode<PlayerAnimationManager>("AnimationManager");

            DataStore = new PlayerDataStore
            {
                PlayerStatus = PlayerStatus,
                Inventory = new Inventory(),
                MissionManager = new MissionManager()
            };

            Damagable = GetNode<DamagableBehavior>("Behaviors/Damagable");
            Damagable.Init(PlayerStatus);

            Interactable = GetNode<InteractableBehavior>("Behaviors/Interactable");
            Interactable.Init(DataStore);


            Flashlight = GetNode<FlashlightBehavior>("Behaviors/Flashlight");
            Flashlight.Init(DataStore);

            Ui = GetNode<UiBehavior>("UI");
            Ui.Init(DataStore);

            Interactable.InteractingCallback += (e) => CanMove = false;
            Interactable.InteractingCompleteCallback += (e) => CanMove = true;

            Damagable.OnTakeDamage += OnTakeDamage;
            Damagable.HurtboxInvincibilityStartedCallback += OnHurtboxInvincibilityStarted;
            Damagable.HurtboxInvincibilityEndedCallback += OnHurtboxInvincibilityEnded;
            this.OnPhysicsProcessMovement += OnProcessMovement;
            this.OnMove += OnWalkAction;
            this.OnIdle += OnIdleAction;
            this.OnRoll += OnRollAction;
        }

        private void OnHurtboxInvincibilityEnded()
        {
            AnimationManager?.StopBlinkAnimation();
            
        }

        private void OnHurtboxInvincibilityStarted()
        {
            AnimationManager?.StartBlinkAnimation();
        }

        private void OnProcessMovement(Vector2 vector2)
        {
            AnimationManager?.UpdateAnimationBlendPositions(vector2);            
        }
        
        private void OnRollAction(Vector2 velocity)
        {
            AnimationManager?.PlayRollAnimation(velocity);
        }

        private void OnIdleAction(Vector2 velocity, float delta)
        {
            AnimationManager?.PlayIdleAnimation(velocity);
        }

        private void OnWalkAction(Vector2 velocity, float delta)
        {
            AnimationManager?.PlayWalkAnimation(velocity);
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