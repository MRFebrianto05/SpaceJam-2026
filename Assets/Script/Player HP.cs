using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    public float maxHP = 300f;
    public float currentHP;
    public EnemyDamage enemyDamage;

    void Start()
    {
        currentHP = maxHP;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(enemyDamage.damage1);
        }
    }

    void TakeDamage(float damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        gameObject.SetActive(false);
    }
}
