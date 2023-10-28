﻿using System;
using UnityEngine;
using UnityEngine.Events;

namespace DamageSystem {
    public class FreezeReceiver : MonoBehaviour {

        private static bool anyoneFreezed = false;

        public UnityEvent<bool> onFreezeEnd;
        public UnityEvent onFreeze;
        private bool freezed = false;
        [SerializeField] private DamageReceiver damageReceiver;

        public Action<bool> Freeze() {
            if (!CanBeFreezed() || anyoneFreezed) return null;
            onFreeze.Invoke();
            freezed = true;
            anyoneFreezed = true;
            Debug.Log("Freezed");
            return Unfreeze;
        }

        public void Unfreeze(bool finished) {
            Debug.Log("Funreezed");
            freezed = false;
            anyoneFreezed = false;
            onFreezeEnd.Invoke(finished);
        }

        private bool CanBeFreezed() {
            return !freezed && damageReceiver.IsUnderTreshold();
        }

        public bool CanMove() {
            return !freezed;
        }
    }
}
