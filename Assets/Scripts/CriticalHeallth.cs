using UnityEngine;

public class CriticalHeallth : MonoBehaviour
{
    private bool bossDialogueDone = false;
    private void Update()
    {
        if (bossDialogueDone) return;

        EnemyHealth enemyHealth = FindFirstObjectByType<EnemyHealth>();
        if (enemyHealth == null) return;

        if (enemyHealth.currentHealth / enemyHealth.startingHealth <= 0.1)
        {
            bossDialogueDone = true;
            // bossFightManager.StartBossDialog();
            FindFirstObjectByType<BossFightManager>().StartBossDialog();
        }
    }
}
