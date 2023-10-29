using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DamageSystem {
    public class FreezeInvoker : MonoBehaviour {

        [SerializeField] private LayerMask freezeReceivers;
        [SerializeField] private float freezeTime;
        [SerializeField] private float successHealthBonus = 20f;
        [SerializeField] private float successStaminaBonus = 20f;
        [SerializeField] private DamageReceiver damageReceiver;
        [SerializeField] private StaminaBar staminaBar;
        private Coroutine delayedCoroutine;
        private List<Action<bool>> unFreezeOtherList = new();
        private Action<bool> unFreezeSelf;

        public void CancelFreeze() {
            if (unFreezeOtherList.Count > 0) {
                StopCoroutine(delayedCoroutine);
                delayedCoroutine = null;
            }
            Unfreeze(false);
        }

        private void OnTriggerEnter2D(Collider2D other) {
            // Check freezing mask
            if (freezeReceivers != (freezeReceivers | 1 << other.gameObject.layer)) 
                return;
            
            // Try get FreezeReceiver
            if (!other.gameObject.TryGetComponent(out FreezeReceiver freezeReceiver)) 
                return;
            
            // Try to freeze target
            Action<bool> tempUnFreezeOther = freezeReceiver.Freeze();
            if (tempUnFreezeOther == null) 
                return;

            // Freeze self & run coroutine only once per attack
            if (unFreezeOtherList.Count == 0) {
                unFreezeSelf = GetComponentInParent<FreezeReceiver>().Freeze();
                delayedCoroutine = StartCoroutine(DelayedUnfreeze());
            }
            
            // Add target unfreeze function to list
            unFreezeOtherList.Add(tempUnFreezeOther);
        }


        private IEnumerator DelayedUnfreeze() {
            yield return new WaitForSeconds(freezeTime);
            Unfreeze(true);

            damageReceiver.AddHealth(successHealthBonus);
            staminaBar.Add(successStaminaBonus);

            delayedCoroutine = null;
        }

        private void Unfreeze(bool v) {
            if (unFreezeOtherList.Count > 0) {
                unFreezeSelf(v);
                unFreezeSelf = null;
            }
            
            foreach (Action<bool> action in unFreezeOtherList)
                action(v);
            unFreezeOtherList.Clear();
        }
    }
}
