using UnityEngine;

namespace Gameplay.Enemy
{
    [RequireComponent(typeof(Gameplay.Health.HealthComponent))]
    public class DeathExplosion : MonoBehaviour
    {
        [SerializeField] private GameObject explosionPrefab;

        private Gameplay.Health.HealthComponent _health;

        private void Awake()
        {
            _health = GetComponent<Gameplay.Health.HealthComponent>();
            if (_health != null)
                _health.OnDeath += OnHealthDeath;
        }

        private void OnDestroy()
        {
            if (_health != null)
                _health.OnDeath -= OnHealthDeath;
        }

        private void OnHealthDeath()
        {
            if (explosionPrefab != null)
            {
                GameObject.Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                var go = new GameObject("Explosion");
                go.transform.position = transform.position;
                go.AddComponent<Explosion>();
            }

            // destroy the enemy gameobject after spawning explosion
            GameObject.Destroy(gameObject);
        }
    }
}
