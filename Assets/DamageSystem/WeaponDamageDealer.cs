using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DamageSystem {
    public class WeaponDamageDealer : MonoBehaviour, IActiveDamageDealer {
        [SerializeField] private Weapon weapon;
        [SerializeField] private Movement movement;

        public float GetDamage() {
            return weapon.GetDamage();
        }

        public void OnAttack(InputAction.CallbackContext ctx) {
            if(ctx.ReadValue<float>() == 0f) return;
            if (!weapon || movement.isInDashMove()) return;
            weapon.Attack();
        }
    }
}
