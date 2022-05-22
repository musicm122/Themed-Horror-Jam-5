using System.Globalization;
using Godot;
using ThemedHorrorJam5.Entities.Components;
using ThemedHorrorJam5.Scripts.Constants;
using ThemedHorrorJam5.Scripts.ItemComponents;

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
            var maxTime = 2f;
            this.CameraSprite.Color = CommonColors.IdleColor;
            if (Elapsed > maxTime && !IsPausing)
            {
                IsStartMovement = !IsStartMovement;
                IsPausing = true;
                Elapsed = 0f;
            }

            if (Elapsed > maxTime && IsPausing)
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

            //var minRad = Mathf.Deg2Rad(90f);
            //var maxRad = Mathf.Deg2Rad(120f);
            //this.Rotation = minRad;
            //var startDegree = Mathf.Deg2Rad(StartRotationAngle);
            //var endDegree = Mathf.Deg2Rad(EndRotationAngle);
            //this.Rotation += RotationSpeed * delta;


            //this.Rotation = this.Rotation > maxDegree ? minDegree : this.Rotation;
            //this.Rotation = Mathf.LerpAngle(minDegree, maxDegree, Elapsed);
            //this.Rotation = Mathf.Clamp(Mathf.LerpAngle(minDegree, maxDegree, elapsed), startDegree, endDegree) * delta;
            //if (this.Rotation < StartRotationAngle)
            //{
            //    this.Rotation = Mathf.Clamp(Mathf.LerpAngle(startDegree, endDegree, elapsed), startDegree, endDegree) * delta;
            //    //this.Rotation += Mathf.LerpAngle(startDegree, endDegree, 0.2f);
            //    this.CameraSprite.Color = WarningColor;
            //}
            //else
            //{
            //    this.Rotation = Mathf.Clamp(Mathf.LerpAngle(endDegree, startDegree, elapsed), endDegree, startDegree) * delta;
            //    //this.Rotation += Mathf.LerpAngle(endDegree, startDegree, 0.2f);
            //    this.CameraSprite.Color = IdleColor;
            //}
            Elapsed += delta;

            // this.Rotation += RotationSpeed * delta;
            //var minAngle = 
            //var maxAngle = Mathf.Deg2Rad(RotationArc);
            //while(this.Transform.Rotation)

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
