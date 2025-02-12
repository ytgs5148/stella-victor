using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;

    public GameObject popupPanel;
    public TextMeshProUGUI popupText;
    public TextMeshProUGUI popupDesc;
    public TextMeshProUGUI popupLevel;
    public TextMeshProUGUI popupType;
    public TextMeshProUGUI popupXP;
    public GameObject blurPanel;
    public Button closeButton;
    public Button liberateButton;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        popupPanel.SetActive(false);
        blurPanel.SetActive(false);

        closeButton.onClick.AddListener(HidePopup);
        liberateButton.onClick.AddListener(LiberatePlanet);
    }

    public void ShowPopup(string title, string info, int level, string type, string xp)
    {
        popupText.text = title;
        popupDesc.text = info;
        popupLevel.text = "Level: " + level;
        popupType.text = "Elemental Type: " + type;
        popupXP.text = "Win XP: " + xp;

        blurPanel.SetActive(true);

        #pragma warning disable CS0618 // Type or member is obsolete
        MarkerManager[] markers = FindObjectsOfType<MarkerManager>(true);
        #pragma warning restore CS0618 // Type or member is obsolete
        foreach (MarkerManager marker in markers)
        {
            marker.gameObject.SetActive(false);
        }


        popupPanel.SetActive(true);
    }

    public void HidePopup()
    {
        popupPanel.SetActive(false);

        #pragma warning disable CS0618 // Type or member is obsolete
        MarkerManager[] markers = FindObjectsOfType<MarkerManager>(true);
        #pragma warning restore CS0618 // Type or member is obsolete
        foreach (MarkerManager marker in markers)
        {
            marker.gameObject.SetActive(true);
        }

        blurPanel.SetActive(false);
    }

    public void LiberatePlanet()
    {
        PlanetData.Instance.planetName = popupText.text;
        PlanetData.Instance.planetDesc = popupDesc.text;
        PlanetData.Instance.planetLevel = popupLevel.text;
        PlanetData.Instance.planetType = popupType.text;
        PlanetData.Instance.planetXP = popupXP.text;

        SceneManager.LoadScene("LevelScene");
    }
}
