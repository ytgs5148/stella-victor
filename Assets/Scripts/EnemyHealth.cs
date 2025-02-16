using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth;
    [SerializeField] private float knockBackThrust;
    public EnemyHealthManager healthBar;
    private Animator animator;
    public int currentHealth;
    private KnockBack knockBack;
    private Flash flash;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        flash = GetComponent<Flash>();
        knockBack = GetComponent<KnockBack>();
    }
    private void Start()
    {
        currentHealth = startingHealth;
        healthBar.SetHealth(currentHealth, startingHealth);
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth, startingHealth);
        // Debug.Log("Enemy took damage: " + damage + ", current health: " + currentHealth);
        // Debug.Log("KnockBack");
        knockBack.GetKnockedBack(PlayerController.Instance.transform, knockBackThrust);
        // Debug.Log("Knockback called on enemy with thrust: " + knockBackThrust);
        StartCoroutine(flash.FlashRoutine());
        DetectDeath();
        animator.SetTrigger("Hurt");
        StartCoroutine(ResetHurtBool());
    }
    private IEnumerator ResetHurtBool()
    {
        yield return new WaitForSeconds(0.3f);
        animator.ResetTrigger("Hurt");
        animator.SetTrigger("Idle");
    }
    public void DetectDeath()
    {
        if (currentHealth <= 0)
        {
            animator.SetTrigger("Death");
            StartCoroutine(DeathAnimation());
        }
    }
    private IEnumerator DeathAnimation()
    {
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;

        yield return new WaitForSeconds(0.3f);

        Destroy(gameObject);
    }
}
