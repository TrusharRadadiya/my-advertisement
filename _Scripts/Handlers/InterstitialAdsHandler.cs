using System;
using UnityEngine;

namespace MyAdvertisement
{
    public static class InterstitialAdsHandler
    {
        private static InterstitialAdsController _controller;
        private static bool Initialized => _controller is not null;
        public static bool AdAvailable => _controller?.AdAvailable ?? false;
        
        private static bool InterstitialRemoved
        {
            get => PlayerPrefs.GetInt(nameof(InterstitialRemoved), 0) is 1;
            set => PlayerPrefs.SetInt(nameof(InterstitialRemoved), value ? 1 : 0);
        }
        
        public static event Action OnAdAvailable;

        public static void Initialize(InterstitialAdsController controller)
        {
            _controller = controller;
            _controller.OnAdAvailable += () =>
            {
                if (InterstitialRemoved)
                {
                    DestroyAd();
                    return;
                }

                OnAdAvailable?.Invoke();
            };
        }

        public static void LoadAd()
        {
            if (InterstitialRemoved) return;
            if (!Initialized) return;
            _controller.LoadInterstitialAd();
        }

        public static void ShowAd(Action onClose)
        {
            if (InterstitialRemoved) return;
            if (!Initialized)
            {
                onClose?.Invoke();
                return;
            }
            _controller.ShowInterstitialAd(onClose);
        }

        public static void DestroyAd()
        {
            if (!Initialized) return;
            _controller.DestroyInterstitialAd();
        }
        
        public static void RemoveAd()
        {
            InterstitialRemoved = true;
            DestroyAd();
        }

        public static void RestoreAd() => InterstitialRemoved = false;
    }
}