using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class LoadingScreenManager : MonoBehaviour
{
    public GameObject LoadingScreen;
    public VideoPlayer videoPlayer;

    // A flag to indicate that the video has finished.
    private bool videoFinished = false;

    public void LoadScene(int sceneId)
    {
        LoadingScreen.SetActive(true);
        StartCoroutine(PlayVideoAndLoadScene(sceneId));
    }

    IEnumerator PlayVideoAndLoadScene(int sceneId)
    {
        // Subscribe to the loopPointReached event. This event fires when the video finishes playing.
        videoPlayer.loopPointReached += OnVideoFinished;

        // Set playbackSpeed if desired.
        videoPlayer.playbackSpeed = 2.0f;
        videoPlayer.Play();

        // Wait until the video has finished.
        while (!videoFinished)
        {
            yield return null;
        }

        // Optionally, unsubscribe to avoid memory leaks.
        videoPlayer.loopPointReached -= OnVideoFinished;

        // Load the scene once the video is done.
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
        while (!operation.isDone)
        {
            yield return null;
        }
    }

    // This method is called when the video finishes playing.
    void OnVideoFinished(VideoPlayer vp)
    {
        videoFinished = true;
    }
}
