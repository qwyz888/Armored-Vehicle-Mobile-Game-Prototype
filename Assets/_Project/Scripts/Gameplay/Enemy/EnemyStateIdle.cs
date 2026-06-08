using System;
using UnityEngine;

namespace Gameplay.Enemy
{
    public class EnemyStateIdle : IEnemyState
    {
        private EnemyAI enemy;
        private float idleTime = 0f;
        private float maxIdle = 2f;

        public EnemyStateIdle(EnemyAI enemy)
        {
            this.enemy = enemy;
            maxIdle = UnityEngine.Random.Range(1.5f, 4f);
        }

        public void Enter(EnemyAI enemy)
        {
            idleTime = 0f;
            // trigger idle animation
            var anim = enemy.GetComponent<Animator>();
            if (anim != null) anim.SetTrigger("Idle");
        }

        public void Exit()
        {
            var anim = enemy.GetComponent<Animator>();
            if (anim != null) anim.ResetTrigger("Idle");
        }

        public void Update()
        {
            idleTime += Time.deltaTime;
            if (idleTime >= maxIdle)
            {
                enemy.ChangeState(enemy.WalkingState);
            }
        }
    }
}
