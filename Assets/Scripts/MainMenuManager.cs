using UnityEngine;

public class NewGameManager : MonoBehaviour
{
    public LoadingScreenManager LoadingScreenManager;

    public void NewGame()
    {
        TimerManager.Instance.StartTimer(300f);
        FindFirstObjectByType<AudioManager>().Play("Button Click");
        LoadingScreenManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        FindFirstObjectByType<AudioManager>().Play("Button Click");
        Application.Quit();
    }
}