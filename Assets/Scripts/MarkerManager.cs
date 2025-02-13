using UnityEngine;
using UnityEngine.EventSystems;

public class MarkerManager : MonoBehaviour, IPointerClickHandler
{
    public string planetName;
    public string planetDescription;
    public string planetElement;
    public int planetDifficultyLevel;

    public void OnPointerClick(PointerEventData eventData)
    {
        PopupManager.Instance.ShowPopup(planetName, planetDescription, planetElement, planetDifficultyLevel);
    }
}
