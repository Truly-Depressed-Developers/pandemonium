using UnityEngine;
using UnityEngine.UI;

namespace DamageSystem.Health {
    public class HealthBar : MonoBehaviour {
        [SerializeField] private Slider slider;

        public void SetHealth(float health) {
            slider.value = health;
        }

        public void SetMaxHealth(float health) {
            slider.maxValue = health;
            slider.value = health;
        }
    }
}
