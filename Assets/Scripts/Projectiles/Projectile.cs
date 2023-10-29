using System;
using UnityEngine;
using Cam = UnityEngine.Camera;

namespace Projectiles {
    public class Projectile : ProjectileBase {
        [SerializeField] private ProjectileConfig config;

        private Vector3 direction = new Vector3(1, 0, 0);
        private float currentSpeed;
        private float spawnTime;

        private void Awake() {
            spawnTime = Time.time;
        }

        private void Start() {
            currentSpeed = config.initialSpeed;
        }

        private void Update() {
            ApplyVelocityChange();
        
            transform.Translate(currentSpeed * Time.deltaTime * direction, Space.World);

            if (Time.time - spawnTime > config.lifetime) {
                Destroy(gameObject);
                return;
            }
            
            DestroyWhenOutOfScreen();
        }

        public void AssignConfig(ProjectileConfig newConfig) {
            config = newConfig;
        }

        public void SetDirection(Vector3 newDirection) {
            direction = newDirection;
        }

        private void DestroyWhenOutOfScreen() {
            Vector3 screenPos = Cam.main.WorldToViewportPoint(transform.position);
            if (!(screenPos.x >= 0 && screenPos.x <= 1 && screenPos.y >= 0 && screenPos.y <= 1)) {
                Destroy(gameObject);
            }
        }

        private void ApplyVelocityChange() {
            currentSpeed = config.changeSettings.mode switch {
                SpeedChangeMode.Accelerate when currentSpeed < config.changeSettings.threshold => Math.Clamp(
                    currentSpeed + Time.deltaTime * config.changeSettings.value, config.initialSpeed,
                    config.changeSettings.threshold),
                SpeedChangeMode.Decelerate when currentSpeed > config.changeSettings.threshold => Math.Clamp(
                    currentSpeed + Time.deltaTime * config.changeSettings.value, config.changeSettings.threshold,
                    config.initialSpeed),
                _ => currentSpeed
            };
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag("Wall")) {
                Destroy(gameObject);
            }
        }
    }
}
