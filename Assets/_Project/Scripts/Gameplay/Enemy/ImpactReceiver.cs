using System.Collections;
using UnityEngine;

namespace Gameplay.Enemy
{
    public class ImpactReceiver : MonoBehaviour
    {
        [Header("Impact")]
        [SerializeField] private float maxDisplace = 0.5f;
        [SerializeField] private float maxRotation = 10f;
        [SerializeField] private float recoverSpeed = 6f;

        private Vector3 _initialPos;
        private Quaternion _initialRot;
        private Coroutine _impactCoroutine;

        private void Awake()
        {
            _initialPos = transform.localPosition;
            _initialRot = transform.localRotation;
        }

        // Apply impact without using physics. hitDirection should be from bullet to target in world space.
        public void ApplyImpact(Vector3 hitDirection, float strength = 1f, float duration = 0.15f)
        {
            float s = Mathf.Clamp01(strength);
            Vector3 dir = hitDirection.normalized;
            // compute local displacement opposite to hit direction projected on XZ plane
            Vector3 localDir = transform.InverseTransformDirection(dir);
            Vector3 displacement = new Vector3(localDir.x, localDir.y * 0.25f, localDir.z) * (maxDisplace * s);

            // rotation: tilt away from hit direction
            float rotAmount = maxRotation * s;
            Vector3 rotAxis = Vector3.Cross(Vector3.up, dir).normalized;
            Quaternion targetRot = Quaternion.AngleAxis(rotAmount, rotAxis) * _initialRot;

            if (_impactCoroutine != null)
                StopCoroutine(_impactCoroutine);
            _impactCoroutine = StartCoroutine(ImpactRoutine(displacement, targetRot, duration));
        }

        private IEnumerator ImpactRoutine(Vector3 displacement, Quaternion targetRot, float duration)
        {
            // move quickly to displaced position
            Vector3 targetPos = _initialPos + displacement;
            float elapsed = 0f;
            float inDuration = Mathf.Max(0.02f, duration * 0.5f);

            while (elapsed < inDuration)
            {
                float t = elapsed / inDuration;
                transform.localPosition = Vector3.Lerp(_initialPos, targetPos, t);
                transform.localRotation = Quaternion.Slerp(_initialRot, targetRot, t);
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.localPosition = targetPos;
            transform.localRotation = targetRot;

            // hold briefly then recover
            yield return new WaitForSeconds(duration * 0.5f);

            while (Vector3.Distance(transform.localPosition, _initialPos) > 0.001f || Quaternion.Angle(transform.localRotation, _initialRot) > 0.1f)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, _initialPos, Time.deltaTime * recoverSpeed);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, _initialRot, Time.deltaTime * recoverSpeed);
                yield return null;
            }

            transform.localPosition = _initialPos;
            transform.localRotation = _initialRot;
            _impactCoroutine = null;
        }
    }
}
