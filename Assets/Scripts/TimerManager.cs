using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerManager : MonoBehaviour
{
    public static TimerManager Instance { get; private set; }

    public float timeRemaining = 300f;
    public bool isTimerRunning = false;
    public LoadingScreenManager LoadingScreenManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0f)
            {
                timeRemaining = 0f;
                isTimerRunning = false;

                LoadingScreenManager.LoadScene(3);
            }
        }
    }

    public void StartTimer(float startTime)
    {
        isTimerRunning = true;
        timeRemaining = startTime;
    }
}
