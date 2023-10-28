using UnityEngine;

namespace DamageSystem {
    public abstract class Weapon : MonoBehaviour {
        public abstract float GetDamage();
        public abstract void Attack();
    }
}
