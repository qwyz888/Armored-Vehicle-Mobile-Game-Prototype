using UnityEngine;

namespace Camera
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private Transform target;

        [Header("Position")]
        [SerializeField] private Vector3 offset = new Vector3(0, 5, -10);
        [SerializeField] private float followSmooth = 5f;

        [Header("Rotation")]
        [SerializeField] private Vector3 rotationEuler = new Vector3(15, 0, 0);
        [SerializeField] private float rotationSmooth = 5f;

        [SerializeField] private float lookAheadDistance = 10f;

        private void LateUpdate()
        {
            if (target == null) return;

            Vector3 desiredPosition = target.position + offset;

            transform.position = Vector3.Lerp(
                transform.position,
                desiredPosition,
                followSmooth * Time.deltaTime);

            Vector3 lookPoint = target.position + target.forward * lookAheadDistance;

            Quaternion desiredRotation = Quaternion.LookRotation(
                lookPoint - transform.position);

            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                desiredRotation,
                rotationSmooth * Time.deltaTime);
        }

        // Public API to configure camera at runtime
        public void SetTarget(Transform t) => target = t;
        public void SetOffset(Vector3 o) => offset = o;
        public void SetFollowSmooth(float s) => followSmooth = s;
        public void SetRotationSmooth(float s) => rotationSmooth = s;
        public void SetLookAheadDistance(float d) => lookAheadDistance = d;
    }
}