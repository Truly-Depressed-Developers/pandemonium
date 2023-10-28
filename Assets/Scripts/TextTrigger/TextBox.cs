using UnityEngine;

namespace TextTrigger {
    public class TextBox : MonoBehaviour {
        public Canvas canvas;
        public RectTransform rect;
        private void Awake() {
            canvas = GetComponent<Canvas>();
            rect = GetComponent<RectTransform>();
        }

        private void Update() {
            // Debug.Log(transform.TransformPoint(rect.sizeDelta));
            // Debug.Log(transform.TransformPoint(canvas.transform.position));
            // Debug.Log(canvas.worldCamera.WorldToViewportPoint(transform.TransformPoint(canvas.transform.position)));
        }
    }
}
