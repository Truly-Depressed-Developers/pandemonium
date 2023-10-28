using System;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;

namespace Camera {
    internal enum CameraZoomStatus {
        ZoomIn,
        ZoomOut,
        Static,
    }
    
    public class CameraZoomIn : MonoBehaviour {
        public static CameraZoomIn instance;
        
        [SerializeField] private float defaultScope = 8.25f;
        [SerializeField] private float zoomInScope = 5f;
        [SerializeField] private float zoomSpeed = 13f;

        private CameraZoomStatus cameraZoomStatus = CameraZoomStatus.Static;
        private double TOLERANCE = 0.00005f;
        private CinemachineVirtualCamera cvc;

        private void Awake() {
            instance = this;
            cvc = GetComponent<CinemachineVirtualCamera>();
            cvc.m_Lens.OrthographicSize = defaultScope;
        }

        public void ZoomInCamera() {
            cameraZoomStatus = CameraZoomStatus.ZoomIn;
        }

        public void ZoomOutCamera() {
            cameraZoomStatus = CameraZoomStatus.ZoomOut;
        }

        private void Update() {
            switch (cameraZoomStatus) {
                case CameraZoomStatus.ZoomIn:
                    cvc.m_Lens.OrthographicSize -= zoomSpeed * Time.deltaTime;
                    break;
                case CameraZoomStatus.ZoomOut:
                    cvc.m_Lens.OrthographicSize += zoomSpeed * Time.deltaTime;
                    break;
                case CameraZoomStatus.Static:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            cvc.m_Lens.OrthographicSize = Math.Clamp(cvc.m_Lens.OrthographicSize, zoomInScope, defaultScope);
            
            if (Math.Abs(cvc.m_Lens.OrthographicSize - zoomInScope) < TOLERANCE ||
                Math.Abs(cvc.m_Lens.OrthographicSize - defaultScope) < TOLERANCE) {
                cameraZoomStatus = CameraZoomStatus.Static;
            }
        }
    }
}
