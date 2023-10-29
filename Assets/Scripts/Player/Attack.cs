using System.Collections;
using Camera;
using DamageSystem;
using DamageSystem.Weapons.MeleeWeapon;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {
    public class Attack : MonoBehaviour {
        [SerializeField] private MeleeWeapon meleeWeapon;
        [SerializeField] private Collider2D specialWeaponCollider;
        [SerializeField] private new GameObject particleSystem;
        [SerializeField] private FreezeInvoker crossWeaponFreezeInvoker;
        [SerializeField] private Movement movement;
        [SerializeField] private float staminaCost;
        [SerializeField] private StaminaBar staminaBar;
        [SerializeField] private AudioSource audio;

        public bool SpecialAttackActive => specialAttackCoroutine != null;
        private Coroutine specialAttackCoroutine;

        public void SpecialAttack(InputAction.CallbackContext ctx) {
            if (ctx.ReadValue<float>() == 0f) return;
            if (SpecialAttackActive || movement.isInDashMove()) return;
            if (!staminaBar || !staminaBar.TryUse(staminaCost)) return;
            

            audio.Play();
            specialAttackCoroutine = StartCoroutine(FreeTheSpirit());
        }

        private IEnumerator FreeTheSpirit() {
            FreeTheSpiritBegin();

            yield return new WaitForSeconds(2f);

            FreeTheSpiritEnd();
        }

        private void FreeTheSpiritBegin() {
            meleeWeapon.gameObject.SetActive(false);
            specialWeaponCollider.enabled = true;
            
            particleSystem.SetActive(true);
            CameraZoomIn.instance.ZoomInCamera();
        }

        private void FreeTheSpiritEnd() {
            meleeWeapon.gameObject.SetActive(true);
            specialWeaponCollider.enabled = false;
            
            particleSystem.SetActive(false);
            CameraZoomIn.instance.ZoomOutCamera();
            
            specialAttackCoroutine = null;
        }

        public void CancelSpecialAttack() {
            if (!SpecialAttackActive) return;

            audio.Stop();
            crossWeaponFreezeInvoker.CancelFreeze();
            StopCoroutine(specialAttackCoroutine);
            FreeTheSpiritEnd();
        }
    }
}
