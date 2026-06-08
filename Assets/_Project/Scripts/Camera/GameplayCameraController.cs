using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Camera
{
    public class GameplayCameraController : MonoBehaviour
    {
        [Header("Menu Pose")]
        [SerializeField] private Vector3 menuPosition = new Vector3(0, 6, -12);
        [SerializeField] private Vector3 menuRotationEuler = new Vector3(20, 0, 0);
        [SerializeField] private float menuFOV = 40f;
        [SerializeField] private float gameplayFOV = 60f;

        [Header("References")]
        [SerializeField] private UnityEngine.Camera cam;
        [SerializeField] private FollowCamera followCamera;

        [Header("Follow")]
        [SerializeField] private Vector3 followOffset = new Vector3(0, 5, -10);

        [Header("Transition")]
        [SerializeField] private float transitionDuration = 1f;

        private bool _isInMenuPose = true;

        private void Reset()
        {
            menuPosition = transform.position;
            menuRotationEuler = transform.eulerAngles;
        }

        // GameplayCameraController only performs the transition and FOV changes.
        // Per-frame following is delegated to FollowCamera component.

        public async UniTask TransitionToFollow(Transform target)
        {
            if (target == null) return;
            // disable follow behaviour while we perform manual transition
            if (followCamera != null)
                followCamera.enabled = false;
            Vector3 startPos = transform.position;
            Quaternion startRot = transform.rotation;

            Vector3 endPos = target.position + followOffset;
            Quaternion endRot = Quaternion.LookRotation((target.position + target.forward * 10f) - endPos);

            float startFov = cam != null ? cam.fieldOfView : menuFOV;
            float endFov = gameplayFOV;

            float t = 0f;
            _isInMenuPose = false;

            while (t < transitionDuration)
            {
                t += Time.deltaTime;
                float k = Mathf.Clamp01(t / transitionDuration);
                transform.position = Vector3.Lerp(startPos, endPos, k);
                transform.rotation = Quaternion.Slerp(startRot, endRot, k);
                if (cam != null)
                    cam.fieldOfView = Mathf.Lerp(startFov, endFov, k);
                await UniTask.Yield();
            }

            transform.position = endPos;
            transform.rotation = endRot;
            if (cam != null)
                cam.fieldOfView = endFov;

            // re-enable follow behaviour and set target/offset
            if (followCamera != null)
            {
                followCamera.SetOffset(followOffset);
                followCamera.SetTarget(target);
                followCamera.enabled = true;
            }
        }

        public void SetMenuPose()
        {
            _isInMenuPose = true;

            // disable follow behaviour so it doesn't override our menu pose
            if (followCamera != null)
            {
                followCamera.SetTarget(null);
                followCamera.enabled = false;
            }

            transform.position = menuPosition;
            transform.rotation = Quaternion.Euler(menuRotationEuler);

            if (cam != null)
                cam.fieldOfView = menuFOV;
        }
    }
}
