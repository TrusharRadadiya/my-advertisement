using MyAdvertisement;
using TMPro;
using UnityEngine;

public class BannerAdPanelScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _bannerAdsStatusText;

    private void Awake()
    {
        BannerAdsHandler.OnAdAvailable += OnBannerAdAvailable;
    }
    
    private void OnDestroy()
    {
        BannerAdsHandler.OnAdAvailable -= OnBannerAdAvailable;
    }
    
    public void OnBannerAdAvailable() => UpdateBannerAdStatus("Banner ad loaded");
    
    public void OnLoadBannerAdButtonClick()
    {
        BannerAdsHandler.LoadAd();
#if UNITY_EDITOR
        OnBannerAdAvailable();
#else
        UpdateBannerAdStatus("Banner ad loading.");
#endif
    }

    public void OnShowBannerAdButtonClick()
    {
        BannerAdsHandler.ShowAd();
        UpdateBannerAdStatus("Banner ad showing.");
    }

    public void OnHideBannerAdButtonClick()
    {
        BannerAdsHandler.HideAd();
        UpdateBannerAdStatus("Banner ad hide.");
    }

    public void OnDestroyBannerAdButtonClick()
    {
        BannerAdsHandler.DestroyAd();
        UpdateBannerAdStatus("Banner ad destroyed.");
    }

    public void OnRemoveBannerAdButtonClick()
    {
        BannerAdsHandler.RemoveAd();
        UpdateBannerAdStatus("Banner ad removed.");
    }

    public void OnRestoreBannerAdButtonClick()
    {
        BannerAdsHandler.RestoreAd();
        UpdateBannerAdStatus("Banner ad restored.");
    }
    
    private void UpdateBannerAdStatus(string text) => _bannerAdsStatusText.text = text;
}