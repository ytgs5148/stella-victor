using UnityEngine;

public class CriticalHeallth : MonoBehaviour
{
    private bool bossDialogueDone = false;
    private void Update()
    {
        if (bossDialogueDone) return;
        if (EnemyHealth.Instance.currentHealth / EnemyHealth.Instance.startingHealth <= 0.1)
        {
            bossDialogueDone = true;
            // bossFightManager.StartBossDialog();
            FindFirstObjectByType<BossFightManager>().StartBossDialog();
        }
    }
}
