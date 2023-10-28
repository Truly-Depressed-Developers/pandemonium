using System;
using Cinemachine;
using UnityEngine;

namespace Camera {
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraShake : MonoBehaviour {
        [SerializeField] private float shakeIntensity = 10f;
        [SerializeField] private float shakeTime = 0.2f;
        [SerializeField] private float shakeFrequency = 1f;

        private float timer;

        private CinemachineVirtualCamera cinemachineVirtualCamera;
        private CinemachineBasicMultiChannelPerlin cbmcp;
        
        private void Awake() {
            cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
            cbmcp = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            cbmcp.m_FrequencyGain = shakeFrequency;
        }

        public void ShakeCamera() {
            cbmcp.m_AmplitudeGain = shakeIntensity;
            timer = shakeTime;
        }

        public void StopShake() {
            cbmcp.m_AmplitudeGain = 0f;
            timer = 0;
        }

        private void Update() {
            if (!(timer > 0)) return;
            
            timer -= Time.deltaTime;

            if (timer <= 0) {
                StopShake();
            }
        }
    }
}
