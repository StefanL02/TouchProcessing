using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;

public class PlayServicesManager : MonoBehaviour
{
    public static PlayServicesManager Instance;
    public bool IsAuthenticated => PlayGamesPlatform.Instance.IsAuthenticated();

    [SerializeField] private TMP_Text authStatusText;

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
                if (authStatusText != null)
                    authStatusText.text = "Signed In!";
            }
            else
            {
                Debug.Log("Sign in failed: " + success);
                if (authStatusText != null)
                    authStatusText.text = "Sign in failed: " + success;
            }
        });
    }

    public void PostScore(long score)
    {
        if (!IsAuthenticated) return;
        Social.ReportScore(score, GPGSIds.leaderboard_high_score, (bool success) =>
        {
            Debug.Log("Score posted: " + success);
        });
    }

    public void UnlockAchievement(string achievementId)
    {
        if (!IsAuthenticated) return;
        Social.ReportProgress(achievementId, 100.0f, (bool success) =>
        {
            Debug.Log("Achievement unlocked: " + success);
        });
    }

    public void IncrementAchievement(string achievementId, int steps)
    {
        if (!IsAuthenticated) return;
        PlayGamesPlatform.Instance.IncrementAchievement(achievementId, steps, (bool success) =>
        {
            Debug.Log("Achievement incremented: " + success);
        });
    }

    public void ShowLeaderboard()
    {
        if (!IsAuthenticated) { SignIn(); return; }
        PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_high_score);
    }

    public void ShowAchievements()
    {
        if (!IsAuthenticated) { SignIn(); return; }
        Social.ShowAchievementsUI();
    }
}