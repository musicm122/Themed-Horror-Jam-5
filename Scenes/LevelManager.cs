using Godot;
using System.Collections.Generic;
using ThemedHorrorJam5.Entities;

namespace ThemedHorrorJam5.Scenes
{
    public class LevelManager : Node2D 
    {
        public List<Node2D> Enemies { get; set; }
        
        
        public void AlertEnemies() 
        {
        }

        public override void _Ready()
        {
            base._Ready();
        }
    }
}