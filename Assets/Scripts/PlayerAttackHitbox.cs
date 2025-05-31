using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{
    public int damage = 20;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemyScript = other.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.StartHurt();
            }

            Health target = other.GetComponent<Health>();
            if (target != null)
            {
                target.TakeDamage(damage); 
            }
        }
    }
}
