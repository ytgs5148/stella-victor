using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Transform target;
    [SerializeField] public float attackRadius;
    [SerializeField] public float attackCooldown;
    [SerializeField] public float attackDamage;
    private Rigidbody2D rb;
    private Animator animator;
    private bool canAttack = true;

    private void Start()
    {
        attackDamage = attackDamage * (PlanetData.Instance.planetDifficulty + 1) / 2;
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

    private void Update()
    {
        if (target == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, target.position);

        if (distanceToPlayer <= attackRadius && canAttack)
        {
            Attack();
        }
    }
    private void Attack()
    {
        if (!canAttack) return;

        canAttack = false;
        rb.linearVelocity = Vector2.zero;
        animator.SetBool("IsMoving", false);
        animator.SetTrigger("Attack");

        FindFirstObjectByType<AudioManager>().Play("Goblin Hit");

        PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage, transform);
        }
        StartCoroutine(ResetAttack());
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
        animator.SetTrigger("Idle");
        animator.ResetTrigger("Attack");
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}