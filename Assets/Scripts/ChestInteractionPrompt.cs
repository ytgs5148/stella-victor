using UnityEngine;

public class ChestInteractionPrompt : MonoBehaviour
{
    [Header("References")]
    public GameObject promptUI;

    public Transform playerTransform;

    [Header("Detection Settings")]
    public float detectionRadius = 3.0f;

    public DialogPopupManager dialogPopupManager;

    private void Update()
    {
        if (playerTransform == null || PlayerData.Instance == null)
            return;

        Vector3 chestPos = new Vector3(PlayerData.Instance.chestPosition.x, PlayerData.Instance.chestPosition.y, 0);

        float distance = Vector3.Distance(playerTransform.position, chestPos);

        if (distance <= detectionRadius && PlayerData.Instance.kills >= 15)
        {
            if (!promptUI.activeSelf)
                promptUI.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                OpenChest();
            }
        }
        else
        {
            if (promptUI.activeSelf)
                promptUI.SetActive(false);
        }
    }

    private void OpenChest()
    {
        Debug.Log("Chest opened!");
        FindFirstObjectByType<AudioManager>().Play("Chest Open");

        promptUI.SetActive(false);
        dialogPopupManager.ShowPopup();
    }
}
