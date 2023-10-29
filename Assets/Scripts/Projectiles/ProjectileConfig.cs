using System;
using UnityEngine;

namespace Projectiles {
    public enum SpeedChangeMode {
        Accelerate,
        Decelerate
    }
    
    [CreateAssetMenu(menuName = "TDD/Projectile", order = 0)]
    public class ProjectileConfig : ScriptableObject {
        [field: SerializeField] public float initialSpeed = 5f;
        [field: SerializeField] public SpeedChange changeSettings = new();
        [field: SerializeField] public float lifetime = 5f;

        [Serializable]
        public class SpeedChange {
            [field: SerializeField] public SpeedChangeMode mode = SpeedChangeMode.Accelerate;
            [field: SerializeField] public float value = 4f;
            [field: SerializeField] public float threshold = 9f;
        }
    }
}
