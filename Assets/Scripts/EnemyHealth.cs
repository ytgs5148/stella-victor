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
        Debug.Log("Current health" + currentHealth);
        healthBar.SetHealth(currentHealth, startingHealth);
        knockBack.GetKnockedBack(PlayerController.Instance.transform, knockBackThrust);
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
        PlayerData.Instance.kills++;
        PlayerData.Instance.xp += 100;
        Destroy(gameObject);
    }
}
