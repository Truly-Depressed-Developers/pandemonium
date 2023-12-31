﻿using Cutscenes;
using DamageSystem;
using DamageSystem.Weapons.MeleeWeapon;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {
    public class Movement : MonoBehaviour {
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private float dashCooldown = 0.5f;
        [SerializeField] private float dashTime = 1.15f;
        [SerializeField] private float dashMultiplayer = 4f;

        [SerializeField] private float staminaCost;
        [SerializeField] private StaminaBar staminaBar;

        [SerializeField] private Animator anim;
        [SerializeField] private AudioSource dodgeSound;

        private readonly float dashStartTime = 0.5f;

        private Vector2 direction = new(0, 0);
        private Vector2 lastDashDirection = new(0, 1);
        private Vector2 dashDirection = new(0, 1);
        private float lastDashTime = float.NegativeInfinity;

        private bool inDashMove;
        [SerializeField] private FreezeReceiver freezeReceiver;
        [SerializeField] private Attack playerAttack;

        [SerializeField] private Rigidbody2D rb;

        public void FixedUpdate() {
            rb.velocity = Vector2.zero;
            
            if (CutsceneController.I.CurrentCutscene && CutsceneController.I.CurrentCutscene.BlockPlayerMovement)
                return;
            
            if(playerAttack.SpecialAttackActive)
                return;
                
            if(!freezeReceiver || !freezeReceiver.CanMove())
                return;
            
            MovePlayer();
        }

        public void OnIndicateMovement(InputAction.CallbackContext ctx) {
            direction = ctx.ReadValue<Vector2>().normalized;

            if (!playerAttack.SpecialAttackActive && (direction.y != 0 || direction.x != 0)) {
                anim.SetFloat("X", direction.x);
                anim.SetFloat("Y", direction.y);
                anim.SetFloat("SPEED", 1f);

                lastDashDirection = direction;
            } else {
                anim.SetFloat("SPEED", 0f);
            }
        }

        private void MovePlayer() {
            float constCalculated = moveSpeed;// * Time.deltaTime;

            if (Time.time - lastDashTime < dashTime && Time.time > dashStartTime) {
                rb.velocity = dashMultiplayer * constCalculated * dashDirection;
                // playerTransform.Translate(dashDirection.y * dashMultiplayer * constCalculated * Vector3.up);
                // playerTransform.Translate(dashDirection.x * dashMultiplayer * constCalculated * Vector3.right);
                inDashMove = true;
                return;
            }
            
            inDashMove = false;

            rb.velocity = constCalculated * direction;
            // playerTransform.Translate(direction.y * constCalculated * Vector3.up);
            // playerTransform.Translate(direction.x * constCalculated * Vector3.right);
        }

        public void OnIndicateDash(InputAction.CallbackContext ctx) {
            if (ctx.canceled || Time.time - lastDashTime < dashCooldown + dashTime) return;
            if (!staminaBar.TryUse(staminaCost)) return;
            // if (direction.y != 0 || direction.x != 0) {
            dashDirection = lastDashDirection;
            dodgeSound.Play();
            inDashMove = true;
            // }

            lastDashTime = Time.time;
        }

        public bool isInDashMove() {
            return inDashMove;
        }
    }
}
