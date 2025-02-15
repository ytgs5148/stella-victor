using UnityEngine;
using System.Collections;
public class Rifle : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponInfo weaponInfo;
    private void Update()
    {
        MouseFollowWithOffset();
    }
    public void Attack()
    {
        Debug.Log("Rifle Attack");
    }
    private void MouseFollowWithOffset()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 gunPos = ActiveWeapon.Instance.transform.position;

        Vector3 direction = mousePos - gunPos;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        bool isLeft = mousePos.x < gunPos.x;
        ActiveWeapon.Instance.transform.localScale = new Vector3(1, isLeft ? -1 : 1, 1);

        float smoothedAngle = Mathf.LerpAngle(ActiveWeapon.Instance.transform.eulerAngles.z, angle, Time.deltaTime * 10);
        ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 180, -smoothedAngle);
    }
    public WeaponInfo GetWeaponInfo() {
        return weaponInfo;
    }
}
