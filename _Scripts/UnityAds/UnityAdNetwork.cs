using UnityEngine;
using UnityEngine.Advertisements;
using UnityAds = UnityEngine.Advertisements.Advertisement;

namespace MyAdvertisement
{
    [CreateAssetMenu(fileName = "UnityAdsNetwork", menuName = "My Advertisement/Unity Ads/Network", order = 2)]
    public class UnityAdNetwork : AdNetworkBase, IUnityAdsInitializationListener
    {
        private string AppID => Application.platform is RuntimePlatform.Android ? AndroidAppID : IOSAppID;

        public override bool Initialized => UnityAds.isInitialized;

        public override void Initialize(IAdNetworkInitializationCallback initializationCallback)
        {
            if (!UnityAds.isInitialized && UnityAds.isSupported)
            {
                _initializationCallback = initializationCallback;
                UnityAds.Initialize(AppID, Testing, this);
            }
        }

        public void OnInitializationComplete()
        {
            Debug.Log("Unity ads initialized!");
            _initializationCallback?.OnInitialization();
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.LogError($"Unity ads failed to initialize:{error}, {message}");
        }
    }
}