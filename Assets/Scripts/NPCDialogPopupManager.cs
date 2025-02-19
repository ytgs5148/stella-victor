using UnityEngine;
using UnityEngine.UI;

public class NPCDialogPopupManager : MonoBehaviour
{
    public static NPCDialogPopupManager Instance;

    public GameObject dialogPopupPanel;
    public GameObject blurPanel;
    public Button killBtn;
    public Button pardonBtn;
    public LoadingScreenManager LoadingScreenManager;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        dialogPopupPanel.SetActive(false);
        blurPanel.SetActive(false);
    }

    public void ShowPopup()
    {   
        Debug.Log("Show Popup");
        blurPanel.SetActive(true);
        dialogPopupPanel.SetActive(true);
    }

    public void HidePopup()
    {
        dialogPopupPanel.SetActive(false);
        blurPanel.SetActive(false);
    }

    public void KillBtn()
    {
        Debug.Log("Kill");
        blurPanel.SetActive(false);
        dialogPopupPanel.SetActive(false);

        PlayerData.Instance.endingBar -= 0.5f;
        PlayerData.Instance.xp += 200;
        PlayerData.Instance.UpdateMaxHealth();
        PlayerData.Instance.UnlockUniqueWeapon();
        PlayerData.Instance.planetsExplored.Add(FindFirstObjectByType<PlanetData>().planetName);

        FindFirstObjectByType<AudioManager>().Play("Steal Sound");

        LoadingScreenManager.LoadScene(1);
    }

    public void PardonBtn()
    {
        blurPanel.SetActive(false);
        dialogPopupPanel.SetActive(false);
        PlayerData.Instance.endingBar += 0.5f;
        PlayerData.Instance.xp += 200;
        PlayerData.Instance.UpdateMaxHealth();
        PlayerData.Instance.armourXP += 200;
        PlayerData.Instance.UpdateArmourMaxHealth();
        PlayerData.Instance.planetsExplored.Add(FindFirstObjectByType<PlanetData>().planetName);

        FindFirstObjectByType<AudioManager>().Play("Steal Sound");

        LoadingScreenManager.LoadScene(1);
    }
}
