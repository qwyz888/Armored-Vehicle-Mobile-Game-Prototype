using System.Collections;
using UnityEngine;

namespace Gameplay.Weapon
{
    public class TurretImpact : MonoBehaviour
    {
        [Header("Recoil Settings")]
        [SerializeField] private float maxRecoilDistance = 0.2f;
        [SerializeField] private float maxRecoilAngle = 8f;
        [SerializeField] private float recoilReturnSpeed = 8f;
        [SerializeField] private float recoilDuration = 0.1f;

        private Vector3 _initialLocalPos;
        private Quaternion _initialLocalRot;
        private Coroutine _currentCoroutine;

        private void Awake()
        {
            _initialLocalPos = transform.localPosition;
            _initialLocalRot = transform.localRotation;
        }

        public void TriggerImpact(float strength = 1f)
        {
            float clamped = Mathf.Clamp01(strength);
            if (_currentCoroutine != null)
                StopCoroutine(_currentCoroutine);
            _currentCoroutine = StartCoroutine(ImpactRoutine(clamped));
        }

        private IEnumerator ImpactRoutine(float strength)
        {
            float elapsed = 0f;
            // target offsets
            Vector3 targetPos = _initialLocalPos - Vector3.forward * (maxRecoilDistance * strength);
            // quickly move to target over recoilDuration
            Vector3 initEuler = _initialLocalRot.eulerAngles;
            float initPitch = initEuler.x;
            float initRoll = initEuler.z;
            float targetPitch = initPitch - (maxRecoilAngle * strength);

            while (elapsed < recoilDuration)
            {
                float t = elapsed / recoilDuration;
                transform.localPosition = Vector3.Lerp(_initialLocalPos, targetPos, t);

                // preserve current yaw (Y) controlled by TurretRotationController; only interpolate pitch (X) and keep roll
                float pitch = Mathf.LerpAngle(initPitch, targetPitch, t);
                float currentYaw = transform.localEulerAngles.y;
                transform.localEulerAngles = new Vector3(pitch, currentYaw, initRoll);

                elapsed += Time.deltaTime;
                yield return null;
            }

            // ensure set to target (preserve yaw)
            transform.localPosition = targetPos;
            {
                float currentYaw = transform.localEulerAngles.y;
                transform.localEulerAngles = new Vector3(targetPitch, currentYaw, initRoll);
            }

            // return smoothly to rest (only pitch and position recover; yaw preserved)
            while (Vector3.Distance(transform.localPosition, _initialLocalPos) > 0.001f || Mathf.Abs(Mathf.DeltaAngle(transform.localEulerAngles.x, initPitch)) > 0.1f)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, _initialLocalPos, Time.deltaTime * recoilReturnSpeed);

                float currentYaw = transform.localEulerAngles.y;
                float pitch = Mathf.LerpAngle(transform.localEulerAngles.x, initPitch, Time.deltaTime * recoilReturnSpeed);
                transform.localEulerAngles = new Vector3(pitch, currentYaw, initRoll);

                yield return null;
            }

            transform.localPosition = _initialLocalPos;
            // restore pitch/roll but leave yaw as current
            {
                float currentYaw = transform.localEulerAngles.y;
                transform.localEulerAngles = new Vector3(initPitch, currentYaw, initRoll);
            }
            _currentCoroutine = null;
        }
    }
}
