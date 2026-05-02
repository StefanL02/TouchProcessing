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

    public void RegisterTap()
    {
        tapCount++;

        // Update UI
        if (tapCountText != null)
            tapCountText.text = "Taps: " + tapCount;

        // Update leaderboard
        PlayServicesManager.Instance.PostScore(tapCount);

        // First Touch
        if (tapCount == 1)
            PlayServicesManager.Instance.UnlockAchievement(GPGSIds.achievement_first_touch);

        // Getting Warmed Up (incremental)
        PlayServicesManager.Instance.IncrementAchievement(GPGSIds.achievement_getting_warmed_up, 1);

        // Tap Master (incremental)
        PlayServicesManager.Instance.IncrementAchievement(GPGSIds.achievement_tap_master, 1);
    }

    public int GetTapCount()
    {
        return tapCount;
    }
}