using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;

    public GameObject popupPanel;
    public TextMeshProUGUI popupText;
    public TextMeshProUGUI popupDesc;
    public GameObject blurPanel;
    public Button closeButton;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        popupPanel.SetActive(false);
        blurPanel.SetActive(false);
        
        // closeButton.onClick.AddListener(HidePopup);
    }

    public void ShowPopup(string title, string info) {
        popupText.text = title;
        popupDesc.text = info;

        blurPanel.SetActive(true);

        MarkerManager[] markers = FindObjectsOfType<MarkerManager>(true);
        foreach (MarkerManager marker in markers)
        {
            marker.gameObject.SetActive(false);
        }


        popupPanel.SetActive(true);
    }

    public void HidePopup() {
        popupPanel.SetActive(false);

        MarkerManager[] markers = FindObjectsOfType<MarkerManager>(true);
        foreach (MarkerManager marker in markers)
        {
            marker.gameObject.SetActive(true);
        }

        blurPanel.SetActive(false);
    }
}
