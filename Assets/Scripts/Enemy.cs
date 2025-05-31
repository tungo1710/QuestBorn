using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float moveSpeed = 2f;
    public float patrolDistance = 3f;
    public int baseDamage = 10;

    [Header("Components")]
    [SerializeField] private GameObject attackHitbox;

    private Vector3 startPoint;
    private Vector3 targetPoint;
    private Rigidbody2D rb;
    private Animator anim;
    private AudioManager audioManager;
    private Transform playerTransform;

    private bool isAttacking = false;
    private bool isHurt = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioManager = FindAnyObjectByType<AudioManager>();

        startPoint = transform.position;
        targetPoint = startPoint + Vector3.right * patrolDistance;

        if (attackHitbox != null)
            attackHitbox.SetActive(false);
    }

    private void Update()
    {
        if (isHurt) return;

        if (isAttacking)
        {
            rb.linearVelocity = Vector2.zero;
            FacePlayer();
            return;
        }

        Patrol();
    }

    private void Patrol()
    {
        anim.SetBool("isWalk", true);
        anim.SetBool("isAtk", false);

        Vector3 direction = (targetPoint - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * moveSpeed, 0);

        // Flip hướng
        transform.localScale = new Vector3(direction.x < 0 ? -1 : 1, 1, 1);

        if (Vector2.Distance(transform.position, targetPoint) < 0.2f)
        {
            targetPoint = targetPoint == startPoint
                ? startPoint + Vector3.right * patrolDistance
                : startPoint;
        }
    }

    private void FacePlayer()
    {
        if (playerTransform == null) return;

        float dir = playerTransform.position.x - transform.position.x;
        transform.localScale = new Vector3(dir < 0 ? -1 : 1, 1, 1);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        isAttacking = true;
        rb.linearVelocity = Vector2.zero;

        anim.SetBool("isWalk", false);
        anim.SetBool("isAtk", true);

        playerTransform = other.transform;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerTransform = null;
    }

    public void EndAttack()
    {
        if (playerTransform == null)
        {
            isAttacking = false;
            anim.SetBool("isAtk", false);
            anim.SetBool("isWalk", true);
        }
        else
        {
            anim.SetBool("isAtk", true);
            anim.SetBool("isWalk", false);
        }

        DisableAttackHitbox();
    }

    public void StartHurt()
    {
        if (isHurt) return;

        isHurt = true;
        isAttacking = false;
        rb.linearVelocity = Vector2.zero;

        audioManager.PlayEnemyHurtSound();
        anim.SetTrigger("isHurt");

        Invoke(nameof(EndHurt), 0.5f);
    }

    private void EndHurt()
    {
        isHurt = false;

        bool shouldAttack = playerTransform != null;

        isAttacking = shouldAttack;
        anim.SetBool("isAtk", shouldAttack);
        anim.SetBool("isWalk", !shouldAttack);
    }

    public void EnableAttackHitbox()
    {
        if (attackHitbox == null) return;

        attackHitbox.SetActive(true);
        audioManager.PlayEnemyAttackSound();
    }

    public void DisableAttackHitbox()
    {
        if (attackHitbox != null)
            attackHitbox.SetActive(false);
    }

    public int GetDamage()
    {
        int scaled = baseDamage + GameManager.levelCount * 10;
        Debug.Log($"Enemy Damage: {scaled} (Level: {GameManager.levelCount})");
        return scaled;
    }
}
