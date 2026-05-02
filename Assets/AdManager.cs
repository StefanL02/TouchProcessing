using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance { get; private set; }

    [Header("Android Ad Unit IDs")]
    [SerializeField] private string bannerAdUnitId = "ca-app-pub-6713247261707466/9283254410";       
    [SerializeField] private string interstitialAdUnitId = "ca-app-pub-6713247261707466/4516966713"; 
    [SerializeField] private string rewardedAdUnitId = "ca-app-pub-6713247261707466/9363145460";     

    private BannerView bannerView;
    private InterstitialAd interstitialAd;
    private RewardedAd rewardedAd;

    // Event fired when user earns a reward
    public event Action<string, double> OnRewardEarned;

    void Awake()
    {
        // Singleton - persists across scenes
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Initialise the Google Mobile Ads SDK
        MobileAds.Initialize(initStatus =>
        {
            Debug.Log("AdMob Initialised");
            LoadBanner();
            LoadInterstitial();
            LoadRewarded();
        });
    }

    // BANNER

    private void LoadBanner()
    {
        // Destroy any existing banner first
        if (bannerView != null)
        {
            bannerView.Destroy();
            bannerView = null;
        }

        AdSize adSize = AdSize.Banner;
        bannerView = new BannerView(bannerAdUnitId, adSize, AdPosition.Bottom);

        bannerView.OnBannerAdLoaded += () =>
            Debug.Log("Banner loaded.");

        bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
            Debug.LogError("Banner failed to load: " + error.GetMessage());

        AdRequest request = new AdRequest();
        bannerView.LoadAd(request);
    }

    public void ShowBanner()
    {
        if (bannerView != null)
            bannerView.Show();
    }

    public void HideBanner()
    {
        if (bannerView != null)
            bannerView.Hide();
    }

    // INTERSTITIAL

    private void LoadInterstitial()
    {
        // Clean up previous instance
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        AdRequest request = new AdRequest();

        InterstitialAd.Load(interstitialAdUnitId, request, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("Interstitial failed to load: " + error?.GetMessage());
                return;
            }

            Debug.Log("Interstitial loaded.");
            interstitialAd = ad;

            // Reload when closed so its ready for next time
            interstitialAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Interstitial closed. Reloading...");
                LoadInterstitial();
            };

            interstitialAd.OnAdFullScreenContentFailed += (AdError adError) =>
            {
                Debug.LogError("Interstitial failed to show: " + adError.GetMessage());
                LoadInterstitial();
            };
        });
    }

    public void ShowInterstitial()
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show();
        }
        else
        {
            Debug.LogWarning("Interstitial not ready yet.");
            LoadInterstitial();
        }
    }

    // REWARDED

    private void LoadRewarded()
    {
        // Clean up previous instance
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        AdRequest request = new AdRequest();

        RewardedAd.Load(rewardedAdUnitId, request, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("Rewarded ad failed to load: " + error?.GetMessage());
                return;
            }

            Debug.Log("Rewarded ad loaded.");
            rewardedAd = ad;

            // Reload when closed so its ready for next time
            rewardedAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Rewarded ad closed. Reloading...");
                LoadRewarded();
            };

            rewardedAd.OnAdFullScreenContentFailed += (AdError adError) =>
            {
                Debug.LogError("Rewarded ad failed to show: " + adError.GetMessage());
                LoadRewarded();
            };
        });
    }

    public void ShowRewarded()
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show(reward =>
            {
                Debug.Log($"Reward earned: {reward.Amount} {reward.Type}");
                OnRewardEarned?.Invoke(reward.Type, reward.Amount);
            });
        }
        else
        {
            Debug.LogWarning("Rewarded ad not ready yet.");
            LoadRewarded();
        }
    }

    void OnDestroy()
    {
        bannerView?.Destroy();
        interstitialAd?.Destroy();
        rewardedAd?.Destroy();
    }
}
