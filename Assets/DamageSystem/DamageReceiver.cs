using DamageSystem.Health;
using UnityEngine;
using UnityEngine.Events;

namespace DamageSystem {
    public class DamageReceiver : MonoBehaviour {
        [SerializeField] public float maxHealth = 100f;
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private DeathAction deathAction = DeathAction.Destroy;
        [SerializeField] private LayerMask damageSources;
        [SerializeField] private UnityEvent OnDeath;
        [SerializeField] private UnityEvent<float> OnDamageReceived;
        public float health;
        private Vector3 initialPosition;
        
        
        [SerializeField]
        private float threshold = 0f;
        [SerializeField]
        private float finalDmgReduction = 0f;
        [SerializeField]
        private Color healthBarUnderThresholdColor = Color.green;
        private float actualDmgReduction = 0f;

        private enum DeathAction {
            RespawnAtInitialPosition,
            Destroy,
            None,
        }

        private void Awake() {
            health = maxHealth;
            if (healthBar) healthBar.SetMaxHealth(maxHealth);
            initialPosition = transform.position;
        }

        private void OnCollisionEnter2D(Collision2D other) {
            if (damageSources != (damageSources | 1 << other.gameObject.layer)) return;
            other.gameObject.TryGetComponent(out CollisionDamageDealer damageDealer);
            if (!damageDealer) return;
            TakeDamage(damageDealer.GetDamage());
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (damageSources != (damageSources | 1 << other.gameObject.layer)) return;
            Weapon weapon = other.gameObject.GetComponentInParent<Weapon>();
            if (weapon == null) return;
            TakeDamage(weapon.GetDamage());
        }

        public void TakeDamage(float amount) {
            OnDamageReceived.Invoke(amount);

            if (IsUnderTreshold()) {
                healthBar.SetColor(healthBarUnderThresholdColor);
            }

            actualDmgReduction = IsUnderTreshold() ? finalDmgReduction : 0f;

            health -= amount * (1f - actualDmgReduction);
            health = Mathf.Clamp(health, 0, maxHealth);
            if (health == 0) {
                Die();
            }

            if (healthBar) healthBar.SetHealth(health);

            void Die() {
                OnDeath.Invoke();

                if (deathAction == DeathAction.Destroy) {
                    Destroy(gameObject);
                } else if (deathAction == DeathAction.RespawnAtInitialPosition) {
                    transform.position = initialPosition;
                    health = maxHealth;
                } else if (deathAction == DeathAction.None) {
                    return;
                }
            }
        }
        public bool IsUnderTreshold() {
            return health < maxHealth * threshold;
        }
    }
}
