using TMPro;
using UnityEngine;

public class LeaderboardEntryUI : MonoBehaviour
{
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI scoreText;

    public void SetEntry(string playerName, int score)
    {
        playerNameText.text = playerName;
        scoreText.text = score.ToString();
    }
}
