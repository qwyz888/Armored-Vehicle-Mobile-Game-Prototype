using UnityEngine;
using Gameplay.Enemy;

namespace Gameplay.Weapon
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float speed = 100f;
        [SerializeField] private float lifetime = 2f;
        [SerializeField] private float damage = 25f;

        private void Start()
        {
            Destroy(gameObject, lifetime);
        }

        private void Update()
        {
            transform.position +=
                transform.forward * speed * Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            HandleHit(other.gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            HandleHit(collision.gameObject);
        }

        private void HandleHit(GameObject other)
        {
            var dmg = other.GetComponentInParent<IDamageable>();
            if (dmg != null)
            {
                dmg.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
