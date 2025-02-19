using UnityEngine;

public class CriticalHeallth : MonoBehaviour
{
    private bool bossDialogueDone = false;
    private void Update()
    {
        if (bossDialogueDone) return;

        EnemyHealth enemyHealth = FindFirstObjectByType<EnemyHealth>();
        if (enemyHealth == null) return;

        if (enemyHealth.currentHealth / enemyHealth.startingHealth <= 0.2)
        {
            bossDialogueDone = true;
            PlayerData.Instance.xp += 50 * (PlanetData.Instance.planetDifficulty + 1) / 2;
            int xpReward = 50 * (PlanetData.Instance.planetDifficulty + 1) / 2;
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
            FindFirstObjectByType<BossFightManager>().StartBossDialog();
            Destroy(gameObject);
        }
    }
}
