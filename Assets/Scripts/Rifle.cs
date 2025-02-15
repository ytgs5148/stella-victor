using UnityEngine;
using System.Collections;
public class Rifle : MonoBehaviour, IWeapon
{
    private void Update()
    {
        MouseFollowWithOffset();
    }
    public void Attack() {
        Debug.Log("Rifle Attack");
        StartCoroutine(ResetAttack());
    }

    private IEnumerator ResetAttack() {
        yield return new WaitForSeconds(0.75f);
        ActiveWeapon.Instance.ToggleIsAttacking(false);
    }
    private void MouseFollowWithOffset() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 gunPos = ActiveWeapon.Instance.transform.position;

        // Get the direction from the gun to the mouse
        Vector3 direction = mousePos - gunPos;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        bool isLeft = mousePos.x < gunPos.x;
        ActiveWeapon.Instance.transform.localScale = new Vector3(1, isLeft ? -1 : 1, 1);

        // Smoothly rotate the gun
        float smoothedAngle = Mathf.LerpAngle(ActiveWeapon.Instance.transform.eulerAngles.z, angle, Time.deltaTime * 10);
        ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 180, -smoothedAngle);
    }
}
