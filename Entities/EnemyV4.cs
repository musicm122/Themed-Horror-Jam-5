using Godot;
using System;
using ThemedHorrorJam5.Entities.Components;
using ThemedHorrorJam5.Scripts.Enum;
using ThemedHorrorJam5.Scripts.Patterns.Logger;
using ThemedHorrorJam5.Scripts.Patterns.StateMachine;

namespace ThemedHorrorJam5.Entities
{

    public class EnemyV4 : KinematicBody2D, IDebuggable<Node>
    {
        [Export]
        public bool IsDebugging { get; set; }

        [Export]
        public NodePath PatrolPath { get; set; }

        [Export]
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
            Status = GetNode<EnemyStatus>("Status");
            Status.VisionRadius = GetNode<Area2D>("VisionRadius");
            Status.Cooldown = GetNode<Label>("Cooldown");
            Status.DebugLabel = this.GetNode<Label>("DebugLabel");
            Damagable = GetNode<DamagableBehavior>("Behaviors/Damagable");
            Movable = GetNode<EnemyMovableBehavior>("Behaviors/Movable");
            Cooldown = GetNode<Label>("Cooldown");
            

            if (this.PatrolPath != null)
            {
                Status.Init(this.PatrolPath);
            }else{
                Status.DebugLabel.Text = "this.PatrolPath is null";
            }
            Damagable.Init(Status);
            Movable.Init(this);
            Init();
        }

        public override void _PhysicsProcess(float delta)
        {
            stateMachine.Update(delta);
        }
    }

}