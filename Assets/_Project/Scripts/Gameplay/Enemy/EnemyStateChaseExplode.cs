using UnityEngine;

namespace Gameplay.Enemy
{
    public class EnemyStateChaseExplode : IEnemyState
    {
        private EnemyAI enemy;
        private float explodeDistance;
        private GameObject explosionPrefab;
        private float turnSpeed = 4.5f; 

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

            // rotate to face target on Y axis only
            Vector3 flatDir = new Vector3(dir.x, 0f, dir.z);
            if (flatDir.sqrMagnitude > 0.0001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(flatDir);
                enemy.Transform.rotation = Quaternion.Slerp(enemy.Transform.rotation, targetRot, turnSpeed * Time.deltaTime);
            }

            // move forward in local forward direction (so movement follows rotation)
            enemy.Transform.position += enemy.Transform.forward * (enemy.GetRunSpeed() * Time.deltaTime);

            if (dist <= explodeDistance)
            {
                Explode();
            }
        }

        private void Explode()
        {
            if (explosionPrefab != null)
            {
               var particle = GameObject.Instantiate(explosionPrefab, enemy.Transform.position, Quaternion.identity,null);
            }
            else
            {
                // fallback: create Explosion component
                var go = new GameObject("Explosion");
                go.transform.position = enemy.Transform.position;
                go.AddComponent<Explosion>();
            }

            GameObject.Destroy(enemy.gameObject);
        }
    }
}
