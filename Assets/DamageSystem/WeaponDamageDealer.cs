using System;
using System.Linq;
using Player;
using UnityEngine;

namespace DamageSystem {
    public class WeaponDamageDealer : MonoBehaviour, IActiveDamageDealer {
        [SerializeField] private Weapon weapon;
        [SerializeField] private Movement movement;

        public float GetDamage() {
            return weapon.GetDamage();
        }

        public void OnAttack() {
            if (!weapon || movement.isInDashMove()) return;

            weapon.Attack();
        }
    }
}
