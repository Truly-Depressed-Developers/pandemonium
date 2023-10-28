using System;
using UnityEngine;
using UnityEngine.Events;

namespace DamageSystem {
    public class FreezeReceiver : MonoBehaviour {

        private static bool anyoneFreezed = false;

        public UnityEvent<bool> onFreezeEnd;
        public UnityEvent onFreeze;
        private bool freezed = false;
        [SerializeField] private bool freezeOnlyUnderTreshold = true;
        [SerializeField] private DamageReceiver damageReceiver;

        public Action<bool> Freeze() {
            if (!CanBeFreezed() || (freezeOnlyUnderTreshold && anyoneFreezed)) return null;
            onFreeze.Invoke();
            freezed = true;
            if (freezeOnlyUnderTreshold) {
                anyoneFreezed = true;
            }
            Debug.Log("Freezed " + gameObject.name);
            return Unfreeze;
        }

        public void Unfreeze(bool finished) {
            if (gameObject)
                Debug.Log("Unfreezed " + gameObject.name);
            
            freezed = false;
            if (freezeOnlyUnderTreshold) {
                anyoneFreezed = false;
            }
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
