using UnityEngine;

public class Bullet : MonoBehaviour {

    private float speed = 1.0f;
    private Vector3 direction = new Vector3(1,0,0);

    private void Update() {


        transform.Translate(direction * speed * Time.deltaTime, Space.World);
        DestroyWhenOutOfScreen();
    }

    public void SetSpeed(float _speed) {
        speed = _speed;
    }

    public void SetDirection(Vector3 _direction) {
        direction = _direction;
    }

    private void DestroyWhenOutOfScreen() {
        Vector3 screenPos = Camera.main.WorldToViewportPoint(transform.position);
        if(!(screenPos.x >= 0 && screenPos.x <= 1 && screenPos.y >= 0 && screenPos.y <= 1)) {
            Destroy(gameObject);
        }
    }

}
