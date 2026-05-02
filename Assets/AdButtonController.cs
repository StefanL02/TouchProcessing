using UnityEngine;
using TMPro;

public class AdButtonController : MonoBehaviour
{
    [Header("Optional Reward feedback label")]
    [SerializeField] private TMP_Text rewardStatusText;

    void Start()
    {
        // Subscribe to reward event
        if (AdManager.Instance != null)
            AdManager.Instance.OnRewardEarned += HandleReward;
    }

    void OnDestroy()
    {
        if (AdManager.Instance != null)
            AdManager.Instance.OnRewardEarned -= HandleReward;
    }

    
    public void OnInterstitialButtonPressed()
    {
        if (AdManager.Instance != null)
            AdManager.Instance.ShowInterstitial();
    }

    
    public void OnRewardedButtonPressed()
    {
        if (AdManager.Instance != null)
            AdManager.Instance.ShowRewarded();
    }

    
    public void OnShowBannerPressed()
    {
        if (AdManager.Instance != null)
            AdManager.Instance.ShowBanner();
    }

    public void OnHideBannerPressed()
    {
        if (AdManager.Instance != null)
            AdManager.Instance.HideBanner();
    }

    // Called automatically when the user earns a reward
    private void HandleReward(string rewardType, double rewardAmount)
    {
        Debug.Log($"Player earned: {rewardAmount} x {rewardType}");

        if (rewardStatusText != null)
            rewardStatusText.text = $"Reward earned: {rewardAmount} {rewardType}!";

        // TODO: Add your in-game reward logic here
        // e.g. restore an object, unlock a feature, add points etc.
    }
}
