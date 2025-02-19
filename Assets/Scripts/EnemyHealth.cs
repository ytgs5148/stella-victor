using System.Collections;
using UnityEngine;
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] public float startingHealth;
    [SerializeField] private float knockBackThrust;
    public EnemyHealthManager healthBar;
    private Animator animator;
    public float currentHealth;
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
        startingHealth = startingHealth * (PlanetData.Instance.planetDifficulty + 1) / 2;
        currentHealth = startingHealth;
        healthBar.SetHealth(currentHealth, startingHealth);
    }
    
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth, startingHealth);
        knockBack.GetKnockedBack(PlayerController.Instance.transform, knockBackThrust);
        StartCoroutine(flash.FlashRoutine());
        DetectDeath();
        if (currentHealth > 0)
        {
            animator.SetTrigger("Hurt");
            StartCoroutine(ResetHurt());
        }
    }
    private IEnumerator ResetHurt()
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
        this.enabled = false;

        PlayerData.Instance.kills++;
        PlayerData.Instance.totalKills++;
        PlayerData.Instance.xp += 10 * (PlanetData.Instance.planetDifficulty + 1) / 2;

        int xpReward = 10 * (PlanetData.Instance.planetDifficulty + 1) / 2;

        if (ActiveWeapon.Instance != null && ActiveWeapon.Instance.CurrentActiveWeapon != null)
        {
            MonoBehaviour currentWeapon = ActiveWeapon.Instance.CurrentActiveWeapon;
            if (currentWeapon is Lightsaber)
            {
                PlayerData.Instance.lightSaberXP += xpReward;
                PlayerData.Instance.lightSaberKills++;
                Debug.Log("Added " + xpReward + " XP to Lightsaber. Total: " + PlayerData.Instance.lightSaberXP);
            }
            else if (currentWeapon is Bow)
            {
                PlayerData.Instance.bowXP += xpReward;
                PlayerData.Instance.bowKills++;
                Debug.Log("Added " + xpReward + " XP to Bow. Total: " + PlayerData.Instance.bowXP);
            }
            else if (currentWeapon is Rifle)
            {
                PlayerData.Instance.laserGunXP += xpReward;
                PlayerData.Instance.laserGunKills++;
                Debug.Log("Added " + xpReward + " XP to Rifle. Total: " + PlayerData.Instance.laserGunXP);
            }
            else
            {
                PlayerData.Instance.xp += xpReward;
                Debug.Log("Added " + xpReward + " XP to general XP. Total: " + PlayerData.Instance.xp);
            }
        }
        else
        {
            PlayerData.Instance.xp += xpReward;
            Debug.Log("No active weapon found. Added " + xpReward + " XP to general XP. Total: " + PlayerData.Instance.xp);
        }
        if (PlayerData.Instance.isArmourAvailable)
        {
            PlayerData.Instance.armourXP += 10 * (PlanetData.Instance.planetDifficulty + 1) / 2;
        }
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }
}