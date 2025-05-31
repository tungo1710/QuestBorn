using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 15f;

    [Header("Ground Check Settings")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Attack Settings")]
    [SerializeField] private GameObject attackHitbox;
    [SerializeField] private float stepInterval = 0.4f;
    [SerializeField] private int baseDamage = 10;

    private Rigidbody2D rb;
    private Animator animator;
    private AudioManager audioManager;

    private bool isGrounded;
    private bool isAttacking = false;
    private float nextStepTime = 0f;

    public int GetDamage()
    {
        return baseDamage;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        audioManager = FindAnyObjectByType<AudioManager>();
        if (attackHitbox != null)
            attackHitbox.SetActive(false); 
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;
        HandleAttack(); 

        if (!isAttacking)
        {
            HandleMovement();
            HandleJump();
        }

        UpdateAnimation();
    }

    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        Vector2 velocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        rb.linearVelocity = velocity;

        if (moveInput > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        if (isGrounded && Mathf.Abs(moveInput) > 0.1f && Time.time >= nextStepTime)
        {
            audioManager.PlayPlayerWalkSound();
            nextStepTime = Time.time + stepInterval;
        }
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            audioManager.PlayJumpSound();
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void HandleAttack()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (Input.GetMouseButtonDown(0) && !isAttacking && isGrounded) 
        {
            StartAttack("isAtk");
        }
        else if (Input.GetMouseButtonDown(1) && !isAttacking && isGrounded) 
        {
            StartAttack("isAtk2");
        }
    }

    private void StartAttack(string triggerName)
    {
        isAttacking = true;
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        animator.SetTrigger(triggerName);
        audioManager.PlayPlayerAttackSound();
    }

    private void UpdateAnimation()
    {
        float velocityY = rb.linearVelocity.y;
        bool isRunning = Mathf.Abs(rb.linearVelocity.x) > 0.1f && !isAttacking;
        bool isFalling = !isGrounded && velocityY < -0.1f;
        bool isJumping = !isGrounded && velocityY > 0.1f;

        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isFalling", isFalling);
    }

    public void EnableAttackHitbox()
    {
        if (attackHitbox != null)
            attackHitbox.SetActive(true);
    }

    public void DisableAttackHitbox()
    {
        if (attackHitbox != null)
            attackHitbox.SetActive(false);
    }

    public void EndAttack()
    {
        isAttacking = false;
        DisableAttackHitbox();
    }
}
