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
    }
}