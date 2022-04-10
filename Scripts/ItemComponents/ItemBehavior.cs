using Godot;
using ThemedHorrorJam5.Scripts.Extensions;

namespace ThemedHorrorJam5.Scripts.ItemComponents
{
    public class ItemBehavior : Area2D, IDebuggable<Node>
    {
        [Export]
        public bool IsDebugging { get; set; }

        public bool IsDebugPrintEnabled() => IsDebugging;

        public string Direction = "down";

        public float Time = 0.5f;

        public string Property = "scale";

        public Sprite Sprite { get; set; }

        public Tween Tween { get; set; }

        [Export]
        public float ScaleUp = 2f;

        [Export]
        public float ScaleDown = 0.5f;

        public void GrowShrink(float scaleUp, float scaleDown, float time)
        {
            Tween.InterpolateProperty(Sprite, Property, new Vector2(scaleUp, scaleUp), new Vector2(scaleDown, scaleDown), time, Tween.TransitionType.Linear, Tween.EaseType.InOut);
            Tween.Start();
        }

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            if (HasNode("Sprite"))
            {
                Sprite = GetNode<Sprite>("Sprite");
            }
            if (HasNode("Tween"))
            {
                Tween = GetNode<Tween>("Tween");
                GrowShrink(ScaleDown, ScaleUp, Time);
            }
        }

        public void OnTweenCompleted(Godot.Object obj, NodePath key)
        {
            if (Direction == "down")
            {
                GrowShrink(ScaleDown, ScaleUp, Time);
                this.Direction = "up";
            }
            else
            {
                GrowShrink(ScaleUp, ScaleDown, Time);
                this.Direction = "down";
            }
        }
        public virtual void OnExaminableAreaEntered(Node body)
        {
        }

        public virtual void OnExaminableAreaExited(Node body)
        {
        }
    }
}