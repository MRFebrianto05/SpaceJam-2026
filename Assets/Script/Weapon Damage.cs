using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    public float damageValue = 15f;
    private AutoPunchWeapon autoPunchWeapon;

    void Start()
    {
        autoPunchWeapon = GetComponentInParent<AutoPunchWeapon>();
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (autoPunchWeapon != null && autoPunchWeapon.isPunching)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                EnemyHP enemyHP = other.GetComponent<EnemyHP>();

                if (enemyHP != null)
                {
                    enemyHP.TakeDamage(damageValue);
                }
            }
        }
    }
}
