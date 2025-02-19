using UnityEngine;

public class DamageSource : MonoBehaviour
{
    [SerializeField] private float damageAmount = 25;
    private void Start()
    {
        damageAmount *= 1 + (float) PlayerData.Instance.lightSaberXP/1000;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<EnemyHealth>()) {
            EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
            enemyHealth.TakeDamage(damageAmount);
        }
    }
}
