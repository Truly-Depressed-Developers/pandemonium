using System;
using DamageSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {
    public class Movement : MonoBehaviour {
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private float dashCooldown = 0.5f;
        [SerializeField] private float dashTime = 0.15f;
        [SerializeField] private float dashMultiplayer = 4f;
        private readonly float dashStartTime = 0.5f;

        private Vector2 direction = new(0, 0);
        private Vector2 lastDashDirection = new(0, 1);
        private Vector2 dashDirection = new(0, 1);
        private float lastDashTime = float.NegativeInfinity;

        private FreezeReceiver freezeReceiver = null;

        private void Start() {
            freezeReceiver = GetComponent<FreezeReceiver>();
        }

        public void Update() {
            if(freezeReceiver && freezeReceiver.CanMove()) {
                MovePlayer();
            }
        }

        public void OnIndicateMovement(InputAction.CallbackContext ctx) {
            direction = ctx.ReadValue<Vector2>().normalized;
            if (direction.y != 0 || direction.x != 0) {
                lastDashDirection = direction;
            }
        }

        private void MovePlayer() {
            Transform playerTransform = gameObject.transform;
            float constCalculated = moveSpeed * Time.deltaTime;

            if (Time.time - lastDashTime < dashTime && Time.time > dashStartTime) {
                playerTransform.Translate(dashDirection.y * dashMultiplayer * constCalculated * Vector3.up);
                playerTransform.Translate(dashDirection.x * dashMultiplayer * constCalculated * Vector3.right);
                return;
            }

            playerTransform.Translate(direction.y * constCalculated * Vector3.up);
            playerTransform.Translate(direction.x * constCalculated * Vector3.right);
        }

        public void OnIndicateDash(InputAction.CallbackContext ctx) {
            if (ctx.canceled || Time.time - lastDashTime < dashCooldown + dashTime) return;

            // if (direction.y != 0 || direction.x != 0) {
                dashDirection = lastDashDirection;
            // }

            lastDashTime = Time.time;
        }
    }
}
