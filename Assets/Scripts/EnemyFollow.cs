using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    public Transform target;
    public float speed = 2f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (target == null) return;

        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;
    }
}
