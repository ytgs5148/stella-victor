using System.Collections;
using UnityEngine;
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float knockBackThrustAmount = 5f;
    [SerializeField] private float damageRecoveryTime = 1f;
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
        PlayerData.Instance.currentHealth = PlayerData.Instance.maxHealth;
        HealthManager = FindAnyObjectByType<HealthManager>();
        HealthManager.UpdateHealthBar(PlayerData.Instance.currentHealth, PlayerData.Instance.maxHealth);
        Debug.Log("Armor Available? => " + PlayerData.Instance.isArmourAvailable);
        HealthManager.UpdateVisibility(PlayerData.Instance.isArmourAvailable);
        if (PlayerData.Instance.isArmourAvailable)
        {
            PlayerData.Instance.armourHealth = PlayerData.Instance.armourMaxHealth;
            HealthManager.UpdateArmourBar(PlayerData.Instance.armourHealth, PlayerData.Instance.armourMaxHealth);
        }
    }
    public void TakeDamage(float damageAmount, Transform enemyPosition)
    {
        if (!canTakeDamage) return;
        canTakeDamage = false;
        if (PlayerData.Instance.isArmourAvailable)
        {
            PlayerData.Instance.armourHealth -= damageAmount;
            if (PlayerData.Instance.armourHealth <= 0)
            {
                PlayerData.Instance.armourHealth = 0;
                PlayerData.Instance.isArmourAvailable = false;
                HealthManager.UpdateVisibility(PlayerData.Instance.isArmourAvailable);
            }
            HealthManager.UpdateArmourBar(PlayerData.Instance.armourHealth, PlayerData.Instance.armourMaxHealth);
        }
        else
        {
            PlayerData.Instance.currentHealth -= damageAmount;
        }
        HealthManager.UpdateHealthBar(PlayerData.Instance.currentHealth, PlayerData.Instance.maxHealth);
        if (knockBack != null)
        {
            knockBack.GetKnockedBack(enemyPosition, knockBackThrustAmount);
        }
        if (flash != null)
        {
            StartCoroutine(flash.FlashRoutine());
        }
        if (PlayerData.Instance.currentHealth <= 0) {
            PlayerData.Instance.planetsExplored.Remove(PlanetData.Instance.planetName);
            PlayerData.Instance.xp /= 2;
            Destroy(gameObject);
            FindFirstObjectByType<LoadingScreenManager>().LoadScene(1);
            return;
        }
        StartCoroutine(DamageRecoveryRoutine());
        Debug.Log($"Player Health: {PlayerData.Instance.currentHealth}, Armor: {PlayerData.Instance.armourHealth}");
    }
    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }
}