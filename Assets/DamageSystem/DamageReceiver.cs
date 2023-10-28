using System;
using System.Collections;
using System.Collections.Generic;
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
        private float treshold = 0f;
        [SerializeField]
        private float finalDmgReduction = 0f;
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
            if (GetComponent<Weapon>()) return;
            if (damageSources != (damageSources | 1 << other.gameObject.layer)) return;
            IDamageDealer damageDealer = other.gameObject.GetComponentInParent<IDamageDealer>();
            if (damageDealer == null) return;
            TakeDamage(damageDealer.GetDamage());
        }

        public void TakeDamage(float amount) {
            OnDamageReceived.Invoke(amount);

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
            return health < maxHealth * treshold;
        }
    }
}
