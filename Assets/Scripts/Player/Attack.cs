using System;
using System.Collections;
using Camera;
using DamageSystem.Weapons.MeleeWeapon;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {
    public class Attack : MonoBehaviour {
        [SerializeField] private MeleeWeapon meleeWeapon;
        [SerializeField] private Collider2D specialWeaponCollider;
        [SerializeField] private GameObject crossWeapon;
        [SerializeField] private Movement movement;
        [SerializeField] private float staminaCost;
        [SerializeField] private StaminaBar staminaBar;

        private bool specialAttackActive;

        public void SpecialAttack(InputAction.CallbackContext ctx) {
            if (ctx.ReadValue<float>() == 0f) return;
            if (specialAttackActive || movement.isInDashMove()) return;
            if (!staminaBar || !staminaBar.TryUse(staminaCost)) return;
            
            CameraZoomIn.instance.ZoomInCamera();
            specialAttackActive = true;
            StartCoroutine(FreeTheSpirit());
        }

        private IEnumerator FreeTheSpirit() {
            meleeWeapon.gameObject.SetActive(false);
            specialWeaponCollider.enabled = true;
            crossWeapon.gameObject.SetActive(true);

            yield return new WaitForSeconds(2f);

            meleeWeapon.gameObject.SetActive(true);
            specialWeaponCollider.enabled = false;
            crossWeapon.gameObject.SetActive(false);

            CameraZoomIn.instance.ZoomOutCamera();
            specialAttackActive = false;
        }
    }
}
