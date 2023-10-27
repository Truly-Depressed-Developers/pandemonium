using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {
    public class Movement : MonoBehaviour {
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private float dashTime = 0.5f;
        [SerializeField] private float dashMultiplayer = 4f;
        
        private Vector2 direction = new (0, 0);
        private float lastDashTime = 0;

        public void Update() {
            MovePlayer();
        }
        
        public void OnIndicateMovement(InputAction.CallbackContext ctx) {
            direction = ctx.ReadValue<Vector2>().normalized;
        }

        private void MovePlayer() {
            Transform playerTransform = gameObject.transform;

            float isDashActivated = Time.time - lastDashTime < dashTime ? dashMultiplayer : 1;
            
            playerTransform.Translate(direction.y * moveSpeed * isDashActivated * Time.deltaTime * Vector3.up);
            playerTransform.Translate(direction.x * moveSpeed * isDashActivated * Time.deltaTime * Vector3.right);
        }

        public void OnIndicateDash(InputAction.CallbackContext ctx) {
            if (ctx.canceled) return;

            lastDashTime = Time.time;
        }
    }
}
