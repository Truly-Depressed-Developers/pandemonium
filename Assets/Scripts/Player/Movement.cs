using System;
using UnityEngine;

namespace Player {
    public class Movement : MonoBehaviour {
        [SerializeField] private float moveSpeed = 2;

        public void Update() {
            OnIndicateMovement();
        }

        public void OnIndicateMovement() {

            // Get the player's Transform component
            Transform playerTransform = gameObject.transform;

            // Move the player forward
            playerTransform.Translate(Input.GetAxis("Vertical") * Vector3.up * moveSpeed * Time.deltaTime );

            // Move the player left
            playerTransform.Translate(Input.GetAxis("Horizontal") * Vector3.right * moveSpeed * Time.deltaTime);
        }
    }
}
