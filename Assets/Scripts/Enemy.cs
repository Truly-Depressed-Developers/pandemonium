using DamageSystem.Health;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {
    private float maxHp;
    private float hp;

    [SerializeField]
    private float treshold = 0.2f;
    [SerializeField]
    private float finalDmgReduction = 0.9f;
    private float actualDmgReduction = 0f;

    protected HealthBar healthBar;

    protected virtual void Start() {
        healthBar = transform.Find("Canvas").transform.Find("HealthBar").GetComponent<HealthBar>();
        Debug.Log(healthBar);
    }

    protected void InitHp(float _hp) {
        hp = _hp;
        maxHp = _hp;
        healthBar.SetHealth(hp);
        healthBar.SetMaxHealth(maxHp);
    }

    public bool IsUnderTreshold() {
        return hp < maxHp * treshold;
    }

    public void Hit(float dmg) {
        hp -= dmg * (1f - actualDmgReduction);
        if (hp <= 0f) {
            hp = 0f;
            Kill();
        }  

        healthBar.SetHealth(hp);

        actualDmgReduction = IsUnderTreshold() ? finalDmgReduction : 0f;
    }

    public void Kill() {
        Destroy(gameObject);
    }
}
