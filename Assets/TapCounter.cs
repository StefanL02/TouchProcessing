using UnityEngine;
using TMPro;

public class TapCounter : MonoBehaviour
{
    public static TapCounter Instance;
    private int tapCount = 0;
    [SerializeField] private TMP_Text tapCountText;

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
        if (AdManager.Instance != null)
            AdManager.Instance.OnRewardEarned += OnRewardEarned;
    }

    public void RegisterTap()
    {
        tapCount++;

        if (tapCountText != null)
            tapCountText.text = "Taps: " + tapCount;

        if (PlayServicesManager.Instance == null) return;
        if (!PlayServicesManager.Instance.IsAuthenticated) return;

        PlayServicesManager.Instance.PostScore(tapCount);

        if (tapCount == 1)
            PlayServicesManager.Instance.UnlockAchievement(GPGSIds.achievement_first_touch);

        PlayServicesManager.Instance.IncrementAchievement(GPGSIds.achievement_getting_warmed_up, 1);
        PlayServicesManager.Instance.IncrementAchievement(GPGSIds.achievement_tap_master, 1);
    }

    private void OnRewardEarned(string type, double amount)
    {
        int bonus = 10;
        tapCount += bonus;

        if (tapCountText != null)
            tapCountText.text = "Taps: " + tapCount;

        if (PlayServicesManager.Instance != null && PlayServicesManager.Instance.IsAuthenticated)
            PlayServicesManager.Instance.PostScore(tapCount);

        Debug.Log("Bonus taps added: " + bonus);
    }

    public int GetTapCount()
    {
        return tapCount;
    }

    public void RegisterScale()
    {
        if (PlayServicesManager.Instance != null && PlayServicesManager.Instance.IsAuthenticated)
            PlayServicesManager.Instance.UnlockAchievement(GPGSIds.achievement_scaler);
    }

    void OnDestroy()
    {
        if (AdManager.Instance != null)
            AdManager.Instance.OnRewardEarned -= OnRewardEarned;
    }
}