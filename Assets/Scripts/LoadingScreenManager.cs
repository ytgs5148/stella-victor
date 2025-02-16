using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class LoadingScreenManager : MonoBehaviour
{
    public GameObject LoadingScreen;
    public VideoPlayer videoPlayer;

    private bool videoFinished = false;

    public void LoadScene(int sceneId)
    {
        LoadingScreen.SetActive(true);
        StartCoroutine(PlayVideoAndLoadScene(sceneId));
    }

    IEnumerator PlayVideoAndLoadScene(int sceneId)
    {
        videoPlayer.loopPointReached += OnVideoFinished;

        videoPlayer.playbackSpeed = 2.0f;
        videoPlayer.Play();

        while (!videoFinished)
        {
            yield return null;
        }

        videoPlayer.loopPointReached -= OnVideoFinished;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
        while (!operation.isDone)
        {
            yield return null;
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        videoFinished = true;
    }
}
