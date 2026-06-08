using UnityEngine;

namespace Gameplay.Car
{
    public class VehicleController : MonoBehaviour
    {
        [SerializeField] private float speed = 10f;
        private bool _isMoving;

        public void StartMoving()
        {
            _isMoving = true;
        }

        public void StopMoving()
        {
            _isMoving = false;
        }

        private void Update()
        {
            if (!_isMoving)
                return;

            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }
}