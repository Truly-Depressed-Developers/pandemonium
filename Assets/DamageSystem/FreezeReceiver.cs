using System;
using UnityEngine;
using UnityEngine.Events;

namespace DamageSystem {
    public class FreezeReceiver : MonoBehaviour {

        public UnityEvent<bool> onFreezeEnd;
        public UnityEvent onFreeze;
        
        public Action<bool> Freeze() {
            onFreeze.Invoke();
            Debug.Log("Freezed");
            return Unfreeze;
        }

        public void Unfreeze(bool finished) {
            Debug.Log("Funreezed");

            onFreezeEnd.Invoke(finished);
        }
    }
}
