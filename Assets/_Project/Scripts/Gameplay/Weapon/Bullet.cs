using UnityEngine;

namespace Gameplay.Weapon
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float speed = 100f;
        [SerializeField] private float lifetime = 2f;

        private void Start()
        {
            Destroy(gameObject, lifetime);
        }

        private void Update()
        {
            transform.position +=
                transform.forward * speed * Time.deltaTime;
        }
    }
}