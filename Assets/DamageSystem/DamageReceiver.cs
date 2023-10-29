using System;
using DamageSystem.Health;
using UnityEngine;
using UnityEngine.Events;

namespace DamageSystem {
    public class DamageReceiver : MonoBehaviour {
        [field: SerializeField] public bool IsInvulnerable { get; private set; }
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

        private Player.Movement movement;

        private enum DeathAction {
            RespawnAtInitialPosition,
            Destroy,
            None,
        }

        public void AddHealth(float amount) {
            health = Mathf.Clamp(health + amount, 0, maxHealth);

            if (healthBar) {
                healthBar.SetHealth(health);
            }
        }

        private void Awake() {
            health = maxHealth;
            if (healthBar) healthBar.SetMaxHealth(maxHealth);
            initialPosition = transform.position;
            movement = GetComponent<Player.Movement>();
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
            if (weapon != null) {
                TakeDamage(weapon.GetDamage());
            }
            CollisionDamageDealer collisionDamageDealer = other.gameObject.GetComponent<CollisionDamageDealer>();
            if (collisionDamageDealer != null) {
                TakeDamage(collisionDamageDealer.GetDamage());
                if (movement && movement.isInDashMove()) return;
                if (collisionDamageDealer.gameObject.GetComponent<BulletBase>()) {
                    Destroy(collisionDamageDealer.gameObject);
                }
            }
        }

        public void TakeDamage(float amount) {
            if (IsInvulnerable) return;
            if (movement && movement.isInDashMove()) return;

            OnDamageReceived.Invoke(amount);

            if (IsUnderThreshold()) {
                healthBar.SetColor(healthBarUnderThresholdColor);
            }

            actualDmgReduction = IsUnderThreshold() ? finalDmgReduction : 0f;

            health -= amount * (1f - actualDmgReduction);
            health = Mathf.Clamp(health, 0, maxHealth);
            if (health == 0) {
                Die();
            }

            if (healthBar) healthBar.SetHealth(health);

            void Die() {
                OnDeath.Invoke();

                switch (deathAction) {
                    case DeathAction.Destroy:
                        Destroy(gameObject);
                        break;
                    case DeathAction.RespawnAtInitialPosition:
                        transform.position = initialPosition;
                        health = maxHealth;
                        break;
                    case DeathAction.None:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        public bool IsUnderThreshold() {
            return health < maxHealth * threshold;
        }
        
        public void SetInvulnerable() {
            IsInvulnerable = true;
        }

        public void SetInvulnerable(bool val) {
            IsInvulnerable = val;
        }

        public void SetVulnerable() {
            IsInvulnerable = false;
        }
    }
    
}
