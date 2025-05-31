using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private GameManager gameManager;
    private Health playerHealth;
    private AudioManager audioManager;

    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        playerHealth = GetComponent<Health>();
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Trap"))
        {
            Debug.Log("Player trúng bẫy");

            Health health = GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(health.MaxHealth); 
            }
        }
        else if (collision.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
            audioManager.PlayCoinSound();
            gameManager.AddScore(1);
        }
        else if (collision.CompareTag("Heart"))
        {
            if (playerHealth != null)
            {
                if (playerHealth.CurrentHealth < playerHealth.MaxHealth)
                {
                    audioManager.PlayHealthSound();
                    playerHealth.Heal(20);
                    Destroy(collision.gameObject);
                }
                else
                {
                    Debug.Log("Máu đã đầy, không thể ăn trái tim.");
                }
            }
        }
    }



}
