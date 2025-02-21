using UnityEngine;
using UnityEngine.Video;

public class PrologueCanvasManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Canvas canvas;

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SkipVideo();
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        canvas.gameObject.SetActive(false);
    }

    void SkipVideo()
    {
        videoPlayer.Stop();
        canvas.gameObject.SetActive(false);
    }
}
