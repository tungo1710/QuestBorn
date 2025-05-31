using UnityEngine;

public class EnemyAttackHitbox : MonoBehaviour
{
    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Health playerHealth = other.GetComponent<Health>();
            Enemy enemyScript = GetComponentInParent<Enemy>(); 
            if (playerHealth != null && enemyScript != null)
            {
                audioManager.PlayPlayerHurtSound();
                int damage = enemyScript.GetDamage(); 
                playerHealth.TakeDamage(damage);
            }
        }
    }
}
