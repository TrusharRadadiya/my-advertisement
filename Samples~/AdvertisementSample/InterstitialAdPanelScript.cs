using MyAdvertisement;
using TMPro;
using UnityEngine;

public class InterstitialAdPanelScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _interstitialAdsStatusText;

    private void Awake()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        InterstitialAdsHandler.OnAdAvailable += OnInterstitialAdAvailable;
    }

    private void OnDestroy()
    {
        InterstitialAdsHandler.OnAdAvailable -= OnInterstitialAdAvailable;
    }

    private void OnInterstitialAdAvailable() => UpdateInterstitialAdStatus("Interstitial ad loaded.");

    public void OnLoadInterstitialAdButtonClick()
    {
        InterstitialAdsHandler.LoadAd();
#if UNITY_EDITOR
        OnInterstitialAdAvailable();
#else
        UpdateInterstitialAdStatus("Interstitial ad loading...");
#endif
    }

    public void OnShowInterstitialAdShowButtonClick()
    {
        InterstitialAdsHandler.ShowAd(provider => UpdateInterstitialAdStatus($"{provider} : Interstitial ad closed."));
        UpdateInterstitialAdStatus("Interstitial ad showing...");
    }

    public void OnDestroyInterstitialAdButtonClick()
    {
        InterstitialAdsHandler.DestroyAd();
        UpdateInterstitialAdStatus("Interstitial ad destroyed.");
    }

    public void OnRemoveInterstitialAdButtonClick()
    {
        InterstitialAdsHandler.RemoveAd();
        UpdateInterstitialAdStatus("Interstitial ad removed.");
    }

    public void OnRestoreInterstitialAdButtonClick()
    {
        InterstitialAdsHandler.RestoreAd();
        UpdateInterstitialAdStatus("Interstitial ad restored.");
    }
    
    private void UpdateInterstitialAdStatus(string text) => _interstitialAdsStatusText.text = text;
}