using UnityEngine;
using UnityAds = UnityEngine.Advertisements;

namespace MyAdvertisement
{
    [CreateAssetMenu(fileName = "UnityAdsBannerAd", menuName = "My Advertisement/Unity Ads/Banner Ad", order = 2)]
    public class UnityAdsBannerAd : BannerAdBase
    {
        public override AdsProvider Provider => AdsProvider.Unity;
        
        public override void Load(IBannerAdCallbackListener listener)
        {
            Destroy();
            CurrentState = AdState.Loading;
            _bannerAdCallback = listener;
            
            UnityAds.BannerLoadOptions options = new ()
            {
                loadCallback = OnLoadSuccess,
                errorCallback = message =>
                {
                    Debug.LogError($"Unity ads: Banner view failed to load an ad with error: {message}");
                    OnLoadFail();
                }
            };
            
            UnityAds.Advertisement.Banner.SetPosition(UnityAds.BannerPosition.BOTTOM_CENTER);
            UnityAds.Advertisement.Banner.Load(AdID, options);
        }

        public override void OnLoadFail()
        {
            UnityAds.Advertisement.Banner.Hide(true);
            CurrentState = AdState.Null;
            _bannerAdCallback?.OnBannerAdLoadFail();
            _bannerAdCallback = null;
        }

        public override void OnLoadSuccess()
        {
            CurrentState = AdState.Loaded;
            Debug.Log($"Unity ads: Banner view loaded!");
            Hide();
            _bannerAdCallback?.OnBannerAdLoadSuccess();
        }

        public override void Show() => UnityAds.Advertisement.Banner.Show(AdID);

        public override void Hide() => UnityAds.Advertisement.Banner.Hide();
    
        public override void Destroy()
        {
            CurrentState = AdState.Null;
            _bannerAdCallback = null;
            if (!UnityAds.Advertisement.Banner.isLoaded) return;
            UnityAds.Advertisement.Banner.Hide(true);
            Debug.Log($"Unity ads: Banner view destroyed!");
        }
    }
}