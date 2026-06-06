using UnityEngine;

namespace Gameplay.Weapon
{
    public class GunController : MonoBehaviour
    {
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private Transform muzzlePoint;

        [SerializeField] private float fireRate = 0.1f;

        private float _nextShotTime;

        private void Update()
        {
            if (!Input.GetMouseButton(0))
                return;

            if (Time.time < _nextShotTime)
                return;

            _nextShotTime = Time.time + fireRate;

            Shoot();
        }

        private void Shoot()
        {
            Instantiate(
                bulletPrefab,
                muzzlePoint.position,
                muzzlePoint.rotation);
        }
    }
}