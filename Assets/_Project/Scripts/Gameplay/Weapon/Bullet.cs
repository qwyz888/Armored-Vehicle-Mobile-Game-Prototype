using UnityEngine;
using Gameplay.Enemy;

namespace Gameplay.Weapon
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float speed = 100f;
        [SerializeField] private float lifetime = 2f;
        [SerializeField] private float damage = 25f;
        [Header("Impact")]
        [SerializeField] private float impactForce = 5f;
        [SerializeField] private float impactTorque = 2f;

        private void Start()
        {
            Destroy(gameObject, lifetime);

            // trigger turret impact animation on the firing turret if available so automatic fire causes repeated recoil
            var turretImpact = GetComponentInParent<TurretImpact>();
            if (turretImpact != null)
            {
                turretImpact.TriggerImpact(1f);
            }
            else
            {
                var allImpacts = FindObjectsOfType<TurretImpact>();
                float bestDist = float.MaxValue;
                TurretImpact best = null;
                foreach (var t in allImpacts)
                {
                    float d = Vector3.Distance(transform.position, t.transform.position);
                    if (d < bestDist)
                    {
                        bestDist = d;
                        best = t;
                    }
                }

                if (best != null && bestDist < 10f)
                    best.TriggerImpact(1f);
            }
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

            // apply non-physics impact if target has ImpactReceiver
            var receiver = other.GetComponentInParent<Gameplay.Enemy.ImpactReceiver>();
            if (receiver != null)
            {
                Vector3 dir = (other.transform.position - transform.position).normalized;
                receiver.ApplyImpact(dir, 1f, 0.15f);
            }

            Destroy(gameObject);
        }
    }
}
