using UnityEngine;

public class NewGameManager : MonoBehaviour
{
    public LoadingScreenManager LoadingScreenManager;

    public void NewGame()
    {
        TimerManager.Instance.StartTimer(300f);
        LoadingScreenManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}