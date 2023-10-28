using UnityEngine.InputSystem;

namespace DamageSystem {
    public interface IActiveDamageDealer : IDamageDealer {
        public void OnAttack(InputAction.CallbackContext ctx);
    }
}
