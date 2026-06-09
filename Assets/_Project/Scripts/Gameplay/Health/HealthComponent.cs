using System;
using Gameplay.Enemy;
using UnityEngine;

namespace Gameplay.Health
{
    public class HealthComponent : MonoBehaviour, IDamageable
    {
        [SerializeField] private float maxHP = 100f; 
        public float CurrentHP { get; private set; }
        public float MaxHP { get { return maxHP; } }

        public event Action<float, float> OnHealthChanged;
        public event Action OnDeath;

        void Awake()
        {
            CurrentHP = maxHP;
            OnHealthChanged?.Invoke(CurrentHP, maxHP);
        }

        public void TakeDamage(float damage)
        {
            if (CurrentHP <= 0f) return;
            CurrentHP = Mathf.Max(0f, CurrentHP - damage);
            OnHealthChanged?.Invoke(CurrentHP, maxHP);
            if (CurrentHP <= 0f)
                OnDeath?.Invoke();
        }

        public void Heal(float amount)
        {
            if (CurrentHP <= 0f) return;
            CurrentHP = Mathf.Clamp(CurrentHP + amount, 0f, maxHP);
            OnHealthChanged?.Invoke(CurrentHP, maxHP);
        }

        public void SetMaxHP(float hp)
        {
            maxHP = hp;
            CurrentHP = hp;
            OnHealthChanged?.Invoke(CurrentHP, maxHP);
        }
    }
}