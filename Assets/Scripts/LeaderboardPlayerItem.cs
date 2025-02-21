using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards;
using Unity.Services.Core;
using TMPro;

public class LeaderboardPlayerItem : MonoBehaviour
{
    public TMP_InputField usernameInputField;
    public Button submitButton;

    private void Start()
    {
        submitButton.onClick.AddListener(OnSubmitButtonClicked);
    }

    private async void OnSubmitButtonClicked()
    {
        string playerName = usernameInputField.text;
        if (!string.IsNullOrEmpty(playerName))
        {
            submitButton.interactable = false;
            await InitializeUnityServices();

            int currentScore = PlayerData.Instance.xp;

            await AuthenticationService.Instance.UpdatePlayerNameAsync(playerName);
            await SubmitScore(playerName, currentScore);

            PlayerData.Instance.Reset();

            FindFirstObjectByType<LeaderboardDisplay>().RefreshLeaderboard();
        }
    }

    private async Task InitializeUnityServices()
    {
        await UnityServices.InitializeAsync();
        await SignInAnonymously();
    }

    private async Task SignInAnonymously()
    {
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    private async Task SubmitScore(string playerName, int score)
    {
        var leaderboardId = "stella-victor-points-leaderboard";
        await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardId, score);
    }
}
