using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    public float maxHP = 50f;
    public float currentHP;

    void Start()
    {
        currentHP = maxHP;
    }

    void Update()
    {
        
    }

    public void TakeDamage(float damageAmount)
    {
        currentHP -= damageAmount;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
