using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace Morok {
    public class Attack : MonoBehaviour {
        [SerializeField] private GameObject bulletPrefab;
        
        private void Start() {
            StartCoroutine(AttackCoroutine());
        }

        private IEnumerator AttackCoroutine() {
            while (true) {
                foreach (var vect2 in new List<Vector3>(){new (0, 1), new (1, 0), new (1, 1), new (0, 0)}) {
                    GameObject bullet = Instantiate(bulletPrefab, transform.position + 3 * vect2, quaternion.identity);  
                }
                yield return new WaitForSeconds(2);
            }
        }
    }
}
