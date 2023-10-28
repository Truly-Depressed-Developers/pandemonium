using System.Collections;
using DamageSystem.Weapons.MeleeWeapon;
using UnityEngine;

namespace Player {
    public class Attack : MonoBehaviour {

        [SerializeField]
        private MeleeWeapon meleeWeapon;
        [SerializeField]
        private Collider2D specialWeaponCollider;
        [SerializeField]
        private GameObject crossWeapon;
        [SerializeField] 
        private Movement movement;
        [SerializeField] private float staminaCost;
        [SerializeField] private StaminaBar staminaBar;

        bool specialAttackActive = false;

        public void SpecialAttack() {
            if(specialAttackActive || movement.isInDashMove()) return; 

            if (staminaBar) {
                if (staminaBar.TryUse(staminaCost)) {
                    specialAttackActive = true;
                    StartCoroutine(freeTheSpirit());
                }
            } else {
                specialAttackActive = true;
                StartCoroutine(freeTheSpirit());
            }
        }

        private IEnumerator freeTheSpirit() {
            meleeWeapon.gameObject.SetActive(false);
            specialWeaponCollider.enabled = true;
            crossWeapon.gameObject.SetActive(true);

            yield return new WaitForSeconds(2f);

            meleeWeapon.gameObject.SetActive(true);
            specialWeaponCollider.enabled = false;
            crossWeapon.gameObject.SetActive(false);

            specialAttackActive=false;
        }
    }
}
