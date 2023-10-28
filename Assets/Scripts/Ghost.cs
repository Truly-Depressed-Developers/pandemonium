using System;
using UnityEngine;
using System.Collections;
using DamageSystem;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
public class Ghost : Enemy {
    [Header("Movement")]
    [SerializeField] private float moveVelocityBase = 4f;
    [SerializeField] private float moveVelocityRandomness = 0.1f;
    [SerializeField] private float moveIntervalBase = 2f;
    [SerializeField] private float moveIntervalRandomness = 0.4f;
    [SerializeField] private float moveIntervalModifierTooClose = 1.4f;
    [SerializeField] private float distanceMargin = 3f;
    [SerializeField] private float movementDirectionOuterAngle = 30f;
    [SerializeField] private float movementDirectionInnerAngle = 30f;
    
    [Header("Attack")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float attackIntervalBase = 2f;
    [SerializeField] private float attackIntervalRandomness = 0.3f;
    [SerializeField] private float attackDamageBase = 10f;
    [SerializeField] private float attackDamageRandomness = 0.3f;
    [SerializeField] private float projectileSpeedBase = 6f;
    [SerializeField] private float projectileSpeedRandomness = 0.2f;
    
    private float MovementVelocity { get { return Utils.RandomizeValue(moveVelocityBase, moveVelocityRandomness); } }
    private float AttackInterval { get { return Utils.RandomizeValue(attackIntervalBase, attackIntervalRandomness); } }
    private float AttackDamage { get { return Utils.RandomizeValue(attackDamageBase, attackDamageRandomness); } }
    private float MoveInterval { get { return Utils.RandomizeValue(moveIntervalBase, moveIntervalRandomness); } }
    private float ProjectileSpeed { get { return Utils.RandomizeValue(projectileSpeedBase, projectileSpeedRandomness); } }
    
    private Transform target;
    private Animator anim;
    private FreezeReceiver freezeReceiver;
    private Rigidbody2D rb;

    protected override void Start() {
        base.Start();

        freezeReceiver = GetComponent<FreezeReceiver>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        if (!freezeReceiver || !rb || !anim) {
            throw new Exception("Required component is missing");
        }

        GameObject t = GameObject.FindGameObjectWithTag("Player");
        if (!t) t = GameObject.Find("Player");

        if (t) {
            target = t.transform;
        }
        
        StartCoroutine(Attack());
        StartCoroutine(Move());
    }

    private IEnumerator Attack() {
        while (true) {
            if (!target) break;
            
            GameObject projectile = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            Vector3 direction = (target.position - transform.position).normalized;

            Bullet bulletScript = projectile.GetComponent<Bullet>();
            bulletScript.SetSpeed(ProjectileSpeed);
            bulletScript.SetDirection(direction);

            yield return new WaitForSeconds(AttackInterval);
        }
    }

    public void OnFreezeEnd(bool finished) {
        if (!finished) return;
        
        anim.Play("Free");
    }

    public void Destroy() {
        Destroy(gameObject);
    }

    private IEnumerator Move() {
        while (true) {
            if (!target) break;

            bool isTooCloseToTarget =
                (transform.position - target.position).sqrMagnitude <= Math.Pow(distanceMargin, 2);
            
            // if (!(Vector3.Distance(transform.position, player.transform.position) > distanceMargin)) return;
        
            Vector2 movementDirection = (target.position - transform.position).normalized;
            float randomAngle = Random.Range(movementDirectionInnerAngle, movementDirectionOuterAngle) * (Random.value < 0.5f ? 1 : -1);
            movementDirection = Quaternion.AngleAxis(randomAngle, Vector3.forward) * movementDirection;

            if (isTooCloseToTarget) {
                movementDirection *= -1;
            }
            
            anim.SetFloat("X", movementDirection.x);
            anim.SetFloat("Y", movementDirection.y);

            rb.AddForce(movementDirection * MovementVelocity);

            if (isTooCloseToTarget) {
                yield return new WaitForSeconds(MoveInterval * moveIntervalModifierTooClose);
                continue;
            }
            
            yield return new WaitForSeconds(MoveInterval);
        }
    }
}
