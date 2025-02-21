using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private WeaponInfo weaponInfo;
    private Vector3 startPosition;
    private void Start()
    {
        startPosition = transform.position;
    }
    private void Update()
    {
        MoveProjectile();
        DetectFireDistance();
    }
    public void UpdateWeaponInfo(WeaponInfo weaponInfo) {
        this.weaponInfo = weaponInfo;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
        Indestructible indestructible = other.gameObject.GetComponent<Indestructible>();
        if(!other.isTrigger && (enemyHealth || indestructible)) {
            float damage = weaponInfo.weaponDamage;
            MonoBehaviour currentWeapon = ActiveWeapon.Instance.CurrentActiveWeapon;
            if(currentWeapon is Bow) {
                damage *= 1 + (float) PlayerData.Instance.bowXP/1000;
            } else if(currentWeapon is Rifle) {
                damage *= 1 + (float) PlayerData.Instance.laserGunXP/1000;
            } else {
                damage *= 1;
            }
            enemyHealth.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
    private void DetectFireDistance() {
        if(Vector3.Distance(transform.position, startPosition) > weaponInfo.weaponRange) {
            Destroy(gameObject);
        }
    }

    private void MoveProjectile() {
        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
    }
}
