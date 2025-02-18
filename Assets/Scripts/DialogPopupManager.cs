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
        PlayerData.Instance.xp += 200 * (PlanetData.Instance.planetDifficulty + 1) / 2;
        PlayerData.Instance.UpdateMaxHealth();
        PlayerData.Instance.UnlockUniqueWeapon();
        PlayerData.Instance.kills = 0;
        PlayerData.Instance.planetsExplored.Add(FindFirstObjectByType<PlanetData>().planetName);

        FindFirstObjectByType<AudioManager>().Play("Steal Sound");

        LoadingScreenManager.LoadScene(1);
    }

    public void ReportBtn()
    {
        blurPanel.SetActive(false);
        dialogPopupPanel.SetActive(false);
        PlayerData.Instance.endingBar -= 0.5f;
        PlayerData.Instance.xp += 200 * (PlanetData.Instance.planetDifficulty + 1) / 2;
        PlayerData.Instance.UpdateMaxHealth();
        if (PlayerData.Instance.isArmourAvailable)
        {
            PlayerData.Instance.armourXP += 200 * (PlanetData.Instance.planetDifficulty + 1) / 2;
        }
        else
        {
            PlayerData.Instance.isArmourAvailable = true;
        }
        PlayerData.Instance.UpdateArmourMaxHealth();
        PlayerData.Instance.kills = 0;
        PlayerData.Instance.planetsExplored.Add(FindFirstObjectByType<PlanetData>().planetName);

        FindFirstObjectByType<AudioManager>().Play("Steal Sound");

        LoadingScreenManager.LoadScene(1);
    }
}
