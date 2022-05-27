using System.Globalization;
using Godot;
using ThemedHorrorJam5.Entities.Components;
using ThemedHorrorJam5.Scripts.Constants;
using ThemedHorrorJam5.Scripts.Patterns.Logger;

namespace ThemedHorrorJam5.Entities
{
    public enum CameraState { Idle, Warning, Aggro, Damaged, Stun }

    public class SecurityCamera : Node2D, IDebuggable<Node2D>
    {
        private bool IsStartMovement = true;

        private bool IsPausing = false;

        private float Elapsed = 0f;

        private Label DebugLabel { get; set; }

        public CameraState CurrentState = CameraState.Idle;

        public RaycastVision VisionManager { get; set; }

        public Node2D PlayerRef { get; set; }

        [Export]
        public float MaxRotationMovementTime = 2f;

        [Export]
        public float PauseRotationTime = 2f;

        [Export]
        public float RotationSpeed { get; set; } = 80f;

        public Node2D Target { get; set; }

        public Polygon2D CameraSprite { get; set; }

        [Export]
        public bool IsDebugging { get; set; } = false;

        public override void _Ready()
        {
            DebugLabel = GetNode<Label>("DebugLabel");
            VisionManager = GetNode<RaycastVision>("Pivot/RayCast2D");
            CameraSprite = GetNode<Polygon2D>("Polygon2D");
            if (VisionManager != null)
            {
                VisionManager.OnTargetSeen += OnTargetDetection;
                VisionManager.OnTargetOutOfSight += OnTargetLost;
            }
        }

        private void OnTargetLost(Node2D player)
        {
            this.Print($"Player lost. Player last known position at {player.GlobalPosition}");
            CameraSprite.Color = CommonColors.AggroColor;
            this.Target = null;
        }

        public void OnTargetDetection(Node2D player)
        {
            PlayerRef = player;
            CurrentState = CameraState.Aggro;
        }

        private void OnIdle(float delta)
        {
            this.CameraSprite.Color = CommonColors.IdleColor;
            if (Elapsed > MaxRotationMovementTime && !IsPausing)
            {
                IsStartMovement = !IsStartMovement;
                IsPausing = true;
                Elapsed = 0f;
            }

            if (Elapsed > MaxRotationMovementTime && IsPausing)
            {
                IsPausing = false;
                Elapsed = 0f;
            }

            if (!IsPausing)
            {
                if (IsStartMovement)
                {
                    this.Rotation += RotationSpeed * delta;
                }
                else
                {
                    this.Rotation -= RotationSpeed * delta;
                }
            }
            Elapsed += delta;
        }

        private void OnWarning(float delta)
        {
            this.CameraSprite.Color = CommonColors.WarningColor;
        }

        private void OnAggro(float delta)
        {
            if (PlayerRef != null)
            {
                var targetPoint = PlayerRef.GlobalPosition;
                //var targetPoint = PlayerRef.GetAimAtPoint();
                CameraSprite.Color = CommonColors.AggroColor;
                this.LookAt(targetPoint);
                this.PrintCaller();

            }
        }

        private void OnDamaged(float delta)
        {
            this.PrintCaller();
        }

        private void OnStun(float delta)
        {
            this.PrintCaller();
        }

        public bool IsDebugPrintEnabled() => IsDebugging;

        public override void _PhysicsProcess(float delta)
        {
            switch (CurrentState)
            {
                case CameraState.Warning:
                    OnWarning(delta);
                    break;
                case CameraState.Aggro:
                    OnAggro(delta);
                    break;
                case CameraState.Damaged:
                    OnDamaged(delta);
                    break;
                case CameraState.Stun:
                    OnStun(delta);
                    break;
                default:
                    OnIdle(delta);
                    break;
            }
            if (IsDebugging)
            {
                DebugLabel.Text =
@$"
------Angle--------------
Rotation : {Mathf.Rad2Deg(this.Rotation)}
Global Rotation: {Mathf.Rad2Deg(this.GlobalRotation)}
GlobalTransform.Rotation: {Mathf.Rad2Deg(this.GlobalTransform.Rotation)}

------Degree-------------
Rotation: {this.Rotation}
Global Rotation: {this.GlobalRotation}
GlobalTransform.Rotation: {this.GlobalTransform.Rotation}";

            }
        }
    }
}
