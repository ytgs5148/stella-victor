using UnityEngine;
using TMPro;

public class GlobalXPManager : MonoBehaviour
{
    public TextMeshProUGUI xpText;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (PlayerData.Instance != null)
        {
            xpText.text = PlayerData.Instance.xp.ToString();
        }
    }
}
