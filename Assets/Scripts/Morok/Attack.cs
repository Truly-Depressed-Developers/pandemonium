using System.Collections;
using System.Collections.Generic;
using Projectiles;
using UnityEngine;

namespace Morok {
    public class Attack : MonoBehaviour {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private ProjectileConfig projectileConfig;

        [SerializeField] private float spawnAttackCooldown = 1f;
        [SerializeField] private float projectileAttackCooldown = 1.5f;

        [SerializeField] private float aerialAttackRepeats = 3f;
        [SerializeField] private float aerialAttackRepeatCooldown = 0.4f;
        [SerializeField] private float aerialAttackCooldown = 0.7f;

        private List<Transform> projectileSpawnPoints = new();
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
                yield return new WaitForSeconds(spawnAttackCooldown);
                
                if (!target) break;

                float roll = Random.value;

                if (roll <= 1f) yield return ProjectileAttack();
            }
        }

        private IEnumerator ProjectileAttack() {
            foreach (var spawnPoint in projectileSpawnPoints) {
                GameObject projectile = Instantiate(projectilePrefab, spawnPoint.transform.position, Quaternion.identity);
                Projectile projectileScript = projectile.GetComponent<Projectile>();

                Vector2 direction = (target.position - projectile.transform.position).normalized;
                    
                projectileScript.SetDirection(direction);
                if(projectileConfig) projectileScript.AssignConfig(projectileConfig);
            }
            
            yield return new WaitForSeconds(projectileAttackCooldown);
        }

        private IEnumerator AerialAttack() {
            for (int i = 0; i < aerialAttackRepeats; i++) {
                if (!target) break;

                yield return new WaitForSeconds(1f);
            }
        }
    }
}
