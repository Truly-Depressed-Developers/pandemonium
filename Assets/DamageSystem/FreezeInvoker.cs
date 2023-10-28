using System;
using System.Collections;
using UnityEngine;

namespace DamageSystem {
    public class FreezeInvoker : MonoBehaviour {

        [SerializeField] private LayerMask freezeReceivers;
        [SerializeField] private float freezeTime;
        private IEnumerator delayedCoroutine = null;
        private Action<bool> unFreeze;

        public void CancelFreeze() {
            StopCoroutine(delayedCoroutine);
            unFreeze(false);
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (freezeReceivers != (freezeReceivers | 1 << other.gameObject.layer)) return;
            other.gameObject.TryGetComponent(out FreezeReceiver freezeReceiver);
            if (!freezeReceiver) return;
            Freeze(freezeReceiver);
        }

        private void Freeze(FreezeReceiver freezeReceiver) {
            Action<bool> tempUnFreeze = freezeReceiver.Freeze();
            if (tempUnFreeze == null) return;
            unFreeze = tempUnFreeze;
            delayedCoroutine = DelayedUnfreeze();
            StartCoroutine(delayedCoroutine);
        }

        private IEnumerator DelayedUnfreeze() {
            yield return new WaitForSeconds(freezeTime);
            unFreeze(true);
        }
    }
}
