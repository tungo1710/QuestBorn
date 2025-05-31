using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private bool isPlayer = false;

    [Header("UI")]
    [SerializeField] private Image healthBarFill;

    private int currentHealth;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    private void Start()
    {
        animator = GetComponent<Animator>();

        if (isPlayer)
        {
            maxHealth = PlayerData.maxHealth;
            currentHealth = PlayerData.health;
            spriteRenderer = GetComponent<SpriteRenderer>();
            originalColor = spriteRenderer.color;
        }
        else
        {
            maxHealth += GameManager.levelCount * 10;
            currentHealth = maxHealth;
            spriteRenderer = GetComponent<SpriteRenderer>();
            originalColor = spriteRenderer.color;

        }

        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        PlayerData.health = currentHealth;

        Debug.Log($"{gameObject.name} took {damage} damage. Current HP: {currentHealth}");

        animator?.SetTrigger("isHurt");

        spriteRenderer.color = Color.red; 

        Invoke(nameof(RestoreColor), 0.2f);

        UpdateHealthUI();

        if (currentHealth <= 0)
            Die();
    }

    private void RestoreColor()
    {
        spriteRenderer.color = originalColor;  
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        PlayerData.health = currentHealth;

        UpdateHealthUI();
        Debug.Log($"Healed. Current HP: {currentHealth}");
    }

    private void UpdateHealthUI()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;
        }
    }

    private void Die()
    {
        animator?.SetTrigger("isDie");

        if (isPlayer)
        {
            HandlePlayerDeath();
        }
        else
        {
            HandleEnemyDeath();
        }
    }

    private void HandlePlayerDeath()
    {
        Debug.Log("Player Died");

        // Disable control
        var pc = GetComponent<PlayerController>();
        if (pc != null) pc.enabled = false;

        // Stop physics
        var rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        // Disable attack hitbox
        var attackHitbox = transform.Find("AttackHitbox");
        if (attackHitbox != null)
            attackHitbox.gameObject.SetActive(false);

        // Ungroup tag and trigger game over
        tag = "Untagged";

        var gameManager = FindAnyObjectByType<GameManager>();
        gameManager?.ShowGameOver();

        Destroy(gameObject, 1.2f);
    }

    private void HandleEnemyDeath()
    {
        Debug.Log("Enemy Died");

        var enemy = GetComponent<Enemy>();
        if (enemy != null) enemy.enabled = false;

        var rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        var attackHitbox = transform.Find("AttackHitbox");
        if (attackHitbox != null)
            attackHitbox.gameObject.SetActive(false);

        tag = "Untagged";

        var col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Destroy(gameObject, 1.2f);
    }
}
