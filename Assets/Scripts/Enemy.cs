using DamageSystem;
using DamageSystem.Health;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {
    // private float maxHp;
    // private float hp;


    private DamageReceiver damageReceiver;

    // protected HealthBar healthBar;

    protected virtual void Start() {
        damageReceiver = GetComponent<DamageReceiver>();
        // healthBar = transform.Find("Canvas").transform.Find("HealthBar").GetComponent<HealthBar>();
        // Debug.Log(healthBar);
    }

    // protected void InitHp(float _hp) {
        // hp = _hp;
        // maxHp = _hp;
        // healthBar.SetHealth(hp);
        // healthBar.SetMaxHealth(maxHp);
    // }

    


    public void Kill() {
        Destroy(gameObject);
    }
}
