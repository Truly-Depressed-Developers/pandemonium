using System;
using System.Collections;
using DamageSystem;
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

        bool specialAttackActive = false;

        public void SpecialAttack() {
            if(!specialAttackActive) { 
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
