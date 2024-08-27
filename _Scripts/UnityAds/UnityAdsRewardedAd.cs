using UnityEngine;
using UnityAds = UnityEngine.Advertisements;

namespace MyAdvertisement
{
    [CreateAssetMenu(fileName = "UnityAdsRewardedAd", menuName = "My Advertisement/Unity Ads/Rewarded Ad", order = 4)]
    public class UnityAdsRewardedAd : RewardedAdBase, UnityAds.IUnityAdsLoadListener, UnityAds.IUnityAdsShowListener
    {
        public override AdsProvider Provider => AdsProvider.Unity;
        
        public override void Load(IRewardAdCallbackListener listener)
        {
            Destroy();
            CurrentState = AdState.Loading;
            _rewardAdCallback = listener;
            UnityAds.Advertisement.Load(AdID, this);
        } 

        public void OnUnityAdsFailedToLoad(string placementId, UnityAds.UnityAdsLoadError error, string message)
        {
            if (!placementId.Equals(AdID)) return;
            Debug.LogError($"Unity ads: Rewarded ad failed to load an ad with error: {error}, {message}");
            OnLoadFail();
        }

        public void OnUnityAdsAdLoaded(string placementId)
        {
            if (!placementId.Equals(AdID)) return; 
            OnLoadSuccess();
        }

        public override void OnLoadFail()
        {
            CurrentState = AdState.Null;
            _rewardAdCallback?.OnRewardedAdLoadFail();
        }

        public override void OnLoadSuccess()
        {
            CurrentState = AdState.Loaded;
            Debug.Log($"Unity ads: Rewarded ad loaded!");
            _rewardAdCallback?.OnRewardedAdLoadSuccess();
        }

        public override void Show() => UnityAds.Advertisement.Show(AdID, this);

        public void OnUnityAdsShowFailure(string placementId, UnityAds.UnityAdsShowError error, string message)
        {
            if (!placementId.Equals(AdID)) return; 
            Debug.LogError($"Unity ads: Rewarded ad failed to show an ad with error: {error}, {message}");
            OnShowFail();
        }

        public override void OnShowFail()
        {
            CurrentState = AdState.Null;
            _rewardAdCallback?.OnRewardedAdShowFail();
        }
        
        public void OnUnityAdsShowComplete(string placementId, UnityAds.UnityAdsShowCompletionState showCompletionState)
        {
            if (!placementId.Equals(AdID)) return; 
            if (showCompletionState is UnityAds.UnityAdsShowCompletionState.COMPLETED) OnGiveReward();
            OnClose();
        }

        public override void OnClose()
        {
            CurrentState = AdState.Null;
            _rewardAdCallback?.OnRewardedAdClose();
        }

        public override void OnGiveReward() => _rewardAdCallback?.OnRewardedAdGiveReward();
        
        public override void Destroy()
        {
            CurrentState = AdState.Null;
            _rewardAdCallback = null;
        }
        
        public void OnUnityAdsShowStart(string placementId)
        {
        }

        public void OnUnityAdsShowClick(string placementId)
        {
        }
    }
}