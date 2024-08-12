using GoogleMobileAds.Api;
using UnityEngine;

namespace MyAdvertisement
{
    [CreateAssetMenu(fileName = "GoogleMobileAdsNetwork", menuName = "My Advertisement/Google Mobile Ads/Network", order = 1)]
    public class GoogleMobileAdNetwork : AdNetworkBase
    {
        public readonly string TestAppID = "ca-app-pub-3940256099942544~3347511713";

        public override void Initialize(IAdNetworkInitializationCallback initializationCallback)
        {
            if (Initialized) return;
            _initializationCallback = initializationCallback;
            MobileAds.RaiseAdEventsOnUnityMainThread = true;
            MobileAds.Initialize(OnInitializationComplete);
        }
        
        private void OnInitializationComplete(InitializationStatus initializationStatus)
        {
            Debug.Log("Google mobile ads initialized!");
            Initialized = true;
            _initializationCallback?.OnInitialization();
        }
    }
}