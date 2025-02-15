using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActiveInventory : MonoBehaviour
{
    private int activeSlotIndexNum = 1;
    private PlayerControl playerControl;
    private void Awake()
    {
        playerControl = new PlayerControl();
    }
    private void Start()
    {
        playerControl.Inventory.Keyboard.performed += ctx => ToggleActiveSlot((int)ctx.ReadValue<float>());
        ToggleActiveHighlight(0);
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
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 gunPos = ActiveWeapon.Instance.transform.position;
        bool isLeft = mousePos.x < gunPos.x;
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
        if (!isLeft)
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(180, 0, 0);
        }
        newWeapon.transform.parent = ActiveWeapon.Instance.transform;
        ActiveWeapon.Instance.NewWeapon(newWeapon.GetComponent<MonoBehaviour>());
    }
}
