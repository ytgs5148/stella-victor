using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Transform target;
    [SerializeField] public float chaseRadius = 5f;
    [SerializeField] public float attackRadius = 1f;
    [SerializeField] public float speed = 2f;
    [SerializeField] public float attackCooldown = 1f;
    [SerializeField] public int attackDamage = 10;

    private Rigidbody2D rb;
    private Animator animator;
    private bool canAttack = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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

    void Update()
    {
        if (target == null) return;
        
        float distanceToPlayer = Vector2.Distance(transform.position, target.position);

        if (distanceToPlayer <= attackRadius)
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("IsMoving", false);

            if (canAttack)
            {
                Attack();
            }
        }
        else if (distanceToPlayer <= chaseRadius)
        {
            ChasePlayer();
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("IsMoving", false);
        }
    }

    void ChasePlayer()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;
        animator.SetBool("IsMoving", true);

    }

    void Attack()
    {
        canAttack = false;
        animator.SetBool("attack", true);
        Debug.Log("Enemy Attacked!");

        PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
        }

        StartCoroutine(ResetAttack());
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(0.4f);
        canAttack = true;
        animator.SetBool("attack", false);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
    }
}
