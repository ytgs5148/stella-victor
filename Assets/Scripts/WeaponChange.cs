using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponChange : MonoBehaviour
{
    public static WeaponChange Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI weaponInfoText;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void UpdateWeaponInfo()
    {
        StopAllCoroutines();
        weaponInfoText.enabled = false;
        if (ActiveWeapon.Instance != null && ActiveWeapon.Instance.CurrentActiveWeapon != null)
        {
            MonoBehaviour currentWeapon = ActiveWeapon.Instance.CurrentActiveWeapon;
            string weaponName = "Unknown";
            int xp = 0;
            if (currentWeapon is Lightsaber)
            {
                weaponName = "Lightsaber";
                xp = PlayerData.Instance.lightSaberXP;
            }
            else if (currentWeapon is Bow)
            {
                weaponName = "Bow";
                xp = PlayerData.Instance.bowXP;
            }
            else if (currentWeapon is Rifle)
            {
                weaponName = "Rifle";
                xp = PlayerData.Instance.laserGunXP;
            }
            int level = xp / 100 + 1;
            weaponInfoText.enabled = true;
            weaponInfoText.text = $"{weaponName}  Level: {level}";
            StartCoroutine(EndText());
        }
    }
    public IEnumerator EndText() {
        yield return new WaitForSeconds(1.5f);
        weaponInfoText.enabled = false;
    }
}