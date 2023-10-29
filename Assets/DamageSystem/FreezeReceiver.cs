using System;
using UnityEngine;
using UnityEngine.Events;

namespace DamageSystem {
    public class FreezeReceiver : MonoBehaviour {
        public UnityEvent<bool> onFreezeEnd;
        public UnityEvent onFreeze;
        private bool freezed = false;
        [SerializeField] private bool freezeOnlyUnderTreshold = true;
        [SerializeField] private DamageReceiver damageReceiver;

        public Action<bool> Freeze() {
            if (!CanBeFreezed()) return null;
            onFreeze.Invoke();
            freezed = true;
            Debug.Log("Freezed " + gameObject.name);
            return Unfreeze;
        }

        public void Unfreeze(bool finished) {
            if (gameObject)
                Debug.Log("Unfreezed " + gameObject.name);
            
            freezed = false;
            onFreezeEnd.Invoke(finished);
        }

        private bool CanBeFreezed() {
            if (freezeOnlyUnderTreshold) {
                return !freezed && damageReceiver.IsUnderTreshold();
            } else {
                return !freezed;
            }
        }

        public bool CanMove() {
            return !freezed;
        }
    }
}
