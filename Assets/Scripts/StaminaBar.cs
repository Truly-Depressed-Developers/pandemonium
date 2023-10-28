using System;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour {
    [SerializeField] private Slider slider;
    [SerializeField] private float regeneration = 10f;

    public bool TryUse(float value) {
        if (slider.value < value) return false;
        slider.value -= value;
        return true;

    }

    public void SetMaxValue(float value) {
        slider.maxValue = value;
        slider.value = value;
    }

    private void Update() {
        slider.value += regeneration * Time.deltaTime;
    }
}
