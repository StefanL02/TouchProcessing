using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class PlayServicesManager : MonoBehaviour
{
    public static PlayServicesManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PlayGamesPlatform.Activate();
        SignIn();
    }

    public void SignIn()
    {
        PlayGamesPlatform.Instance.Authenticate((success) =>
        {
            if (success == SignInStatus.Success)
            {
                Debug.Log("Signed in to Google Play Games!");
            }
            else
            {
                Debug.Log("Sign in failed: " + success);
            }
        });
    }

    public void PostScore(long score)
    {
        if (Social.localUser.authenticated)
        {
            Social.ReportScore(score, GPGSIds.leaderboard_high_score, (bool success) =>
            {
                Debug.Log("Score posted: " + success);
            });
        }
    }

    public void UnlockAchievement(string achievementId)
    {
        if (Social.localUser.authenticated)
        {
            Social.ReportProgress(achievementId, 100.0f, (bool success) =>
            {
                Debug.Log("Achievement unlocked: " + success);
            });
        }
    }

    public void IncrementAchievement(string achievementId, int steps)
    {
        if (Social.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.IncrementAchievement(achievementId, steps, (bool success) =>
            {
                Debug.Log("Achievement incremented: " + success);
            });
        }
    }

    public void ShowLeaderboard()
    {
        if (Social.localUser.authenticated)
            Social.ShowLeaderboardUI();
    }

    public void ShowAchievements()
    {
        if (Social.localUser.authenticated)
            Social.ShowAchievementsUI();
    }
}
