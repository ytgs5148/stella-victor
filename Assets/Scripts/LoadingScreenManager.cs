using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class LoadingScreenManager : MonoBehaviour
{
    public GameObject LoadingScreen;
    public VideoPlayer videoPlayer;

    public void LoadScene(int sceneId)
    {
        LoadingScreen.SetActive(true);
        StartCoroutine(LoadSceneAsync(sceneId));
    }

    IEnumerator LoadSceneAsync(int sceneId)
    {
        yield return new WaitForSeconds(0.5f);
        
        videoPlayer.playbackSpeed = 0.5f;
        if (!videoPlayer.isPlaying)
            videoPlayer.Play();

        yield return new WaitForSeconds(2f);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            videoPlayer.playbackSpeed = Mathf.Lerp(0.5f, 2.0f, progress);

            yield return null;
        }
    }
}
