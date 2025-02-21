using TMPro;
using UnityEngine;

public class BossBarManager : MonoBehaviour
{
    [Header("UI Settings")]
    public RectTransform progressImageRectTransform;
    
    [Header("Width Settings")]
    public float minWidth = 50f;
    public TextMeshProUGUI text;

    public float maxWidth = 200f;
    public int totalKillsRequired = 20;
    
    void Update()
    {
        if (PlayerData.Instance != null)
        {
            int currentKills = PlayerData.Instance.kills;

            text.text = "Kill all enemies: " + currentKills + "/" + totalKillsRequired;

            currentKills = Mathf.Clamp(currentKills, 0, totalKillsRequired);
            
            float fraction = (float)currentKills / totalKillsRequired;
            
            float newWidth = Mathf.Lerp(minWidth, maxWidth, fraction);
            
            Vector2 newSize = progressImageRectTransform.sizeDelta;
            newSize.x = newWidth;
            progressImageRectTransform.sizeDelta = newSize;

            if (currentKills >= totalKillsRequired)
            {
                FindFirstObjectByType<AudioManager>().Play("Button Click");
                text.text = "Find and open the chest!";
            }
        }
    }
}
