using System;
using System.Collections;
using UnityEngine;

namespace DamageSystem {
    public class FreezeInvoker : MonoBehaviour {

        [SerializeField] private LayerMask freezeReceivers;
        [SerializeField] private float freezeTime;
        private IEnumerator delayedCoroutine = null;
        private Action<bool> unFreezeOther;
        private Action<bool> unFreezeSelf;

        public void CancelFreeze() {
            StopCoroutine(delayedCoroutine);
            unFreezeSelf(false);
            unFreezeOther(false);
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (freezeReceivers != (freezeReceivers | 1 << other.gameObject.layer)) return;
            other.gameObject.TryGetComponent(out FreezeReceiver freezeReceiver);
            if (freezeReceiver == null) return;

            Action<bool> tempUnFreezeOther = freezeReceiver.Freeze();
            if (tempUnFreezeOther == null) return;
            unFreezeOther = tempUnFreezeOther;



            Action<bool> tempUnFreezeSelf = GetComponentInParent<FreezeReceiver>().Freeze();
            Debug.Log(tempUnFreezeSelf);
            // if (tempUnFreezeSelf != null) {
            unFreezeSelf = tempUnFreezeSelf;
            // }

            delayedCoroutine = DelayedUnfreeze();
            StartCoroutine(delayedCoroutine);
        }


        private IEnumerator DelayedUnfreeze() {
            yield return new WaitForSeconds(freezeTime);
            unFreezeSelf(true);
            unFreezeOther(true);
        }
    }
}
