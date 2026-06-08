using UnityEngine;

namespace Gameplay.Enemy
{
    public class EnemyStateChaseExplode : IEnemyState
    {
        private EnemyAI enemy;
        private float explodeDistance;
        private GameObject explosionPrefab;

        public EnemyStateChaseExplode(EnemyAI enemy, float explodeDistance, GameObject explosionPrefab)
        {
            this.enemy = enemy;
            this.explodeDistance = explodeDistance;
            this.explosionPrefab = explosionPrefab;
        }

        public void Enter(EnemyAI enemy)
        {
            var anim = enemy.GetComponent<Animator>();
            if (anim != null) anim.SetTrigger("Run");
        }

        public void Exit()
        {
            var anim = enemy.GetComponent<Animator>();
            if (anim != null) anim.ResetTrigger("Run");
        }

        public void Update()
        {
            var target = enemy.GetTarget();
            if (target == null) return;

            Vector3 dir = (target.position - enemy.Transform.position);
            float dist = dir.magnitude;
            dir.Normalize();

            enemy.Transform.position += dir * (enemy.GetRunSpeed() * Time.deltaTime);

            if (dist <= explodeDistance)
            {
                Explode();
            }
        }

        private void Explode()
        {
            if (explosionPrefab != null)
            {
                GameObject.Instantiate(explosionPrefab, enemy.Transform.position, Quaternion.identity);
            }
            else
            {
                // fallback: create Explosion component
                var go = new GameObject("Explosion");
                go.transform.position = enemy.Transform.position;
                go.AddComponent<Explosion>();
            }

            // destroy enemy after explosion
            GameObject.Destroy(enemy.gameObject);
        }
    }
}
