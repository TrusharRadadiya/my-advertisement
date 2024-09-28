using System;
using UnityEngine;

namespace MyAdvertisement
{
    public static class RewardedAdsHandler
    {
        private static RewardedAdsController _controller;
        private static bool Initialized => _controller is not null;
        public static bool AdAvailable => _controller?.AdAvailable ?? false;
        
        public static event Action OnAdAvailable;

        private static bool RewardedRemoved
        {
            get => PlayerPrefs.GetInt(nameof(RewardedRemoved), 0) is 1;
            set => PlayerPrefs.SetInt(nameof(RewardedRemoved), value ? 1 : 0);
        }
        
        public static void Initialize(RewardedAdsController controller) 
        {
            _controller = controller;
            _controller.OnAdAvailable += () =>
            {
                if (RewardedRemoved)
                {
                    DestroyAd();
                    return;
                }
                OnAdAvailable?.Invoke();
            };
        }

        public static void LoadAd()
        {
            if (RewardedRemoved || !Initialized) return;
            _controller.LoadRewardedAd();
        }

        public static void ShowAd(Action<AdsProvider> onClose, Action onReward)
        {
            if (RewardedRemoved || !Initialized)
            {
                onClose?.Invoke(AdsProvider.Null);
                return;
            }
            _controller.ShowRewardedAd(onClose, onReward);
        }

        public static void DestroyAd()
        {
            if (!Initialized) return;
            _controller.DestroyRewardedAd();
        }

        public static void RemoveAd()
        {
            RewardedRemoved = true;
            DestroyAd();
        }

        public static void RestoreAd() => RewardedRemoved = false;
    }
}