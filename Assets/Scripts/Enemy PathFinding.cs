using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyPathFinding : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    private Rigidbody2D rb;
    private Vector2 moveDir;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // private void FixedUpdate()
    // {
    //     rb.MovePosition(rb.position + moveDir * (moveSpeed * Time.fixedDeltaTime));
    // }
    public void MoveTo(Vector2 targetPosition)
    {

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.linearVelocity = Vector2.zero;
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
        Debug.Log("From: " + (Vector2)transform.position + " Moving to: " + targetPosition + " With velocity: " + rb.linearVelocity);

    }
    public void Stop()
    {

        rb.linearVelocity = Vector2.zero;
        // rb.bodyType = RigidbodyType2D.Kinematic;
        Debug.Log("Enemy Stopped");
    }
}
