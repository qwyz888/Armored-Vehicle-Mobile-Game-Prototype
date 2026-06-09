using UnityEngine;

namespace Gameplay.Enemy
{
    public class Explosion : MonoBehaviour
    {
        [SerializeField] private float radius = 3f;
        [SerializeField] private float damage = 50f;
        [SerializeField] private LayerMask damageMask = ~0;
        [SerializeField] private float lifeTime = 2f;

        private void Start()
        {
            ApplyDamage();

            // Ensure particle systems run in world space and compute a safe lifetime
            float particleMax = 0f;
            var systems = GetComponentsInChildren<ParticleSystem>(true);
            foreach (var ps in systems)
            {
                var main = ps.main;
                // set simulation space to World so particles are not affected if parents are destroyed
                main.simulationSpace = ParticleSystemSimulationSpace.World;

                // estimate total duration: duration + startLifetime (use constantMax for MinMaxCurve)
                float duration = main.duration;
                float startLifetime = main.startLifetime.constantMax;
                particleMax = Mathf.Max(particleMax, duration + startLifetime);
                // ensure particle plays
                if (!ps.isPlaying) ps.Play();
            }

            float destroyAfter = lifeTime;
            if (particleMax > 0f)
                destroyAfter = Mathf.Max(destroyAfter, particleMax + 0.1f);

            if (destroyAfter > 0f)
                Destroy(gameObject, destroyAfter);
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
