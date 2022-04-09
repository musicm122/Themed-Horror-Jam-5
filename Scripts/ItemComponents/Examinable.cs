using Godot;
using System.Threading.Tasks;
using ThemedHorrorJam5.Scripts.GDUtils;

namespace ThemedHorrorJam5.Scripts.ItemComponents
{
    public class Examinable : Node2D, IDebuggable<Node>
    {
        [Export]
        public bool IsDebugging { get; set; } = false;

        public bool IsDebugPrintEnabled() => IsDebugging;

        private Area2D InteractableArea { get; set; }

        [Signal]
        public delegate void PlayerInteracting(Examinable examinable);

        [Signal]
        public delegate void PlayerInteractingComplete(Examinable examinable);

        [Signal]
        public delegate void PlayerInteractingUnavailable(Examinable examinable);

        [Signal]
        public delegate void PlayerInteractingAvailable(Examinable examinable);

        private const string Flashlight = "Flashlight";

        [Export]
        public string Timeline { get; set; }

        public bool CanInteract { get; set; } = false;
        public bool ShouldRemove { get; set; } = false;

        public void DialogListener(System.Object value)
        {
            this.Pause();
            var val = value.ToString();
            switch (val)
            {
                case Flashlight:
                    if (Name == Flashlight)
                    {
                        GetTree().AddItem(Flashlight.ToLower());
                        ShouldRemove = true;
                    }
                    break;

                case "Keys":
                    if (Name == "key")
                    {
                        GetTree().AddItem("KeyA");
                        ShouldRemove = true;
                    }
                    break;
            }
            Task.Run(async () =>
            {
                await DialogComplete();
            });
        }

        private async Task DialogComplete()
        {
            EmitSignal(nameof(PlayerInteractingComplete), this);
            await this.WaitForSeconds(0.2f);
            this.Unpause();
            CanInteract = true;
            if (ShouldRemove)
            {
                RemoveItem();
            }
        }

        protected virtual void OnInteract()
        {
            this.PrintCaller();
            if (!Timeline.IsNullOrWhiteSpace())
            {
                EmitSignal(nameof(PlayerInteracting), this);
                CanInteract = false;
                var dialog = DialogicSharp.Start(Timeline);
                dialog.Connect("dialogic_signal", this, "DialogListener");
                this.Print<Node>("dialog.IsConnected(\"dialogic_signal\",this, \"DialogListener\") = " + dialog.IsConnected("dialogic_signal", this, "DialogListener"));
                AddChild(dialog);
            }
            else
            {
                GD.Print("No timeline assigned to Examinable");
            }
        }

        private void RemoveItem()
        {
            this.PrintCaller();
            this.Unpause();
            EmitSignal(nameof(PlayerInteractingUnavailable));
            Visible = false;
            SetProcess(false);
            SetPhysicsProcess(false);
            SetProcessInput(false);

            if (GetChildCount() > 0)
            {
                InteractableArea.DisconnectBodyEntered(nameof(OnExaminableAreaEntered));
                InteractableArea.DisconnectBodyExited(nameof(OnExaminableAreaExited));
                RemoveChild(InteractableArea);
            }
        }

        public void OnExaminableAreaEntered(Node body)
        {
            if (body.IsPlayer())
            {
                EmitSignal(nameof(PlayerInteractingAvailable), this);
            }
        }

        public void OnExaminableAreaExited(Node body)
        {
            if (body.IsPlayer())
            {
                EmitSignal(nameof(PlayerInteractingUnavailable), this);
            }
        }

        public override void _Process(float delta)
        {
            if (CanInteract && InputUtils.IsInteracting())
            {
                OnInteract();
            }
        }

        private void RegisterInteractable(Area2D area2D)
        {
            area2D.ConnectBodyEntered("OnExaminableAreaEntered");
            area2D.ConnectBodyExited("OnExaminableAreaExited");
        }

        public override void _Ready()
        {
            InteractableArea = this.GetNode<Area2D>("Area2D");
            RegisterInteractable(InteractableArea);
        }
    }
}