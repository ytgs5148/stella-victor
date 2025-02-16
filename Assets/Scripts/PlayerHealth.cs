using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float knockBackThrustAmount = 5f;
    [SerializeField] private float damageRecoveryTime = 1f;
    private int currentHealth;
    private bool canTakeDamage = true;
    private KnockBack knockBack;
    private Flash flash;
    private HealthManager HealthManager;
    private void Awake()
    {
        flash = GetComponent<Flash>();
        knockBack = GetComponent<KnockBack>();
    }
    private void Start()
    {
        currentHealth = maxHealth;
        HealthManager = FindAnyObjectByType<HealthManager>();
        HealthManager.UpdateHealthBar(currentHealth, maxHealth);
    }
    public void TakeDamage(int damageAmount, Transform enemyPosition)
    {
        if (!canTakeDamage) return;
        canTakeDamage = false;
        currentHealth -= damageAmount;
        HealthManager.UpdateHealthBar(currentHealth, maxHealth);
        if (knockBack != null)
        {
            Debug.Log("KnockBack");
            knockBack.GetKnockedBack(enemyPosition, knockBackThrustAmount);
        }
        if (flash != null)
        {
            StartCoroutine(flash.FlashRoutine());
        }
        StartCoroutine(DamageRecoveryRoutine());
        Debug.Log("Current Health = " + currentHealth);
    }
    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }
}