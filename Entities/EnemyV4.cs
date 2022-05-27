using System.Linq;
using Godot;
using System;
using System.Collections.Generic;
using ThemedHorrorJam5.Entities.Components;
using ThemedHorrorJam5.Scripts.Enum;
using ThemedHorrorJam5.Scripts.Extensions;
using ThemedHorrorJam5.Scripts.Patterns.Logger;
using ThemedHorrorJam5.Scripts.Patterns.StateMachine;

namespace ThemedHorrorJam5.Entities
{

    public class EnemyV4 : KinematicBody2D, IDebuggable<Node>
    {
        [Export]
        public bool IsDebugging { get; set; }

        public EnemyBehaviorStates DefaultState = EnemyBehaviorStates.Idle;

        public bool IsDebugPrintEnabled() => IsDebugging;

        public EnemyStatus Status { get; set; }

        public DamagableBehavior Damagable { get; private set; }

        public EnemyMovableBehavior Movable { get; set; }

        private readonly StateMachine stateMachine = new StateMachine();

        public Label Cooldown { get; set; }



        public void Init()
        {
            stateMachine.AddState(new IdleEnemyState(this));
            stateMachine.AddState(new PatrolEnemyState(this));
            stateMachine.AddState(new ChaseEnemyState(this));

            if (!Status.LineOfSight)
            {
                stateMachine.TransitionTo(EnemyBehaviorStates.Patrol);
            }
            if (Status.CurrentCoolDownCounter <= 0f)
            {
                stateMachine.TransitionTo(EnemyBehaviorStates.Patrol);
                Status.Target = null;
                Cooldown.Text = String.Empty;
            }
            else
            {
                Status.Cooldown.Text = $"Cooling Down in {Status.CurrentCoolDownCounter} seconds";
            }
            stateMachine.TransitionTo(DefaultState);
        }

        public override void _Ready()
        {
            base._Ready();
            Status.VisionRadius = GetNode<Area2D>("VisionRadius");
            Status.Cooldown = GetNode<Label>("Cooldown");
        }
    }

}