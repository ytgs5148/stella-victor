using UnityEngine;
using System.Collections;
public class Rifle : MonoBehaviour, IWeapon
{
    public void Attack() {
        Debug.Log("Rifle Attack");
        StartCoroutine(ResetAttack());
    }

    private IEnumerator ResetAttack() {
        yield return new WaitForSeconds(0.75f);
        ActiveWeapon.Instance.ToggleIsAttacking(false);
    }
}
