using UnityEngine;
using System.Collections;
using DamageSystem;
using DamageSystem.Health;

public class Ghost : Enemy {
    public Animator anim;

    [SerializeField]
    private GameObject bulletPrefab;
    private float moveSpeedBase = 4f;
    private float distanceMarginBase = 3f;

    private float moveSpeed = 4f;
    private float distanceMargin = 3f;

    private Vector3 target = new Vector3(0,0,0);
    public float shootInterval = 2.0f;
    private GameObject player;

    private FreezeReceiver freezeReceiver;

    protected override void Start() {
        base.Start();

        freezeReceiver = GetComponent<FreezeReceiver>();

        // InitHp(100f);

        moveSpeed = Random.Range(0.9f * moveSpeedBase, 1.1f * moveSpeedBase);
        distanceMargin = Random.Range(0.9f * distanceMarginBase, 1.1f * distanceMarginBase);

        player = GameObject.FindGameObjectWithTag("Player");
        if (!player) player = GameObject.Find("Player");
        if (player != null) {
            target = player.transform.position;
        }

        StartCoroutine(Shoot());
    }

    private void Update() {
        if (player == null) return;
        target = player.transform.position;

        if(freezeReceiver && freezeReceiver.CanMove()) {
            MoveTo(target);
        }
    }

    private void MoveTo(Vector3 moveTarget) {
        if(Vector3.Distance(transform.position, player.transform.position) > distanceMargin) {
            Vector3 dir = moveTarget - transform.position;
            anim.SetFloat("X", dir.x);
            anim.SetFloat("Y", dir.y);
            dir.Normalize();
            transform.Translate(dir * moveSpeed * Time.deltaTime, Space.World);
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
