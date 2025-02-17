using UnityEngine;
using UnityEngine.UI;

public class DialogPopupManager : MonoBehaviour
{
    public static DialogPopupManager Instance;

    public GameObject dialogPopupPanel;
    public GameObject blurPanel;
    public Button closeButton;
    public Button stealBtn;
    public Button reportBtn;
    public LoadingScreenManager LoadingScreenManager;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        dialogPopupPanel.SetActive(false);
        blurPanel.SetActive(false);

        closeButton.onClick.AddListener(HidePopup);
    }

    public void ShowPopup()
    {   
        blurPanel.SetActive(true);
        dialogPopupPanel.SetActive(true);
    }

    public void HidePopup()
    {
        dialogPopupPanel.SetActive(false);
        blurPanel.SetActive(false);
    }

    public void StealBtn()
    {
        blurPanel.SetActive(false);
        dialogPopupPanel.SetActive(false);

        PlayerData.Instance.endingBar += 0.5f;
        PlayerData.Instance.xp += 200;
        PlayerData.Instance.UpdateMaxHealth();
        PlayerData.Instance.UnlockUniqueWeapon();
        PlayerData.Instance.planetsExplored.Add(FindFirstObjectByType<PlanetData>().planetName);

        FindFirstObjectByType<AudioManager>().Play("Steal Sound");
        PlayerController.Instance.gameObject.SetActive(false);

        PlayerData.Instance.currentHealth = PlayerData.Instance.maxHealth;
        PlayerData.Instance.armourHealth = PlayerData.Instance.armourMaxHealth;

        LoadingScreenManager.LoadScene(1);
    }

    public void ReportBtn()
    {
        blurPanel.SetActive(false);
        dialogPopupPanel.SetActive(false);
        PlayerData.Instance.endingBar -= 0.5f;
        PlayerData.Instance.xp += 150;
        PlayerData.Instance.UpdateMaxHealth();
        PlayerData.Instance.planetsExplored.Add(FindFirstObjectByType<PlanetData>().planetName);

        FindFirstObjectByType<AudioManager>().Play("Steal Sound");

        LoadingScreenManager.LoadScene(1);
    }
}
