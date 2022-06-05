using Godot;
using System;
using ThemedHorrorJam5.Entities.Behaviors;
using ThemedHorrorJam5.Entities.Components;
using ThemedHorrorJam5.Scripts.Enum;
using ThemedHorrorJam5.Scripts.Patterns.Logger;
using ThemedHorrorJam5.Scripts.Patterns.StateMachine;

namespace ThemedHorrorJam5.Entities
{
    public class EnemyV4 : EnemyMovableBehavior
    {
        [Export]
        public NodePath PatrolPath { get; set; }

        [Export]
        public EnemyBehaviorStates DefaultState = EnemyBehaviorStates.Wander;

        public EnemyStatus Status { get; set; }

        public DamagableBehavior Damagable { get; private set; }

        //public EnemyMovableBehavior Movable { get; set; }

        public Node2D ObstacleAvoidance { get; set; }

        public Label Cooldown { get; set; }

        private readonly StateMachine stateMachine = new();


        public void Init()
        {
            stateMachine.AddState(new IdleEnemyState(this));
            stateMachine.AddState(new PatrolEnemyState(this));
            stateMachine.AddState(new ChaseEnemyState(this));
            stateMachine.AddState(new WanderState(this));

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
            Status.VisionManager = GetNode<Area2dVision>("Vision");
            ObstacleAvoidance = GetNode<Node2D>("ObstacleAvoidance");

            if (Status.VisionManager != null)
            {
                Status.VisionManager.OnTargetSeen += OnTargetDetection;
                Status.VisionManager.OnTargetOutOfSight += OnTargetLost;
            }
            Status.Cooldown = GetNode<Label>("Cooldown");
            Cooldown = GetNode<Label>("Cooldown");
            Status.DebugLabel = this.GetNode<Label>("DebugLabel");
            Damagable = GetNode<DamagableBehavior>("Behaviors/Damagable");
            //Movable = GetNode<EnemyMovableBehavior>("Behaviors/Movable");


            if (this.PatrolPath != null)
            {
                Status.Init(this.PatrolPath);
            }
            else
            {
                Status.DebugLabel.Text = "this.PatrolPath is null";
            }
            Damagable.Init(Status);
            //Movable.Init(this);
            Init();
        }

        private void OnTargetLost(Node2D target)
        {
            Status.Target = null;
            Status.CurrentCoolDownCounter = Status.MaxCoolDownTime;
        }

        private void OnTargetDetection(Node2D target)
        {
            Status.Target = target;
            Status.CurrentCoolDownCounter = Status.MaxCoolDownTime;
            this.stateMachine.TransitionTo(EnemyBehaviorStates.ChasePlayer);
        }

        public override void _PhysicsProcess(float delta)
        {
            stateMachine.Update(delta);
        }

        public override void _Draw()
        {
            // DrawRect(this.EnclosureZone, new Godot.Color(255f, 255f, 0f));
            //if (IsDebugging)
            //{
            //    DrawRect(this.EnclosureZone, new Godot.Color(100));
            //}
        }
    }
}