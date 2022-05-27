using Godot;
using ThemedHorrorJam5.Scripts.Patterns.Logger;

namespace ThemedHorrorJam5.Entities.Components
{
    public class HitBox : Area2D, IDebuggable<Node>
    {
        [Export]
        public bool IsDebugging { get; set; } = false;

        public bool IsDebugPrintEnabled() => IsDebugging;

        [Export]
        public int Damage { get; set; } = 1;

        [Export]
        public float EffectForce { get; set; } = 50f;

        public override void _Ready()
        {
            base._Ready();
            //this.ConnectBodyEntered(this, nameof(OnHitBoxEnter));
        }

        // public void OnHitBoxEnter(Node body)
        // {
        //     this.PrintCaller();
        //     if(body.IsPlayer()){
        //         var player =  (Player)body;
        //         player.On
        //     }
        // }

    }
}