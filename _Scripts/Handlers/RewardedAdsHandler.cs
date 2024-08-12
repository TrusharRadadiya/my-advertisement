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

        public static void Initialize(RewardedAdsController controller) 
        {
            _controller = controller;
            _controller.OnAdAvailable += () => OnAdAvailable?.Invoke();
        }

        public static void LoadAd()
        {
            if (!Initialized) return;
            _controller.LoadRewardedAd();
        }

        public static void ShowAd(Action onClose, Action onReward)
        {
            if (!Initialized)
            {
                Debug.LogError("Rewarded ad handler is not initialized yet!");
                onClose?.Invoke();
                return;
            }
            _controller.ShowRewardedAd(onClose, onReward);
        }
    }
}