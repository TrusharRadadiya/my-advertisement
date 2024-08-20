using MyAdvertisement;
using TMPro;
using UnityEngine;

public class RewardedAdPanelScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _rewardedAdStatusText;

    private void Awake()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        RewardedAdsHandler.OnAdAvailable += OnRewardedAdAvailable;
    }

    private void OnDestroy()
    {
        RewardedAdsHandler.OnAdAvailable -= OnRewardedAdAvailable;
    }

    private void OnRewardedAdAvailable()=> UpdateRewardedAdStatus("Rewarded ad loaded.");

    public void OnLoadRewardedAdButtonClick()
    {
        RewardedAdsHandler.LoadAd();
#if UNITY_EDITOR
        OnRewardedAdAvailable();
#else
        UpdateRewardedAdStatus("Rewarded ad loading...");
#endif
    }

    public void OnShowRewardedAdShowButtonClick()
    {
        RewardedAdsHandler.ShowAd(
            () => UpdateRewardedAdStatus("Rewarded ad closed."),
            () => UpdateRewardedAdStatus("Reward granted.")
        );
        UpdateRewardedAdStatus("Rewarded ad showing...");
    }

    public void OnDestroyRewardedAdButtonClick()
    {
        RewardedAdsHandler.DestroyAd();
        UpdateRewardedAdStatus("Rewarded ad destroyed.");
    }

    public void OnRemoveRewardedAdButtonClick()
    {
        RewardedAdsHandler.RemoveAd();
        UpdateRewardedAdStatus("Rewarded ad removed.");
    }

    public void OnRestoreRewardedAdButtonClick()
    {
        RewardedAdsHandler.RestoreAd();
        UpdateRewardedAdStatus("Rewarded ad restored.");
    }
    
    private void UpdateRewardedAdStatus(string text) => _rewardedAdStatusText.text = text;
}