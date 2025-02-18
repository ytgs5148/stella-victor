using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActiveInventory : MonoBehaviour
{
    private int activeSlotIndexNum = 0;
    private PlayerControl playerControl;
    private void Awake()
    {
        playerControl = new PlayerControl();

    }
    private void Start()
    {
        playerControl.Inventory.Keyboard.performed += ctx => ToggleActiveSlot((int)ctx.ReadValue<float>());
        ChangeActiveWeapon();
        Availability.Instance.UpdateRifleAvailability();
        Availability.Instance.UpdateBowAvailability();
    }
    private void OnEnable()
    {
        playerControl.Enable();
    }
    private void OnDisable()
    {
        playerControl.Disable();
    }
    private void ToggleActiveSlot(int numValue)
    {
        int newSlotIndex = numValue - 1;
        if (newSlotIndex >= transform.childCount)
        {
            Debug.Log("Slot " + newSlotIndex + " does not exist.");
            Debug.Log("Total Weapon count = " + transform.childCount);
            return;
        }
        if (newSlotIndex == 1 && !PlayerData.Instance.isBowPurchased)
        {
            Debug.Log("Bow is not purchased. Staying with current weapon.");
            return;
        }
        if (newSlotIndex == 2 && !PlayerData.Instance.isLaserGunPurchased)
        {
            Debug.Log("Rifle is not purchased. Staying with current weapon.");
            return;
        }
        if (newSlotIndex == activeSlotIndexNum)
        {
            return;
        }
        ToggleActiveHighlight(numValue - 1);
    }
    private void ToggleActiveHighlight(int indexNum)
    {
        activeSlotIndexNum = indexNum;
        foreach (Transform inventorySlot in this.transform)
        {
            inventorySlot.GetChild(0).gameObject.SetActive(false);
        }
        this.transform.GetChild(indexNum).GetChild(0).gameObject.SetActive(true);
        ChangeActiveWeapon();
    }
    private void ChangeActiveWeapon()
    {
        if (ActiveWeapon.Instance.CurrentActiveWeapon != null)
        {
            Destroy(ActiveWeapon.Instance.CurrentActiveWeapon.gameObject);
        }
        if (!transform.GetChild(activeSlotIndexNum).GetComponentInChildren<InventorySlot>())
        {
            ActiveWeapon.Instance.WeaponNull();
            return;
        }
        GameObject weaponToSpawn = transform.GetChild(activeSlotIndexNum).GetComponentInChildren<InventorySlot>().GetWeaponInfo().weaponPrefab;
        GameObject newWeapon = Instantiate(weaponToSpawn, ActiveWeapon.Instance.transform.position, Quaternion.identity);
        ActiveWeapon.Instance.transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y), Mathf.Abs(transform.localScale.z));
        newWeapon.transform.parent = ActiveWeapon.Instance.transform;
        ActiveWeapon.Instance.NewWeapon(newWeapon.GetComponent<MonoBehaviour>());
    }
}
