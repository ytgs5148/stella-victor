using UnityEngine;
using UnityEngine.EventSystems;

public class MarkerManager : MonoBehaviour, IPointerClickHandler
{
    public string planetName;
    public string planetDescription;
    public int planetLevel;
    public string planetElementalType;
    public string planetWinXP;

    public void OnPointerClick(PointerEventData eventData)
    {
        PopupManager.Instance.ShowPopup(planetName, planetDescription, planetLevel, planetElementalType, planetWinXP);
    }
}
