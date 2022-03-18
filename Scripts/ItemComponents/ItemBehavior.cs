using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThemedHorrorJam5.Scripts.ItemComponents
{
    public class ItemBehavior : Area2D
    {
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
            Tween.InterpolateProperty(Sprite, Property, new Vector2(scaleUp, scaleUp), new Vector2(scaleDown, scaleDown), Time, Tween.TransitionType.Linear, Tween.EaseType.InOut);
            Tween.Start();
        }

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            Tween = GetNode<Tween>(new NodePath("Tween"));
            Sprite = GetNode<Sprite>(new NodePath("Sprite"));
            GrowShrink(ScaleDown, ScaleUp, Time);
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
    }
}
