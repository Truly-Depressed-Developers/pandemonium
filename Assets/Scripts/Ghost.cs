using UnityEngine;
using System.Collections;
using DamageSystem.Health;

public class Ghost : Enemy {

    [SerializeField]
    private GameObject bulletPrefab;

    private Vector3 target;
    public float shootInterval = 2.0f;
    protected override void Start() {
        base.Start();

        InitHp(100f);

        target = new Vector3(0, 0, 0);
        StartCoroutine(Shoot());
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.A)) {
            Hit(5f);
        }
    }

    private IEnumerator Shoot() {
        while (true) {

            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            Vector3 direction = target - transform.position;
            direction.Normalize();

            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.SetSpeed(10);
            bulletScript.SetDirection(direction);

            yield return new WaitForSeconds(shootInterval);
        }
    }

}
