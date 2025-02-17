using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private Image healthBarFill;
    [SerializeField] private Image ArmourBarFill;
    [SerializeField] private GameObject armourIcon;
    [SerializeField] private GameObject armourBar;
    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        healthBarFill.fillAmount = currentHealth / maxHealth;
    }
    public void UpdateArmourBar(float currentHealth, float maxHealth)
    {
        ArmourBarFill.fillAmount = currentHealth / maxHealth;
    }
    public void UpdateVisibility(bool isArmourAvailable)
    {
        if (isArmourAvailable)
        {
            armourIcon.SetActive(true);
            armourBar.SetActive(true);
            Debug.Log("(1) Armour icon and Armour bar available");
        }
        else
        {
            armourIcon.SetActive(false);
            armourBar.SetActive(false);
            Debug.Log("(X) Armour icon and Armour bar NOT available");
        }
    }
}
