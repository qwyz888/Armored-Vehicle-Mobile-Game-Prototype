using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Health
{

    [RequireComponent(typeof(HealthComponent))]
    public class EnemyHealthUI : MonoBehaviour
    {
        [SerializeField] private Slider hpSlider;
        [SerializeField] private float showDuration = 3f;
        [SerializeField] private HealthComponent health;

        private Coroutine hideCoroutine;

        void Awake()
        {
            health = GetComponent<HealthComponent>();
            if (hpSlider != null)
            {
                hpSlider.minValue = 0f;
                hpSlider.maxValue = health.MaxHP;
                hpSlider.value = health.CurrentHP;
                hpSlider.gameObject.SetActive(false);
            }

            health.OnHealthChanged += OnHealthChanged;
            health.OnDeath += OnDeath;
        }

        void OnDestroy()
        {
            if (health != null)
            {
                health.OnHealthChanged -= OnHealthChanged;
                health.OnDeath -= OnDeath;
            }
        }

        void OnHealthChanged(float current, float max)
        {
            if (hpSlider == null) return;
            hpSlider.maxValue = max;
            hpSlider.value = current;

            if (current > 0f)
                ShowTemporary();
            else
                hpSlider.gameObject.SetActive(false);
        }

        void ShowTemporary()
        {
            if (hideCoroutine != null) StopCoroutine(hideCoroutine);
            hpSlider.gameObject.SetActive(true);
            hideCoroutine = StartCoroutine(HideAfterDelay());
        }

        IEnumerator HideAfterDelay()
        {
            yield return new WaitForSeconds(showDuration);
            if (health.CurrentHP > 0f) hpSlider.gameObject.SetActive(false);
        }

        void OnDeath()
        {
            if (hpSlider != null) hpSlider.gameObject.SetActive(false);
        }
    }
}