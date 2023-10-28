using System;
using UnityEngine;

namespace Player {
    public class Attack : MonoBehaviour {
        [SerializeField] private float attackCooldown = 2f;
        private float lastAttack = float.NegativeInfinity;
        
        private void OnTriggerEnter2D(Collider2D other) {
            // Debug.Log(other.gameObject.name);
        }

        private void OnTriggerStay(Collider other) {
            if (Time.time - lastAttack < attackCooldown) return;
            
            
        }
    }
}
