using UnityEngine;
using System.Collections;
public class Bow : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowSpawnPoint;
    private Animator myAnimator;
    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }
    private void Update()
    {
        MouseFollowWithOffset();
    }
    public void Attack()
    {
        myAnimator.SetBool("IsFiring", true);
        GameObject newArrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, ActiveWeapon.Instance.transform.rotation);
        newArrow.GetComponent<Projectile>().UpdateWeaponInfo(weaponInfo);
        StartCoroutine(ResetAttack());
    }
    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(0.5f);
        myAnimator.SetBool("IsFiring", false);
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
    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }
}
