using System;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace Morok {
    public class Movement : MonoBehaviour {
        [SerializeField] private float morokSpeed = 2f;
        
        enum Direction {
            NE, 
            NW, 
            SE, 
            SW
        };

        private Direction direction = Direction.NE;

        private void Update() {
            Move();
        }

        private void Move() {
            Vector3 vector2 = new (0,0);
            if (direction == Direction.NE) {
                vector2 = new (1,1);
            }
            else if (direction == Direction.NW) {
                vector2 = new (-1,1);
            }
            else if (direction == Direction.SE) {
                vector2 = new (1,-1);
            }
            else if (direction == Direction.SW) {
                vector2 = new (-1,-1);
            }

            transform.Translate(  Time.deltaTime * morokSpeed * vector2);
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.transform.name == "Wall1") {
                if (direction == Direction.NE) {
                    direction = Direction.SE;
                }
                else if (direction == Direction.NW) {
                    direction = Direction.SW;
                }
            }
            else if (other.transform.name == "Wall2") {
                if (direction == Direction.NW) {
                    direction = Direction.NE;
                }
                else if (direction == Direction.SW) {
                    direction = Direction.SE;
                }
            }
            else if (other.transform.name == "Wall3") {
                if (direction == Direction.SE) {
                    direction = Direction.NE;
                }
                else if (direction == Direction.SW) {
                    direction = Direction.NW;
                }
            }
            else if (other.transform.name == "Wall4") {
                if (direction == Direction.NE) {
                    direction = Direction.NW;
                }
                else if (direction == Direction.SE) {
                    direction = Direction.SW;
                }
            }
        }
    }
}
