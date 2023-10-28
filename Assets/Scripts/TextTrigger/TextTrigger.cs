using System;
using UnityEngine;

namespace TextTrigger {
    public class TextTrigger : MonoBehaviour {
        [SerializeField] private TextBox textBox;

        [Tooltip("Half player width + spacing")] [SerializeField]
        private float textBoxPlayerSpacing = 2f;

        private Vector3[] triggerCorners;

        private void OnTriggerEnter2D(Collider2D other) {
            if (textBox.gameObject.activeSelf) return;
            textBox.gameObject.SetActive(true);
            Vector3 otherPos = other.transform.position;
            int x = -Math.Sign(otherPos.x);
            float textWidth = textBox.rect.sizeDelta.x * textBox.canvas.gameObject.transform.localScale.x;
            textBox.gameObject.transform.position = new Vector3(
                otherPos.x + x * (textBoxPlayerSpacing + textWidth / 2),
                otherPos.y,
                0
            );
        }

        private void OnDrawGizmos() {
            var bounds = GetComponent<BoxCollider2D>().bounds;
            var center = bounds.center;
            var halfSize = bounds.size / 2;
            triggerCorners = new Vector3[] {
                new(center.x + halfSize.x, center.y + halfSize.y, 0),
                new(center.x + halfSize.x, center.y - halfSize.y, 0),
                new(center.x + halfSize.x, center.y - halfSize.y, 0),
                new(center.x - halfSize.x, center.y - halfSize.y, 0),
                new(center.x - halfSize.x, center.y - halfSize.y, 0),
                new(center.x - halfSize.x, center.y + halfSize.y, 0),
                new(center.x - halfSize.x, center.y + halfSize.y, 0),
                new(center.x + halfSize.x, center.y + halfSize.y, 0),
            };
            Gizmos.color = Color.green;
            Gizmos.DrawLineList(triggerCorners);
        }
    }
}
