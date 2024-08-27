using GoogleMobileAds.Api;
using UnityEngine;

namespace MyAdvertisement
{
    [CreateAssetMenu(fileName = "GoogleMobileAdsBannerAd", menuName = "My Advertisement/Google Mobile Ads/Banner Ad", order = 2)]
    public class GoogleMobileAdsBannerAd : BannerAdBase
    {
        private readonly string TestAndroidAdID = "ca-app-pub-3940256099942544/6300978111";
        private readonly string TestiOSAdID = "ca-app-pub-3940256099942544/2934735716";


        public override AdsProvider Provider => AdsProvider.Google;

        protected override string AdID =>
            AdNetwork.Testing ? Application.platform is RuntimePlatform.Android ? TestAndroidAdID : TestiOSAdID : base.AdID;

        private BannerView _bannerView;

        public override void Load(IBannerAdCallbackListener listener)
        {
            Destroy();
            CurrentState = AdState.Loading;
            _bannerAdCallback = listener;
            
            _bannerView = new BannerView(AdID, AdSize.Banner, AdPosition.Bottom);
            ListenToAdEvents();
            AdRequest adRequest = new ();
            _bannerView.LoadAd(adRequest);
        }

        public override void OnLoadFail()
        {
            CurrentState = AdState.Null;
            _bannerAdCallback?.OnBannerAdLoadFail();
        }

        public override void OnLoadSuccess()
        {
            CurrentState = AdState.Loaded;
            Debug.Log($"Google mobile ads: Banner view loaded!");
            Hide();
            _bannerAdCallback?.OnBannerAdLoadSuccess();
        }

        public override void Show() => _bannerView?.Show();
        
        public override void Hide() => _bannerView?.Hide();

        public override void Destroy()
        {
            CurrentState = AdState.Null;
            _bannerAdCallback = null;
            
            if (_bannerView is null) return;
            _bannerView.Destroy();
            _bannerView = null;
            Debug.Log($"Google mobile ads: Banner view destroyed!");
        }

        private void ListenToAdEvents()
        {
            _bannerView.OnBannerAdLoaded += OnLoadSuccess;
            _bannerView.OnBannerAdLoadFailed += error =>
            {
                Debug.LogError($"Google mobile ads: Banner view failed to load an ad with error: {error}");
                OnLoadFail();
            };
        }
    }
}