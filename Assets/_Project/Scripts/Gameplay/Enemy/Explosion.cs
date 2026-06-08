using UnityEngine;

namespace Gameplay.Enemy
{
    public class Explosion : MonoBehaviour
    {
        [SerializeField] private float radius = 3f;
        [SerializeField] private float damage = 50f;
        [SerializeField] private LayerMask damageMask = ~0;
        [SerializeField] private float lifeTime = 0.2f;

        private void Start()
        {
            ApplyDamage();
            if (lifeTime > 0f)
                Destroy(gameObject, lifeTime);
        }

        private void ApplyDamage()
        {
            var colliders = Physics.OverlapSphere(transform.position, radius, damageMask);
            foreach (var c in colliders)
            {
                var dmg = c.GetComponent<IDamageable>();
                if (dmg != null)
                {
                    dmg.TakeDamage(damage);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
