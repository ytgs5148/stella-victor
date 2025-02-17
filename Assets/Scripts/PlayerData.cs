using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance { get; private set; }
    public int xp = 0;
    public int currentHealth = 100;
    public int maxHealth = 100;
    public int armourHealth = 100;
    public int armourMaxHealth = 100;
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
        maxHealth = Mathf.Clamp(100 * ((xp / 100) + 1), 100, 10000);
    }

    public void UpdateArmourMaxHealth()
    {
        armourMaxHealth = Mathf.Clamp(100 * ((armourXP / 100) + 1), 100, 10000);
    }

    public void UnlockUniqueWeapon()
    {
        if (isBowPurchased && isLaserGunPurchased)
        {
            if (isArmourAvailable)
            {
                bowXP += 100;
                laserGunXP += 100;
                armourXP += 100;
            }
            isArmourAvailable = true;
        }
        else if (isBowPurchased && !isLaserGunPurchased)
        {
            isLaserGunPurchased = true;
            Debug.Log("Laser gun is now obtained.");
        }
        else
        {
            isBowPurchased = true;
            Debug.Log("Bow is now obtained.");
        }
    }
}
