using System;
using Cinemachine;
using UnityEngine;

namespace Camera {
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraShake : MonoBehaviour {
        private CinemachineVirtualCamera cinemachineVirtualCamera;
        private float shakeIntensity = 20f;
        private float shakeTime = 0.2f;

        private float timer;

        private void Awake() {
            cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        }

        public void ShakeCamera() {
            CinemachineBasicMultiChannelPerlin cbmcp =
                cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            cbmcp.m_AmplitudeGain = shakeIntensity;
            timer = shakeTime;
        }

        void StopShake() {
            CinemachineBasicMultiChannelPerlin cbmcp =
                cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            cbmcp.m_AmplitudeGain = 0f;
            timer = 0;
        }

        private void Update() {
            if (timer > 0) {
                timer -= Time.deltaTime;

                if (timer <= 0) {
                    StopShake();
                }
            }
        }
    }
}
