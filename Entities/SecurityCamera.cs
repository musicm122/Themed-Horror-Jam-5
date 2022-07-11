using System.Globalization;
using Godot;
using ThemedHorrorJam5.Entities.Behaviors.Interfaces;
using ThemedHorrorJam5.Entities.Vision;
using ThemedHorrorJam5.Scripts.Constants;
using ThemedHorrorJam5.Scripts.Extensions;
using ThemedHorrorJam5.Scripts.Patterns.Logger;

namespace ThemedHorrorJam5.Entities
{
    public class SecurityCamera : Node2D, IDebuggable<Node2D>
    {
        public float CurrentCoolDownCounter { get; set; }

        public float MaxCoolDownTime { get; set; } = 10f;

        private bool IsStartMovement { get; set; } = true;

        private bool IsPausing { get; set; }

        private float Elapsed { get; set; }

        private Label DebugLabel { get; set; }

        public CameraState CurrentState { get; set; } = CameraState.Idle;

        public IVision VisionManager { get; set; }

        public Node2D PlayerRef { get; set; }

        [Export] public float MaxRotationMovementTime { get; set; } = 2f;

        [Export] public float PauseRotationTime { get; set; } = 2f;

        [Export] public float RotationSpeed { get; set; } = 80f;

        public Node2D Target { get; set; }

        public Polygon2D CameraSprite { get; set; }

        [Export] public bool IsDebugging { get; set; }

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
            this.Print($"Player lost. Player last known position at {player.GlobalPosition.ToString()}");
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
            this.PrintCaller();
            if (PlayerRef == null) return;
            var targetPoint = PlayerRef.GlobalPosition;
            CameraSprite.Color = CommonColors.AggroColor;
            this.LookAt(targetPoint);
            GetTree().AlertAllEnemies();
            CurrentCoolDownCounter = MaxCoolDownTime;
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

        public void Alert()
        {
            (_, PlayerRef) = GetTree().GetPlayerNode();
            CurrentState = CameraState.Aggro;
        }

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

            if (CurrentCoolDownCounter > 0)
            {
                CurrentCoolDownCounter -= delta;
            }
            else
            {
                CurrentState = CameraState.Idle;
            }

            if (IsDebugging)
            {
                DebugLabel.Text =
                    @$"
------Angle--------------
Rotation : {Mathf.Rad2Deg(this.Rotation).ToString(CultureInfo.InvariantCulture)}
Global Rotation: {Mathf.Rad2Deg(this.GlobalRotation).ToString(CultureInfo.InvariantCulture)}
GlobalTransform.Rotation: {Mathf.Rad2Deg(this.GlobalTransform.Rotation).ToString(CultureInfo.InvariantCulture)}

------Degree-------------
Rotation: {this.Rotation.ToString(CultureInfo.InvariantCulture)}
Global Rotation: {this.GlobalRotation.ToString(CultureInfo.InvariantCulture)}
GlobalTransform.Rotation: {this.GlobalTransform.Rotation.ToString(CultureInfo.InvariantCulture)}";
            }
        }
    }
}