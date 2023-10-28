using System;
using UnityEngine;

namespace Morok.Bullet {
    public class Movement : MonoBehaviour {
        [SerializeField] private float speed = 5f;
        [SerializeField] private float lifeTime = 3f;
        
        private Player.Movement movementPlayer;
        private float lastTime;
        private Vector2 direction;

        private void Start() {
            movementPlayer = GameObject.Find("Player").GetComponent<Player.Movement>();
            direction = (movementPlayer.transform.position - transform.position).normalized;
            Debug.Log(movementPlayer);
            lastTime = Time.time;
        }

        private void Update() {
            transform.Translate( speed * Time.deltaTime * direction);

            if (Time.time - lastTime > lifeTime) {
                Destroy(gameObject);
            }
        }
    }
}
