using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Availability : MonoBehaviour
{
    public static Availability Instance { get; private set; }
    [SerializeField] public GameObject bow;
    [SerializeField] public GameObject rifle;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        UpdateBowAvailability();
        UpdateRifleAvailability();
    }
    public void UpdateBowAvailability()
    {
        if (PlayerData.Instance.isBowPurchased)
        {
            bow.SetActive(true);
            Debug.Log("BowIcon Visible");
        }
    }
    public void UpdateRifleAvailability()
    {
        if (PlayerData.Instance.isLaserGunPurchased)
        {
            rifle.SetActive(true);
            Debug.Log("RifleIcon Visible");
        }
    }
}
