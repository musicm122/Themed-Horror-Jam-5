using System;
using Godot;
using ThemedHorrorJam5.Scripts.Patterns.Logger;

namespace ThemedHorrorJam5.Entities.Components
{

    [Tool]
    public class VisionManager : Node2D, IDebuggable<Node2D>
    {
        [Export]
        public float MovementSpeed { get; set; } = 3f;

        Label DebugLabel { get; set; }
        DynamicFont Font { get; set; }

        [Export]
        public bool IsDebugging { get; set; } = false;

        public bool IsDebugPrintEnabled() => IsDebugging;

        public Array ExcludeFromCollisions;

        public uint CollisionMask = 1;

        [Export]
        public Vector2 FacingDirection { get; set; } = Vector2.Right;

        [Export]
        public float ConeArc = 60f;

        public bool LineOfSight = false;
        public bool CheckThisFrame = false;

        public override void _Ready()
        {
            CheckThisFrame = CanCheckFrame();
            Font = new DynamicFont
            {
                FontData = (DynamicFontData)ResourceLoader.Load("res://Assets/Art/UI/Font/Fonts/Overlock/Overlock-Regular.ttf"),
                Size = 10
            };
            DebugLabel = GetNode<Label>("DebugLabel");
        }

        private bool CanCheckFrame(int interval = 2)
        {
            Random random = new Random();
            return random.Next() % interval == 0;
        }

        public bool IsInVisionCone(Vector2 point)
        {
            var ourPos = GlobalTransform.origin;
            var directionToPoint = point - ourPos;
            return directionToPoint.AngleTo(FacingDirection) <= Mathf.Deg2Rad(ConeArc / 2f);
        }

        public bool HasLineOfSight(Vector2 point)
        {
            if (!CanCheckFrame()) return LineOfSight;
            var spaceState = GetWorld2d().DirectSpaceState;
            var result = spaceState.IntersectRay(GlobalTransform.origin, point, null, CollisionMask);
            LineOfSight = result?.Count > 0;
            return LineOfSight;
        }

        public override void _PhysicsProcess(float delta)
        {
            //FacingDirection = GetParent(). GlobalTransform.b
            //FacingDirection = new Vector2(Mathf.Cos(GlobalRotation), Mathf.Sin(GlobalRotation)).Normalized();
            FacingDirection = new Vector2(Mathf.Cos(GlobalRotation), Mathf.Sin(GlobalRotation)).Normalized();
            if (IsDebugging)
            {
                DebugLabel.Text =
                    @$"
                    | Global Position: {GlobalPosition} 
                    | Global Rotation: {GlobalRotation} 
                    | Global Rotation Degrees: {GlobalRotationDegrees} 
                    | Global Transform Origin : {GlobalTransform.origin} 
                    | Global Transform Rotation : {GlobalTransform.Rotation} 
                    | Global Transform Basis X: {GlobalTransform.x} 
                    | Global Transform Basis Y: {GlobalTransform.y} 
                    | Position: {Position}                     
                    | Rotation: {Rotation} 
                    | Rotation Degrees: {RotationDegrees} 
                    | Facing Direction: {FacingDirection}";
            }
        }

        void DrawGuideLines() 
        {
            var angleOffset = 20;
            var angle2 = ConeArc + angleOffset;
            var angle3 = ConeArc - angleOffset;

            var endline1 = (FacingDirection * 10f);
            var endline2 = new Vector2(endline1.x, endline1.y);
            endline2.Rotated(Mathf.Deg2Rad(angle2));

            var endline3 = new Vector2(endline1.x, endline1.y);
            endline3.Rotated(Mathf.Deg2Rad(angle3));

            //var endline2 = endline1.RotatedCounterClockwiseByAngle(angle2);
            //var endline3 = endline1.RotatedCounterClockwiseByAngle(angle3);

            this.Print($"endline1: {endline1}");
            this.Print($"endline2: {endline2}");
            this.Print($"endline3: {endline3}");

            DrawString(Font, Vector2.Zero, $"FacingDirection:{FacingDirection} \r\n endline1:{endline1}", new Color(255, 255, 0));

            DrawLine(Position, endline1, new Color(255, 255, 0));
            DrawLine(Position, endline2, new Color(255, 0, 0));
            DrawLine(Position, endline3, new Color(255, 255, 0));
        }

        public override void _Draw()
        {
            DrawGuideLines();
        }
    }
}
