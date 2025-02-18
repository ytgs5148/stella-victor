using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossFightManager : MonoBehaviour
{
    [Header("References")]
    public GameObject promptUI;
    public Transform playerTransform;
    public GameObject bossPrefab;
    public TextMeshProUGUI bossBarText;
    public RawImage screenFadeImage;
    public GameObject dialogPopupPanel;

    [Header("Detection Settings")]
    public float detectionRadius = 3.0f;

    private bool bossSpawned = false;

    private void Update()
    {
        if (playerTransform == null || PlayerData.Instance == null || bossSpawned)
            return;

        Vector3 cavePos = new Vector3(PlayerData.Instance.chestPosition.x, PlayerData.Instance.chestPosition.y, 0);

        float distance = Vector3.Distance(playerTransform.position, cavePos);

        if (distance <= detectionRadius && PlayerData.Instance.kills == 8)
        {
            if (!promptUI.activeSelf)
                promptUI.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(SpawnBoss(cavePos));
            }
        }
        else
        {
            if (promptUI.activeSelf)
                promptUI.SetActive(false);
        }
    }

    public void StartBossDialog()
    {
        Debug.Log("Starting boss dialog!");
        dialogPopupPanel.SetActive(true);
    }

    private IEnumerator SpawnBoss(Vector3 cavePos)
    {
        Debug.Log("Spawning boss!");
        FindFirstObjectByType<AudioManager>().Play("Chest Open");

        promptUI.SetActive(false);
        bossSpawned = true;

        yield return StartCoroutine(FadeScreen(true));

        Instantiate(bossPrefab, cavePos, Quaternion.identity);

        bossBarText.text = "Kill the boss";

        yield return StartCoroutine(FadeScreen(false));
    }

    private IEnumerator FadeScreen(bool fadeToBlack)
    {
        float fadeDuration = 1.0f;
        float fadeSpeed = 1.0f / fadeDuration;
        float alpha = fadeToBlack ? 0 : 1;

        while (fadeToBlack ? alpha < 1 : alpha > 0)
        {
            alpha += fadeToBlack ? fadeSpeed * Time.deltaTime : -fadeSpeed * Time.deltaTime;
            screenFadeImage.color = new Color(0, 0, 0, Mathf.Clamp01(alpha));
            yield return null;
        }
    }
}
