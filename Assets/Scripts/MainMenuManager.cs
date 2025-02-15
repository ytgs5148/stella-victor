using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameManager : MonoBehaviour
{
    public void NewGame()
    {
        TimerManager.Instance.StartTimer(300f);
        SceneManager.LoadScene("GalacticAtlasScene");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}