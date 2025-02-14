using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float lerpSpeed = 1.0f;

    private Vector3 offset;

    private Vector3 targetPos;

    private void Start()
    {
        if (PlayerController.Instance == null) return;

        offset = transform.position - PlayerController.Instance.transform.position;
    }

    private void Update()
    {
        if (PlayerController.Instance == null) return;

        targetPos = PlayerController.Instance.transform.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);
    }

}
