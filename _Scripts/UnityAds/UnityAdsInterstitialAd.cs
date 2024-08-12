using UnityEngine;
using UnityAds = UnityEngine.Advertisements;

namespace MyAdvertisement
{
    [CreateAssetMenu(fileName = "UnityAdsInterstitialAd", menuName = "My Advertisement/Unity Ads/Interstitial Ad", order = 3)]
    public class UnityInterstitialAd : InterstitialAdBase, UnityAds.IUnityAdsLoadListener, UnityAds.IUnityAdsShowListener
    {
        public override void Load(IInterstitialAdCallbackListener listener)
        {
            Destroy();
            CurrentState = AdState.Loading;
            _interstitialAdCallback = listener;
            UnityAds.Advertisement.Load(AdID, this);
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAds.UnityAdsLoadError error, string message)
        {
            if (!placementId.Equals(AdID)) return;
            Debug.LogError($"Unity ads: Interstitial ad failed to load an ad with error: {error}, {message}");
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
            _interstitialAdCallback?.OnInterstitialAdLoadFail();
        }

        public override void OnLoadSuccess()
        {
            CurrentState = AdState.Loaded;
            Debug.Log($"Unity ads: Interstitial ad loaded!");
            _interstitialAdCallback?.OnInterstitialAdLoadSuccess();
        }

        public override void Show() => UnityAds.Advertisement.Show(AdID, this);
        
        public void OnUnityAdsShowFailure(string placementId, UnityAds.UnityAdsShowError error, string message)
        {
            if (!placementId.Equals(AdID)) return; 
            Debug.LogError($"Unity ads: Interstitial ad failed to show an ad with error: {error}, {message}");
            OnShowFail();
        }

        public override void OnShowFail()
        {
            CurrentState = AdState.Null;
            _interstitialAdCallback?.OnInterstitialAdShowFail();
        }
        
        public void OnUnityAdsShowComplete(string placementId, UnityAds.UnityAdsShowCompletionState showCompletionState)
        {
            if (!placementId.Equals(AdID)) return; 
            OnClose();
        }

        public override void OnClose()
        {
            CurrentState = AdState.Null;
            _interstitialAdCallback?.OnInterstitialAdClose();
        }

        public override void Destroy()
        {
            CurrentState = AdState.Null;
            _interstitialAdCallback = null;
        }

        public void OnUnityAdsShowStart(string placementId)
        {
        }

        public void OnUnityAdsShowClick(string placementId)
        {
        }
    }
}