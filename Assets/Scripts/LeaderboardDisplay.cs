using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using Unity.Services.Leaderboards;
using Unity.Services.Authentication;
using Unity.Services.Core;

public class LeaderboardDisplay : MonoBehaviour
{
    [Header("UI References")]
    public Transform leaderboardContent;
    public GameObject leaderboardEntryPrefab;

    [Header("Leaderboard Settings")]
    public string leaderboardId = "stella-victor-points-leaderboard";

    private async void Start()
    {
        await InitializeUnityServices();
        await FetchAndDisplayLeaderboard();
    }

    private async Task InitializeUnityServices()
    {
        if (UnityServices.State != ServicesInitializationState.Initialized)
        {
            await UnityServices.InitializeAsync();
        }
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    private async Task FetchAndDisplayLeaderboard()
    {
        Debug.Log("Fetching Leaderboard");
        var leaderboardResponse = await LeaderboardsService.Instance.GetScoresAsync(leaderboardId);

        Debug.Log("Displaying Leaderboard");

        foreach (Transform child in leaderboardContent)
        {
            Destroy(child.gameObject);
        }

        foreach (var entry in leaderboardResponse.Results)
        {
            Debug.Log($"Player: {entry.PlayerName}, Score: {entry.Score}");
            GameObject entryObject = Instantiate(leaderboardEntryPrefab, leaderboardContent);
            LeaderboardEntryUI entryUI = entryObject.GetComponent<LeaderboardEntryUI>();
            if (entryUI != null)
            {
                entryUI.SetEntry(entry.PlayerName, (int)entry.Score);
            }
        }
    }

    public async void RefreshLeaderboard()
    {
        Debug.Log("Refreshing Leaderboard");
        await FetchAndDisplayLeaderboard();
    }
}
