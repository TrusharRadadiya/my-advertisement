using GoogleMobileAds.Api;
using UnityEngine;

namespace MyAdvertisement
{
    [CreateAssetMenu(fileName = "GoogleMobileAdsInterstitialAd", menuName = "My Advertisement/Google Mobile Ads/Interstitial Ad", order = 3)]
    public class GoogleMobileAdsInterstitialAd : InterstitialAdBase
    {
        private readonly string TestAndroidAdID = "ca-app-pub-3940256099942544/1033173712";
        private readonly string TestiOSAdID = "ca-app-pub-3940256099942544/4411468910";

        public override AdsProvider Provider => AdsProvider.Google;
        protected override string AdID =>
            AdNetwork.Testing ? Application.platform is RuntimePlatform.Android ? TestAndroidAdID : TestiOSAdID : base.AdID;

        private InterstitialAd _interstitialAd;

        public override void Load(IInterstitialAdCallbackListener listener)
        {
            Destroy();
            CurrentState = AdState.Loading;
            _interstitialAdCallback = listener;
            
            AdRequest adRequest = new ();
            InterstitialAd.Load(AdID, adRequest, AdLoadCallback);
        } 

        public override void OnLoadFail()
        {
            CurrentState = AdState.Null;
            _interstitialAdCallback?.OnInterstitialAdLoadFail();
        }

        public override void OnLoadSuccess()
        {
            CurrentState = AdState.Loaded;
            Debug.Log($"Google mobile ads: Interstitial ad loaded!");
            _interstitialAdCallback?.OnInterstitialAdLoadSuccess();
        }

        public override void Show()
        {
            if (_interstitialAd is not null && _interstitialAd.CanShowAd()) _interstitialAd.Show();
        }

        public override void OnShowFail()
        {
            CurrentState = AdState.Null;
            _interstitialAdCallback?.OnInterstitialAdShowFail();
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
            
            if (_interstitialAd is null) return;
            _interstitialAd?.Destroy();
            _interstitialAd = null;
        }

        private void AdLoadCallback(InterstitialAd ad, LoadAdError error)
        {
            if (error is not null || ad is null)
            {
                Debug.LogError($"Google mobile ads: Interstitial ad failed to load an ad with error: {error}");
                OnLoadFail();
                return;
            }
  
            _interstitialAd = ad;
            _interstitialAd.OnAdFullScreenContentFailed += adError =>
            {
                Debug.LogError($"Google mobile ads: Interstitial ad failed to show an ad with error: {adError}");
                OnShowFail();
            };
            _interstitialAd.OnAdFullScreenContentClosed += OnClose;
            OnLoadSuccess();                    
        }
    }
}