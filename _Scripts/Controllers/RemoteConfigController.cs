using Newtonsoft.Json;
using Unity.Services.RemoteConfig;
using UnityEngine;

namespace MyAdvertisement
{
    public class RemoteConfigController : MonoBehaviour
    {
        private const string KEY = "AdvertisementConfig";
        
#if UNITY_EDITOR
        [SerializeField] private AdConfigData configData;
#endif
        
        public void OnRemoteConfigDataFetched(RuntimeConfig config)
        {
            if (!config.HasKey(KEY)) return;
            string data = config.GetJson(KEY);
#if UNITY_EDITOR
            configData = JsonConvert.DeserializeObject<AdConfigData>(data);
#else
            AdConfigData configData = JsonConvert.DeserializeObject<AdConfigData>(data);
#endif

            GetComponent<BannerAdsController>()?.SetRemoteConfig(configData.BannerAdConfig);
            GetComponent<InterstitialAdsController>()?.SetRemoteConfig(configData.InterstitialAdConfig);
            GetComponent<RewardedAdsController>()?.SetRemoteConfig(configData.RewardedAdConfig);
        }
    }
}