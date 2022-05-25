using System.Linq;
using Godot;
using System;
using System.Collections.Generic;
using ThemedHorrorJam5.Entities.Components;
using ThemedHorrorJam5.Scripts.Enum;
using ThemedHorrorJam5.Scripts.Extensions;
using ThemedHorrorJam5.Scripts.ItemComponents;

namespace ThemedHorrorJam5.Entities
{
    public class EnemyV4: KinematicBody2D, IDebuggable<Node>
    {
        [Export]
        public bool IsDebugging { get; set; } = false;
        public bool IsDebugPrintEnabled() => IsDebugging;
        
        public Status EnemyStatus { get; set; }

        public DamagableBehavior Damagable { get; private set; }
        
        public EnemyMovableBehavior Movable {get;set;}

        public Status EnemyStatus { get; set; }

    }
}