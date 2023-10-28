using System;
using System.Collections;
using UnityEngine;

namespace DamageSystem {
    public class FreezeInvoker : MonoBehaviour {

        [SerializeField] private LayerMask freezeReceivers;
        [SerializeField] private float freezeTime;
        private IEnumerator delayedCoroutine = null;
        Action<bool> unFreeze;

        public void CancelFreeze() {
            StopCoroutine(delayedCoroutine);
            unFreeze(false);
        }

        private void OnCollisionEnter2D(Collision2D other) {
            Debug.Log("1");
            if (freezeReceivers != (freezeReceivers | 1 << other.gameObject.layer)) return;
            Debug.Log("2");
            other.gameObject.TryGetComponent(out FreezeReceiver freezeReceiver);
            Debug.Log("3");
            if (!freezeReceiver) return;
            Debug.Log("4");
            Freeze(freezeReceiver);
        
            //TakeDamage(damageDealer.GetDamage());
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (GetComponent<Weapon>()) return;
            if (freezeReceivers != (freezeReceivers | 1 << other.gameObject.layer)) return;
            FreezeReceiver freezeReceiver = other.gameObject.GetComponentInParent<FreezeReceiver>();
            if (freezeReceiver == null) return;
            Freeze(freezeReceiver);

            //TakeDamage(damageDealer.GetDamage());
        }

        private void Freeze(FreezeReceiver freezeReceiver) {
            unFreeze = freezeReceiver.Freeze();
            delayedCoroutine = DelayedUnfreeze();
            StartCoroutine(delayedCoroutine);
        }

        private IEnumerator DelayedUnfreeze() {
            yield return new WaitForSeconds(freezeTime);
            unFreeze(true);
        }

    }
}
