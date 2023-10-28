using System;
using System.Linq;
using UnityEngine;

namespace DamageSystem {
    public class WeaponDamageDealer : MonoBehaviour, IActiveDamageDealer {
        [SerializeField] private Weapon weapon;
        public float GetDamage() {
            return weapon.GetDamage();
        }

        public void OnAttack() {
            if (!weapon) return;

            weapon.Attack();
        }
    }
}
