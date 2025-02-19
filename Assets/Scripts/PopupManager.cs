using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;

    public GameObject popupPanel;
    public TextMeshProUGUI popupText;
    public TextMeshProUGUI popupDesc;
    public TextMeshProUGUI popupElement;
    public TextMeshProUGUI popupDifficulty;
    public TextMeshProUGUI popupObjective;
    public GameObject blurPanel;
    public Button closeButton;
    public Button liberateButton;
    public LoadingScreenManager LoadingScreenManager;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        popupPanel.SetActive(false);
        blurPanel.SetActive(false);

        closeButton.onClick.AddListener(HidePopup);
    }

    public void ShowPopup(string title, string info, string element, int difficulty, int objectiveType)
    {
        if (PlayerData.Instance.planetsExplored.Contains(title))
        {
            Debug.Log("Planet already explored: " + title);
            return;
        }

        FindFirstObjectByType<AudioManager>().Play("Button Click");

        popupText.text = title;
        popupDesc.text = info;
        popupElement.text = "Element: " + element;
        popupElement.color = element == "Forest" ? new Color32(41, 126, 84, 255) : Color.blue;
        popupDifficulty.text = "Difficulty: " + difficulty;

        switch(objectiveType)
        {
            case 0:
                popupObjective.text = "Objective: Eliminate Planet";
                break;
            case 1:
                popupObjective.text = "Objective: Save Weaponry";
                break;
            default:
                popupObjective.text = "Objective: Unknown";
                break;
        }

        switch (difficulty)
        {
            case 1:
                popupDifficulty.color = new Color32(41, 126, 0, 255);
                break;
            case 2:
                popupDifficulty.color = new Color32(98, 103, 10, 255);
                break;
            case 3:
                popupDifficulty.color = new Color32(255, 108, 0, 255);
                break;
            case 4:
                popupDifficulty.color = Color.red;
                break;
            default:
                popupDifficulty.color = Color.white;
                break;
        }

        blurPanel.SetActive(true);

        #pragma warning disable CS0618 // Type or member is obsolete
        MarkerManager[] markers = FindObjectsOfType<MarkerManager>(true);
        #pragma warning restore CS0618 // Type or member is obsolete

        foreach (MarkerManager marker in markers)
            marker.gameObject.SetActive(false);

        popupPanel.SetActive(true);
    }

    public void HidePopup()
    {
        popupPanel.SetActive(false);

        #pragma warning disable CS0618 // Type or member is obsolete
        MarkerManager[] markers = FindObjectsOfType<MarkerManager>(true);
        #pragma warning restore CS0618 // Type or member is obsolete

        foreach (MarkerManager marker in markers)
            marker.gameObject.SetActive(true);

        blurPanel.SetActive(false);
    }

    public void LiberatePlanet()
    {
        FindFirstObjectByType<AudioManager>().Play("Button Click");

        PlanetData.Instance.planetName = popupText.text;
        PlanetData.Instance.planetDesc = popupDesc.text;
        PlanetData.Instance.planetElement = "Element: " + popupElement.text;
        PlanetData.Instance.planetDifficulty = int.Parse(popupDifficulty.text.Substring(12));
        PlanetData.Instance.planetObjectiveType = popupObjective.text.Contains("Eliminate") ? 0 : 1;

        Debug.Log("Liberating planet: " + PlanetData.Instance.planetName);

        if (PlanetData.Instance.planetObjectiveType == 0)
        {
            PlayerData.Instance.kills = 0;

            // NPC
            if (PlanetData.Instance.planetElement == "Element: Element: Forest")
                LoadingScreenManager.LoadScene(2);
            else if (PlanetData.Instance.planetElement == "Element: Element: Ancient")
                LoadingScreenManager.LoadScene(6);
            else
                LoadingScreenManager.LoadScene(8);
        }
        else
        {
            PlayerData.Instance.kills = 0;

            // Weaponry
            if (PlanetData.Instance.planetElement == "Element: Element: Forest")
                LoadingScreenManager.LoadScene(4);
            else if (PlanetData.Instance.planetElement == "Element: Element: Ancient")
                LoadingScreenManager.LoadScene(5);
            else
                LoadingScreenManager.LoadScene(7);
        }
    }
}
