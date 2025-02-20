using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance { get; private set; }
    public int xp = 0;
    public float currentHealth = 100;
    public float maxHealth = 100;
    public float armourHealth = 100;
    public float armourMaxHealth = 100;
    public int lightSaberXP = 0;
    public int bowXP = 0;
    public bool isBowPurchased = false;
    public int laserGunXP = 0;
    public bool isLaserGunPurchased = false;
    public int armourXP = 0;
    public bool isArmourAvailable = true;
    public List<string> planetsExplored;
    public float endingBar = 0f;
    public int kills = 0;
    public int totalKills = 0;
    public int lightSaberKills = 0;
    public int bowKills = 0;
    public int laserGunKills = 0;
    public Vector2Int chestPosition = Vector2Int.zero;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void UpdateMaxHealth()
    {
        maxHealth = 100 + 25 * (xp / 100);
    }

    public void UpdateArmourMaxHealth()
    {
        armourMaxHealth = 100 + 25 * (armourXP / 100);
    }

    public void UnlockUniqueWeapon()
    {
        if (isBowPurchased && isLaserGunPurchased)
        {
            if (isArmourAvailable)
            {
                armourXP += 100;
                Debug.Log("You obtained XPs");
            }
            else
            {
                isArmourAvailable = true;
                Debug.Log("You obtained Armour");
            }
        }
        else if (isBowPurchased && !isLaserGunPurchased)
        {
            isLaserGunPurchased = true;
            Debug.Log("You obtained Rifle");
            Availability.Instance.UpdateRifleAvailability();
        }
        else
        {
            isBowPurchased = true;
            Debug.Log("You obtained Bow");
            Availability.Instance.UpdateBowAvailability();
        }
    }

    public void Reset()
    {
        xp = 0;
        currentHealth = 100;
        maxHealth = 100;
        armourHealth = 100;
        armourMaxHealth = 100;
        lightSaberXP = 0;
        bowXP = 0;
        isBowPurchased = false;
        laserGunXP = 0;
        isLaserGunPurchased = false;
        armourXP = 0;
        isArmourAvailable = true;
        planetsExplored = new List<string>();
        kills = 0;
        totalKills = 0;
        lightSaberKills = 0;
        bowKills = 0;
        laserGunKills = 0;
        chestPosition = Vector2Int.zero;
    }
}
