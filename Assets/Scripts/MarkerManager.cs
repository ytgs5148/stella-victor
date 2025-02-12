using UnityEngine;
using UnityEngine.EventSystems;

public class MarkerManager : MonoBehaviour, IPointerClickHandler
{
    public string planetName;
    public string planetDescription;

    public void OnPointerClick(PointerEventData eventData)
    {
        PopupManager.Instance.ShowPopup(planetName, planetDescription);
    }
}
