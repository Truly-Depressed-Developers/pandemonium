using System;
using System.Collections;
using UnityEngine;

namespace DamageSystem {
    public class FreezeInvoker : MonoBehaviour {

        [SerializeField] private LayerMask freezeReceivers;
        [SerializeField] private float freezeTime;
        [SerializeField] private float successHealthBonus = 20f;
        [SerializeField] private float successStaminaBonus = 20f;
        [SerializeField] private DamageReceiver damageReceiver;
        [SerializeField] private StaminaBar staminaBar;
        private IEnumerator delayedCoroutine = null;
        private Action<bool> unFreezeOther;
        private Action<bool> unFreezeSelf;
        private FreezeReceiver lastFreezeReceiver;

        public void CancelFreeze() {
            StopCoroutine(delayedCoroutine);
            unFreezeSelf(false);
            unFreezeOther(false);
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (freezeReceivers != (freezeReceivers | 1 << other.gameObject.layer)) 
                return;
            
            if (!other.gameObject.TryGetComponent(out FreezeReceiver freezeReceiver)) 
                return;
            
            lastFreezeReceiver = freezeReceiver;
            Action<bool> tempUnFreezeOther = freezeReceiver.Freeze();
            if (tempUnFreezeOther == null)
                return;
            
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
            if (lastFreezeReceiver != null) {
                unFreezeOther(true);
            }

            damageReceiver.AddHealth(successHealthBonus);
            staminaBar.Add(successStaminaBonus);
        }
    }
}
