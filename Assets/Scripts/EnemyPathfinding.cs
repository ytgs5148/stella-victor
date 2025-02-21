using UnityEngine;
public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] public float chaseRadius;
    [SerializeField] public float returnSpeed;

    private Animator animator;
    private Vector2 startPosition;
    private Transform target;
    private Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startPosition = rb.position;
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            target = player.transform;
        }
        else
        {
            Debug.LogError("Player GameObject not found in the hierarchy.");
        }
    }
    private void FixedUpdate()
    {
        if (target == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, target.position);

        if (distanceToPlayer <= chaseRadius)
        {
            MoveTowards(target.position, speed);
        }
        else
        {
            MoveTowards(startPosition, returnSpeed);
            if (Vector2.Distance(transform.position, startPosition) < 0.1f)
            {
                rb.linearVelocity = Vector2.zero;
                animator.SetBool("IsMoving", false);
            }
        }
    }
    private void MoveTowards(Vector2 destination, float moveSpeed)
    {
        Vector2 direction = (destination - (Vector2)transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
        animator.SetBool("IsMoving", true);
        float scaleX = Mathf.Abs(transform.localScale.x);
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z); // Facing Right
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(-scaleX, transform.localScale.y, transform.localScale.z); // Facing Left
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
    }
}
