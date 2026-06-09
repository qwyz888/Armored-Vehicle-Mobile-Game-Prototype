using UnityEngine;

namespace Gameplay.Weapon
{
    public class TurretRotationController : MonoBehaviour
    {
        [SerializeField] private float sensitivity = 0.2f;
        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] private float minAngle = -80f;
        [SerializeField] private float maxAngle = 80f;

        private float _currentAngle;

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                float deltaX = Input.GetAxis("Mouse X");

                _currentAngle += deltaX * sensitivity;

                _currentAngle = Mathf.Clamp(
                    _currentAngle,
                    minAngle,
                    maxAngle);
                // input handling only; actual recoil handled by TurretImpact
                // no additional code here
            }

            Quaternion targetRotation =
                Quaternion.Euler(0, _currentAngle, 0);

            transform.localRotation = Quaternion.Lerp(
                transform.localRotation,
                targetRotation,
                rotationSpeed * Time.deltaTime);

        }
    }
}