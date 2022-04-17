using Godot;
using ThemedHorrorJam5.Scripts.Enum;
using ThemedHorrorJam5.Scripts.ItemComponents;

namespace ThemedHorrorJam5.Entities
{
    public class LockedDoor : Examinable
    {
        private const string OpeningDoorMessage = "Opening Door";
        private const string ClosingDoorMessage = "Closing Door";
        private const string DoorIsClosedMessage = "Door is now closed";
        private const string DoorIsNowOpenedMessage = "Door is now opened";

        //private const string OnInteractingWithDoorMessage = "Interacting with door...";

        [Signal]
        public delegate void DoorInteraction(LockedDoor lockedDoor);

        [Export]
        public Key RequiredKey { get; set; }

        [Export]
        public DoorState CurrentDoorState { get; set; } = DoorState.Locked;

        public AnimatedSprite Sprite { get; set; }

        public CollisionShape2D Collider { get; set; }

        public void OpenDoor()
        {
            this.Print(OpeningDoorMessage);
            StartDialog(LockedDoorTimelines.OpeningDoorTimeline);
            Sprite.Play(LockedDoorAnimations.OpenAnimation);
            CurrentDoorState = DoorState.Opened;
            //Collider.Disabled = true;
            this.Print(DoorIsNowOpenedMessage);
        }

        public void CloseDoor()
        {
            this.Print(ClosingDoorMessage);
            StartDialog(LockedDoorTimelines.ClosingDoorTimeline);
            Sprite.Play(LockedDoorAnimations.ClosedAnimation);
            CurrentDoorState = DoorState.Closed;
            //Collider.Disabled = false;
            this.Print(DoorIsClosedMessage);
        }

        public void StartLockedDoorNotification()
        {
            this.Print($"Door is locked and requires {RequiredKey}");
            StartDialog(LockedDoorTimelines.LockedDoorTimeline);
        }

        public void SetColliderState(DoorState state)
        {
            switch (state)
            {
                case DoorState.Locked:
                case DoorState.Closed:
                    Collider.Disabled = false;
                    break;
                case DoorState.Opened:
                    Collider.Disabled = true;
                    break;
            }
        }

        public override void _Ready()
        {
            base._Ready();
            Sprite = GetNode<AnimatedSprite>("AnimatedSprite");
            Sprite.Connect("animation_finished", this, nameof(OnAnimationFinished));
            Collider = GetNode<CollisionShape2D>("Collider/ColliderShape2d");
            if (CurrentDoorState == DoorState.Closed || CurrentDoorState == DoorState.Locked)
            {
                Collider.Disabled = false;
            }
        }

        private void OnAnimationFinished()
        {
            if (Sprite.Animation == LockedDoorAnimations.OpenAnimation)
            {
                Sprite.Stop();
                Sprite.Frame = 3;
            }
            if (Sprite.Animation == LockedDoorAnimations.ClosedAnimation)
            {
                Sprite.Stop();
                Sprite.Frame = 3;
            }
        }

        protected override void OnInteract()
        {
            this.Print($"LockedDoor.OnOnInteract called with the current door state set to {CurrentDoorState}");
            EmitSignal(nameof(DoorInteraction), this);
            switch (CurrentDoorState)
            {
                case DoorState.Locked:
                    StartLockedDoorNotification();
                    break;

                case DoorState.Closed:
                    OpenDoor();
                    break;

                case DoorState.Opened:
                    CloseDoor();
                    break;
            }
            SetColliderState(CurrentDoorState);
        }
    }
}