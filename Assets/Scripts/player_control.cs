using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

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
        // Get movement input
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        movement = new Vector2(moveX, moveY).normalized;
        
        // Set animation parameters
        animator.SetFloat("MoveX", moveX);
        animator.SetFloat("MoveY", moveY);
        animator.SetBool("IsMoving", movement.magnitude > 0);

        // Flip sprite if moving left
        if (moveX > 0)
            spriteRenderer.flipX = false; // Face right
        else if (moveX < 0)
            spriteRenderer.flipX = true;  // Face left
    }

    void FixedUpdate()
    {
        // Move the player
        rb.linearVelocity = movement * speed;
    }
}
