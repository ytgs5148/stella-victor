using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    public float speed = 2f;
    public float chaseRadius = 5f;
    public float returnSpeed = 2f;

    private Transform target = GameObject.Find("Player").transform;
    private Vector2 startPosition;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = rb.position;
    }

    void Update()
    {
        if (target == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, target.position);

        if (distanceToPlayer <= chaseRadius)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            rb.linearVelocity = direction * speed;
        }
        else
        {
            Vector2 direction = (startPosition - (Vector2)transform.position).normalized;
            rb.linearVelocity = direction * returnSpeed;

            if (Vector2.Distance(transform.position, startPosition) < 0.1f)
            {
                rb.linearVelocity = Vector2.zero; // Stop moving when back at start
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
    }
}
