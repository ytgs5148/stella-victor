using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 10f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Ensure Rigidbody2D has proper settings
        rb.gravityScale = 0;  // No gravity in top-down movement
        rb.freezeRotation = true; // Prevent rotation
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal"); // A (-1) & D (+1)
        float moveY = Input.GetAxisRaw("Vertical");   // W (+1) & S (-1)
        movement = new Vector2(moveX, moveY).normalized;

        // Flip sprite based on movement direction
        if (moveX > 0)
            spriteRenderer.flipX = false;
        else if (moveX < 0)
            spriteRenderer.flipX = true;

        animator.SetBool("IsMoving", movement.magnitude > 0);

        // Check for attacks
        if (Input.GetMouseButtonDown(0)) // Left Click for Melee Attack
        {
            MeleeAttack();
        }
        if (Input.GetMouseButtonDown(1)) // Right Click for Ranged Attack
        {
            RangedAttack();
        }
    }


    void FixedUpdate()
    {
        // Move the player
        rb.linearVelocity = movement * speed;
    }

    void MeleeAttack()
    {
        animator.SetTrigger("Attack"); // Play attack animation

        // Detect enemies in range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Damage enemies
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(25); // Call enemy damage function
        }
    }

    void RangedAttack()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(spriteRenderer.flipX ? -projectileSpeed : projectileSpeed, 0);
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
