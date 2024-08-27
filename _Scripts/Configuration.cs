using System;

namespace MyAdvertisement
{
    [Serializable]
    public class AdConfigData
    {
        public Configuration BannerAdConfig;
        public Configuration InterstitialAdConfig;
        public Configuration RewardedAdConfig;
    }
    
    [Serializable]
    public class Configuration
    {
        public bool Show;
        public AdSettings[] Settings;
    }

    [Serializable]
    public class AdSettings
    {
        public AdsProvider Provider;
        public bool Enabled;
        public int Retry;
        public int Continue;
    }
}