using Godot;
using System.Threading.Tasks;
using ThemedHorrorJam5.Scripts.Extensions;
using ThemedHorrorJam5.Scripts.GDUtils;

namespace ThemedHorrorJam5.Scripts.ItemComponents
{
    public class Examinable : Node2D, IDebuggable<Node>
    {
        [Export]
        public bool IsDebugging { get; set; } = false;

        [Export]
        public string Timeline { get; set; }

        [Signal]
        public delegate void PlayerInteracting(Examinable examinable);

        [Signal]
        public delegate void PlayerInteractingComplete(Examinable examinable);

        [Signal]
        public delegate void PlayerInteractingUnavailable(Examinable examinable);

        [Signal]
        public delegate void PlayerInteractingAvailable(Examinable examinable);

        public bool IsDebugPrintEnabled() => IsDebugging;

        protected Area2D InteractableArea { get; set; }

        private const string Flashlight = "Flashlight";

        public bool CanInteract { get; set; }

        public bool ShouldRemove { get; set; }

        public void DialogListener(System.Object value)
        {
            this.Print($"DialogListener called with arg {value}");
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
                    this.Print("DialogListener: KEY switch");
                    if (Name.Equals("Key"))
                    {
                        this.Print("DialogListener: KEY switch");
                        GetTree().AddItem("KeyA");
                        ShouldRemove = true;
                    }
                    break;
            }
            Task.Run(async () => await DialogComplete().ConfigureAwait(false));
        }

        private async Task DialogComplete()
        {
            this.Print($"Examinable.{nameof(DialogComplete)} called");
            this.Print($"Examinable.ShouldRemove = {ShouldRemove}");
            EmitSignal(nameof(PlayerInteractingComplete), this);
            await this.WaitForSeconds(0.2f).ConfigureAwait(false);
            this.Unpause();
            CanInteract = true;

            if (ShouldRemove)
            {
                RemoveItem();
            }
        }

        public virtual void StartDialog(string timeLine)
        {
            this.Print($"{nameof(Examinable)} StartDialog(${timeLine}) called");
            EmitSignal(nameof(PlayerInteracting), this);
            CanInteract = false;
            var dialog = DialogicSharp.Start(timeLine);
            var result = dialog.Connect("dialogic_signal", this, "DialogListener");
            if (result == Error.Ok)
            {
                AddChild(dialog);
            }
            else
            {
                this.PrintError(result, $"{nameof(Examinable)} StartDialog(${timeLine}) failed");
            }

            //this.Print("dialog.IsConnected(\"dialogic_signal\",this, \"DialogListener\") = " + dialog.IsConnected("dialogic_signal", this, "DialogListener"));

        }

        protected virtual void OnInteract()
        {
            this.PrintCaller();
            if (!Timeline.IsNullOrWhiteSpace())
            {
                StartDialog(Timeline);
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
            if (this.HasSignal(nameof(PlayerInteractingUnavailable)))
            {
                EmitSignal(nameof(PlayerInteractingAvailable), this);
            }
            Visible = false;
            SetProcess(false);
            SetPhysicsProcess(false);
            SetProcessInput(false);

            if (GetChildCount() > 0 && InteractableArea != null)
            {
                //InteractableArea.DisconnectBodyEntered(nameof(OnExaminableAreaEntered));
                //InteractableArea.DisconnectBodyExited(nameof(OnExaminableAreaExited));
                RemoveChild(InteractableArea);
            }
        }

        public virtual void OnExaminableAreaEntered(Node body)
        {
            this.PrintCaller();
            if (body.IsPlayer() && this.HasSignal(nameof(PlayerInteractingAvailable)))
            {
                EmitSignal(nameof(PlayerInteractingAvailable), this);
            }
        }

        public virtual void OnExaminableAreaExited(Node body)
        {
            this.PrintCaller();
            if (body.IsPlayer())
            {
                if (this.HasSignal(nameof(PlayerInteractingUnavailable)))
                {
                    EmitSignal(nameof(PlayerInteractingUnavailable), this);
                }
            }
        }

        public virtual void ProcessLoop(float delta)
        {
            if (CanInteract && InputUtils.IsInteracting())
            {
                OnInteract();
            }
        }

        public override void _Process(float delta)
        {
            ProcessLoop(delta);
        }

        private void RegisterInteractable(Area2D area2D)
        {
            area2D.ConnectBodyEntered(this, nameof(OnExaminableAreaEntered));
            area2D.ConnectBodyExited(this, nameof(OnExaminableAreaExited));
        }

        public override void _Ready()
        {
            InteractableArea = this.GetNode<Area2D>("Area2D");
            RegisterInteractable(InteractableArea);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            InteractableArea.DisconnectBodyEntered(nameof(OnExaminableAreaEntered));
            InteractableArea.DisconnectBodyExited(nameof(OnExaminableAreaExited));
        }
    }
}