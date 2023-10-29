using System.Collections;
using System.Collections.Generic;
using Projectiles;
using UnityEngine;
using UnityEngine.Serialization;

namespace Morok {
    public enum Difficulty {
        Easy,
        Hard
    }

    public class Attack : MonoBehaviour {
        [Header("Spawn")] [SerializeField] private float spawnIdleAttackTime = 1f;
        [field: SerializeField] private Difficulty morokDifficulty = Difficulty.Easy;

        [Header("Projectile attack")] [SerializeField]
        private GameObject projectilePrefab;

        [SerializeField] private ProjectileConfig projectileAttackConfig;
        [SerializeField] private float projectileAttackCooldown = 1.5f;

        [Header("Aerial attack")] [SerializeField]
        private int aerialAttackRepeats = 4;

        [SerializeField] private float aerialAttackRepeatCooldown = 0.4f;
        [SerializeField] private float aerialAttackCooldown = 0.7f;
        [SerializeField] private GameObject pentagramPrefab;

        [Header("Laser attack")] [SerializeField]
        private int laserAttackRepeats = 3;

        [SerializeField] private float laserAttackRepeatCooldown = 0.8f;
        [SerializeField] private float laserAttackCooldown = 5f;
        [SerializeField] private GameObject laserPrefab;

        [Header("Bonus attack")] [SerializeField]
        private ProjectileConfig bonusProjectileAttackConfig;

        [SerializeField] private float bonusProjectileAttackChance = 0.15f;

        private readonly List<Transform> projectileSpawnPoints = new();
        private Transform target;

        private void Start() {
            Transform projectileSpawnPointsRoot = transform.GetChild(0).transform;

            foreach (Transform child in projectileSpawnPointsRoot) {
                projectileSpawnPoints.Add(child);
            }

            GameObject t = GameObject.FindGameObjectWithTag("Player");
            if (!t) t = GameObject.Find("Player");

            if (t) {
                target = t.transform;
            }

            StartCoroutine(AttackCoroutine());
        }

        private IEnumerator AttackCoroutine() {
            while (true) {
                yield return new WaitForSeconds(spawnIdleAttackTime);

                if (!target) break;

                float roll = Random.value;

                if (morokDifficulty == Difficulty.Easy) {
                    switch (roll) {
                        case <= 0.85f:
                            yield return ProjectileAttack();
                            break;
                        case < 1f:
                            yield return AerialAttack();
                            break;
                    }
                } else {
                    switch (roll) {
                        case <= 0.65f:
                            yield return ProjectileAttack();
                            break;
                        case < 0.85f:
                            yield return AerialAttack();
                            break;
                        case < 1f:
                            yield return LaserAttack();
                            break;
                    }
                }
            }
        }

        private IEnumerator ProjectileAttack() {
            DoSpawnProjectiles();
            yield return new WaitForSeconds(projectileAttackCooldown);
        }

        private void DoSpawnProjectiles(ProjectileConfig config = null) {
            foreach (var spawnPoint in projectileSpawnPoints) {
                GameObject projectile =
                    Instantiate(projectilePrefab, spawnPoint.transform.position, Quaternion.identity);
                Projectile projectileScript = projectile.GetComponent<Projectile>();

                Vector2 direction = (target.position - projectile.transform.position).normalized;

                projectileScript.SetDirection(direction);
                if (projectileAttackConfig) projectileScript.AssignConfig(config ? config : projectileAttackConfig);
            }
        }

        private IEnumerator AerialAttack() {
            for (int i = 0; i < aerialAttackRepeats; i++) {
                if (!target) break;

                DoSpawnPentagram();

                if (morokDifficulty == Difficulty.Hard && Random.value < bonusProjectileAttackChance) {
                    DoSpawnProjectiles(bonusProjectileAttackConfig);
                }

                if (i != aerialAttackRepeats - 1) {
                    yield return new WaitForSeconds(aerialAttackRepeatCooldown);
                }
            }

            yield return new WaitForSeconds(aerialAttackCooldown);
        }

        private void DoSpawnPentagram() {
            Instantiate(pentagramPrefab, (Vector2)target.transform.position - new Vector2(0f, 0.5f),
                Quaternion.identity);
        }

        private IEnumerator LaserAttack() {
            for (int i = 0; i < laserAttackRepeats; i++) {
                if (!target) break;

                DoSpawnLaser();

                if (morokDifficulty == Difficulty.Hard && Random.value < bonusProjectileAttackChance) {
                    DoSpawnProjectiles(bonusProjectileAttackConfig);
                }

                if (i != laserAttackRepeats - 1) {
                    yield return new WaitForSeconds(laserAttackRepeatCooldown);
                }
            }

            yield return new WaitForSeconds(laserAttackCooldown);
        }

        private void DoSpawnLaser() {
            GameObject laser = Instantiate(laserPrefab, (Vector2)target.transform.position - new Vector2(0f, 0.5f),
                Quaternion.identity);

            if (Random.value < 0.5f) {
                laser.transform.rotation = Quaternion.Euler(0, 0, 90);
            }
        }
    }
}
