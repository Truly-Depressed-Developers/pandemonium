using System;
using UnityEngine;
using UnityEngine.Events;

namespace DamageSystem {
    public class FreezeReceiver : MonoBehaviour {

        public UnityEvent<bool> onFreezeEnd;
        public UnityEvent onFreeze;
        private bool freezed = false;
        [SerializeField] private DamageReceiver damageReceiver;

        public Action<bool> Freeze() {
            if (!CanBeFreezed()) return null;
            onFreeze.Invoke();
            freezed = true;
            Debug.Log("Freezed");
            return Unfreeze;
        }

        public void Unfreeze(bool finished) {
            Debug.Log("Funreezed");
            freezed = false;
            onFreezeEnd.Invoke(finished);
        }

        private bool CanBeFreezed() {
            return !freezed && damageReceiver.IsUnderTreshold();
        }
    }
}
