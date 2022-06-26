using Godot;
using System;
using System.Globalization;
using ThemedHorrorJam5.Entities.Behaviors;
using ThemedHorrorJam5.Entities.Components;
using ThemedHorrorJam5.Entities.EnemyState;
using ThemedHorrorJam5.Entities.Vision;
using ThemedHorrorJam5.Scripts.Enum;
using ThemedHorrorJam5.Scripts.Patterns.StateMachine;

namespace ThemedHorrorJam5.Entities
{
    public class EnemyV4 : EnemyMovableBehavior, IEnemy
    {
        [Export]
        public NodePath PatrolPath { get; set; }

        [Export] public EnemyBehaviorStates DefaultState { get; set; } = EnemyBehaviorStates.Wander;

        public EnemyStatus Status { get; set; }

        public IDamagableBehavior Damagable { get; private set; }

        public Node2D ObstacleAvoidance { get; set; }

        public Label Cooldown { get; set; }

        private Label StateLabel { get; set; }

        private readonly StateMachine _stateMachine = new();

        public void Init()
        {
            _stateMachine.AddState(new IdleEnemyState(this));
            _stateMachine.AddState(new PatrolEnemyState(this));
            _stateMachine.AddState(new ChaseEnemyState(this));
            _stateMachine.AddState(new WanderState(this));

            if (!Status.LineOfSight)
            {
                _stateMachine.TransitionTo(EnemyBehaviorStates.Patrol);
            }
            if (Status.CurrentCoolDownCounter <= 0f)
            {
                _stateMachine.TransitionTo(EnemyBehaviorStates.Patrol);
                Cooldown.Text = String.Empty;
            }
            else
            {
                Status.Cooldown.Text = 
                    $"Cooling Down in {Status.CurrentCoolDownCounter.ToString(CultureInfo.InvariantCulture)} seconds";
            }
            _stateMachine.TransitionTo(DefaultState);
        }

        public override void _Ready()
        {
            StateLabel = GetNode<Label>("StateLabel");
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

            if (this.PatrolPath != null)
            {
                Status.Init(this.PatrolPath);
            }
            else
            {
                Status.DebugLabel.Text = "this.PatrolPath is null";
            }
            Damagable.Init(Status);
            Init();
        }

        private void OnTargetLost(Node2D target)
        {
            Status.CurrentCoolDownCounter = Status.MaxCoolDownTime;
        }

        private void OnTargetDetection(Node2D target)
        {
            Status.CurrentCoolDownCounter = Status.MaxCoolDownTime;
            this._stateMachine.TransitionTo(EnemyBehaviorStates.ChasePlayer);
        }

        public override void _PhysicsProcess(float delta)
        {
            _stateMachine.Update(delta);
            this.StateLabel.Text = _stateMachine.CurrentState.ToString();
        }
    }
}