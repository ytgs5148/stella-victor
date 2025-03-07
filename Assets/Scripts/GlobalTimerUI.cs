using UnityEngine;
using TMPro;

public class GlobalTimerUI : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    private static GlobalTimerUI instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (TimerManager.Instance != null)
        {
            float time = TimerManager.Instance.timeRemaining;
            
            int minutes = Mathf.FloorToInt(time / 60f);
            int seconds = Mathf.FloorToInt(time % 60f);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}
