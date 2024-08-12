using GoogleMobileAds.Api;
using UnityEngine;

namespace MyAdvertisement
{
    [CreateAssetMenu(
        fileName = "GoogleMobileAdsRewardedAd", 
        menuName = "My Advertisement/Google Mobile Ads/Rewarded Ad", 
        order = 4
    )]
    public class GoogleMobileAdsRewardedAd : RewardedAdBase
    {
        private readonly string TestAndroidAdID = "ca-app-pub-3940256099942544/5224354917";
        private readonly string TestiOSAdID = "ca-app-pub-3940256099942544/1712485313";

        protected override string AdID =>
            AdNetwork.Testing ? Application.platform is RuntimePlatform.Android ? TestAndroidAdID : TestiOSAdID : base.AdID;

        private RewardedAd _rewardedAd;

        public override void Load(IRewardAdCallbackListener listener)
        {
            Destroy();
            CurrentState = AdState.Loading;
            _rewardAdCallback = listener;
            
            AdRequest adRequest = new ();
            RewardedAd.Load(AdID, adRequest, AdLoadCallback);
        } 

        public override void OnLoadFail()
        {
            CurrentState = AdState.Null;
            _rewardAdCallback?.OnRewardedAdLoadFail();
        }

        public override void OnLoadSuccess()
        {
            CurrentState = AdState.Loaded;
            Debug.Log($"Google mobile ads: Rewarded ad loaded!");
            _rewardAdCallback?.OnRewardedAdLoadSuccess();
        }

        public override void Show()
        {
            if (_rewardedAd is not null && _rewardedAd.CanShowAd()) _rewardedAd.Show(_ => OnGiveReward());
        }

        public override void OnShowFail()
        {
            CurrentState = AdState.Null;
            _rewardAdCallback?.OnRewardedAdShowFail();
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
            
            if (_rewardedAd is null) return;
            _rewardedAd?.Destroy();
            _rewardedAd = null;
        }

        private void AdLoadCallback(RewardedAd ad, LoadAdError error)
        {
            if (error is not null || ad is null)
            {
                Debug.LogError($"Google mobile ads: Rewarded ad failed to load an ad with error: {error}");
                OnLoadFail();
                return;
            }
  
            _rewardedAd = ad;
            _rewardedAd.OnAdFullScreenContentFailed += adError =>
            {
                Debug.LogError($"Google mobile ads: Rewarded ad failed to show an ad with error: {adError}");
                OnShowFail();
            };
            _rewardedAd.OnAdFullScreenContentClosed += OnClose;
            OnLoadSuccess();                    
        }
    }
}