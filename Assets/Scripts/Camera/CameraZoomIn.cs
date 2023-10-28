using System;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;

namespace Camera {
    public class CameraZoomIn : MonoBehaviour {
        private enum CameraZoomStatus {
            ZoomIn,
            ZoomOut,
            Static,
        }
        
        private CinemachineVirtualCamera cinemachineVirtualCamera;
        [SerializeField] private float defaultScope = 8.25f;
        [SerializeField] private float zoomInScope = 5f;
        [SerializeField] private float zoomSpeed = 13f;

        private CameraZoomStatus cameraZoomStatus = CameraZoomStatus.Static;

        public static CameraZoomIn instance;

        private void Awake() {
            instance = this;
            cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
            cinemachineVirtualCamera.m_Lens.OrthographicSize = defaultScope;
        }

        public void ZoomInCamera() {
            // cinemachineVirtualCamera.m_Lens.OrthographicSize = zoomInScope;
            cameraZoomStatus = CameraZoomStatus.ZoomIn;
        }

        public void ZoomOutCamera() {
            // cinemachineVirtualCamera.m_Lens.OrthographicSize = defaultScope;
            cameraZoomStatus = CameraZoomStatus.ZoomOut;
        }

        private void Update() {
            switch (cameraZoomStatus) {
                case CameraZoomStatus.Static:
                    return;
                case CameraZoomStatus.ZoomIn:
                    cinemachineVirtualCamera.m_Lens.OrthographicSize -= zoomSpeed * Time.deltaTime;
                    break;
                case CameraZoomStatus.ZoomOut:
                    cinemachineVirtualCamera.m_Lens.OrthographicSize += zoomSpeed * Time.deltaTime;
                    break;
            }

            if (cinemachineVirtualCamera.m_Lens.OrthographicSize <= zoomInScope ||
                cinemachineVirtualCamera.m_Lens.OrthographicSize >= defaultScope) {
                cameraZoomStatus = CameraZoomStatus.Static;
            }
        }

        
        private void Editor_FixCameraSizeToDefault() {
            
        }
    }
}
