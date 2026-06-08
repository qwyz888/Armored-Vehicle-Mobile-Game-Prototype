using System;
using UnityEngine;

namespace Gameplay.Enemy
{
    public class EnemyHealth : MonoBehaviour, IDamageable
    {
        [SerializeField] private float maxHealth = 100f;
        private float currentHealth;

        public event Action OnKilled;

        private void Awake()
        {
            currentHealth = maxHealth;
        }

        // allow spawner to configure health
        public void SetMaxHealth(float value)
        {
            maxHealth = value;
            currentHealth = maxHealth;
        }

        public void TakeDamage(float amount)
        {
            if (currentHealth <= 0f) return;

            currentHealth -= amount;
            if (currentHealth <= 0f)
            {
                currentHealth = 0f;
                Die();
            }
        }

        private void Die()
        {
            OnKilled?.Invoke();
            // try to play death animation if present
            //var animator = GetComponent<Animator>();
            //if (animator != null)
            //{
            //   // animator.SetTrigger("Die");
            //}
            //else
            //{
                Destroy(gameObject);
            //}
        }
    }
}
