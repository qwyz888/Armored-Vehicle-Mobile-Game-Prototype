using UnityEngine;

namespace Gameplay.Enemy
{
    public class EnemyStateWalking : IEnemyState
    {
        private EnemyAI enemy;
        private float walkDuration;
        private float elapsed;

        public EnemyStateWalking(EnemyAI enemy)
        {
            this.enemy = enemy;
            walkDuration = Random.Range(1.5f, 4f);
        }

        public void Enter(EnemyAI enemy)
        {
            elapsed = 0f;
            var anim = enemy.GetComponent<Animator>();
            if (anim != null) anim.SetTrigger("Walk");
        }

        public void Exit()
        {
            var anim = enemy.GetComponent<Animator>();
            if (anim != null) anim.ResetTrigger("Walk");
        }

        public void Update()
        {
            elapsed += Time.deltaTime;
            // simple wandering forward
            enemy.Transform.position += enemy.Transform.forward * (enemy.GetWalkSpeed() * Time.deltaTime);

            if (elapsed >= walkDuration)
            {
                enemy.ChangeState(enemy.IdleState);
            }
        }
    }
}
