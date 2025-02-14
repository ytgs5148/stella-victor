using UnityEngine;
using System.Collections;
public class Bow : MonoBehaviour, IWeapon
{
    public void Attack() {
        Debug.Log("Bow Attack");
        StartCoroutine(ResetAttack());
    }

    private IEnumerator ResetAttack() {
        yield return new WaitForSeconds(1f); // Small delay before resetting
        ActiveWeapon.Instance.ToggleIsAttacking(false);
    }
}
