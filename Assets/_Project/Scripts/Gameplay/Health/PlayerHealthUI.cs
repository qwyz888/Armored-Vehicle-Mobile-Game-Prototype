using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Health
{
    [RequireComponent(typeof(HealthComponent))]
    public class PlayerHealthUI : MonoBehaviour
    {
        [SerializeField] private Slider hpSlider;
        [SerializeField] private HealthComponent health;

        void Awake()
        {
            health = GetComponent<HealthComponent>();
            if (hpSlider != null)
            {
                hpSlider.minValue = 0f;
                hpSlider.maxValue = health.MaxHP;
                hpSlider.value = health.CurrentHP;
                hpSlider.gameObject.SetActive(true); 
            }

            health.OnHealthChanged += UpdateSlider;
            health.OnDeath += OnDeath;
        }

        void OnDestroy()
        {
            if (health != null)
            {
                health.OnHealthChanged -= UpdateSlider;
                health.OnDeath -= OnDeath;
            }
        }

        void UpdateSlider(float current, float max)
        {
            if (hpSlider == null) return;
            hpSlider.maxValue = max;
            hpSlider.value = current;
        }

        void OnDeath()
        {
            if (hpSlider != null) hpSlider.value = 0f;
        }
    }
}