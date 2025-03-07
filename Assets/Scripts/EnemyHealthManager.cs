using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthManager : MonoBehaviour
{
    [SerializeField] public float enemyHeight;
    public Slider Slider;
    public Color Low;
    public Color High;
    public Vector3 Offset;
    public void SetHealth(float currentHealth, float maxHealth) {
        Offset = new Vector3(0, enemyHeight, 0);
        Slider.gameObject.SetActive(currentHealth < maxHealth);
        Slider.value = currentHealth;
        Slider.maxValue = maxHealth;
        Slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(Low, High, Slider.normalizedValue);
    }
    private void Update()
    {
        Slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + Offset);   
    }
}
