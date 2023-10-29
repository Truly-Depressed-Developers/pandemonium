using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace DamageSystem.Weapons.MeleeWeapon {
    public class MeleeWeapon : Weapon {
        public float damage = 20f;
        [SerializeField] private UnityEvent OnAttackEnd;
        private new PolygonCollider2D collider;
        private Animator animator;
        [SerializeField] private AudioSource audio;
        [SerializeField] private Animator playerAnim;

        private Transform inner;

        public override float GetDamage() {
            return damage;
        }

        private void Awake() {
            //collider = GetComponentInChildren<PolygonCollider2D>();
            animator = GetComponent<Animator>();
            inner = transform.GetChild(0);
        }

        public override void Attack() {
            //collider.enabled = true;
            //Debug.Log("atck");
            if (!isActiveAndEnabled) return;
            if (playerAnim.GetCurrentAnimatorStateInfo(0).IsName("BasicAttackTree")) return;

            audio.Play();
            playerAnim.Play("BasicAttackTree");
            //animator.Play("Attack");
        }

        private void _OnAttackEnd() {
            OnAttackEnd.Invoke();
            //collider.enabled = false;
        }

        private void OnDrawGizmos() {
            if (!collider) return;
            Gizmos.color = collider.enabled ? Color.yellow : Color.gray;
            DrawHitbox();
        }

        private void DrawHitbox() {
            Vector3[] points = collider.points.Select(point => inner.TransformPoint(point)).ToArray();
            for (int i = 0; i < points.Length; i++) {
                Vector3 a = points[i];
                Vector3 b = points[i + 1 < points.Length ? i + 1 : 0];
                Gizmos.DrawLine(a, b);
            }
        }
    }
}
