using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public static TimerManager Instance { get; private set; }

    public float timeRemaining = 600f;
    public bool isTimerRunning = false;

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

                FindFirstObjectByType<LoadingScreenManager>().LoadScene(3);
            }
        }
    }

    public void StartTimer(float startTime)
    {
        isTimerRunning = true;
        timeRemaining = startTime;
    }

    public void StopTimer()
    {
        timeRemaining = 1f;
    }
}
