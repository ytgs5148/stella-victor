using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 100;
    [SerializeField] private float knockBackThrust = 5f;
    private Animator animator;
    private EnemyAI enemyAI;
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
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("Hurt");
        knockBack.GetKnockedBack(PlayerController.Instance.transform, knockBackThrust);
        DetectDeath();
        // StartCoroutine(flash.FlashRoutine());
        StartCoroutine(ResetHurtTrigger());
    }
    private IEnumerator ResetHurtTrigger()
    {
        yield return new WaitForSeconds(0.3f);
        animator.ResetTrigger("Hurt");
    }
    public void DetectDeath()
    {
        if (currentHealth <= 0)
        {
            animator.SetTrigger("Death");
            StartCoroutine(DeathAnimation());
        }
    }
    private IEnumerator DeathAnimation(){
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;

        yield return new WaitForSeconds(0.3f);

        Destroy(gameObject);
    }
}
