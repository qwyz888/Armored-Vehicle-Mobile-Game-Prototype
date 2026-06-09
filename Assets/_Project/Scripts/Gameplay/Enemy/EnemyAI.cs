using System;
using UnityEngine;

namespace Gameplay.Enemy
{
    [RequireComponent(typeof(Gameplay.Health.HealthComponent))]
    public class EnemyAI : MonoBehaviour, IEnemy
    {
        [Header("Detection")]
        [SerializeField] private float detectRadius = 10f;
        [SerializeField] private LayerMask detectMask;
        [Header("Movement")]
        [SerializeField] private float walkSpeed = 1f;
        [SerializeField] private float runSpeed = 4f;
        [SerializeField] private float explodeDistance = 1.2f;
        [Header("Explosion")]
        [SerializeField] private GameObject explosionPrefab;

        public Transform Transform => transform;

        public IEnemyState CurrentState { get; private set; }
        public IEnemyState IdleState { get; private set; }
        public IEnemyState WalkingState { get; private set; }
        public IEnemyState ChaseExplodeState { get; private set; }

        public Transform Target { get; private set; }


        private void Awake()
        {
            IdleState = new EnemyStateIdle(this);
            WalkingState = new EnemyStateWalking(this);
            ChaseExplodeState = new EnemyStateChaseExplode(this, explodeDistance, explosionPrefab);
        }

        private void Start()
        {
            ChangeState(IdleState);
        }

        private void Update()
        {
            DetectTarget();
            CurrentState?.Update();
        }

        private void DetectTarget()
        {
            if (Target != null) return;

            Collider[] hits = Physics.OverlapSphere(transform.position, detectRadius, detectMask);
            if (hits.Length > 0)
            {
                Target = hits[0].transform;
                ChangeState(ChaseExplodeState);
            }
        }

        public void ChangeState(IEnemyState newState)
        {
            if (CurrentState != null)
                CurrentState.Exit();

            CurrentState = newState;
            CurrentState?.Enter(this);
        }



        public float GetWalkSpeed() => walkSpeed;
        public float GetRunSpeed() => runSpeed;
        public Transform GetTarget() => Target;
    }
}
