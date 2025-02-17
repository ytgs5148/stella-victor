using UnityEngine;

public class DamageSource : MonoBehaviour
{
    [SerializeField] private int damageAmount = 25;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<EnemyHealth>()) {
            EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
            enemyHealth.TakeDamage(damageAmount);
        }
    }
}
