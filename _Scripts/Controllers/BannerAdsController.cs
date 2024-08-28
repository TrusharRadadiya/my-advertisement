using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyAdvertisement
{   
    [RequireComponent(typeof(AdNetworksController))]
    public class BannerAdsController : MonoBehaviour, IBannerAdCallbackListener
    {
        private AdNetworksController _networksController;

        [SerializeField] private bool _enabled = true;
        [SerializeField] private List<BannerAdBase> _bannerAds = new();
        [SerializeField] private List<BannerAdSettings> _adSettings = new();
        
        private BannerAdSettings _currentAdSettings;
        private BannerAdBase _bannerAd;
        private bool _loadOnNetworkInitialize;
        private bool _showOnLoad;
        private bool _shownOnce;
        private int _adCounter;
        private int _retryCount;
        private int _continueCount;
        
        public event Action<AdsProvider> OnAdAvailable;
        
        private void Awake()
        {
            _networksController = GetComponent<AdNetworksController>();
            BannerAdsHandler.Initialize(this);
        }

        private void OnDestroy() => _bannerAd?.Destroy();

        public void OnAdNetworksInitialization()
        {
            SetAdSettings();
            if (!_loadOnNetworkInitialize) return;
            _loadOnNetworkInitialize = false;
            LoadBannerAd();
        }
        
        private bool CheckAdNetworkInitialization()
        {
            if (_networksController.Initialized) return true;
            
            Debug.LogWarning("Ad networks are not initialized!");
            Debug.Log("Starting initialization!");
            _networksController.InitializeAdNetworks();
            return false;
        }

        private void SetAdSettings()
        {
            _currentAdSettings = _adSettings[_adCounter];
            _bannerAd = _currentAdSettings.Ad;
            _retryCount = 0;
            _continueCount = 0;
        }
        
        public void SetRemoteConfig(Configuration configuration)
        {
            _enabled = configuration.Show;
            if (!_enabled)
            {
                DestroyBannerAd();
                return;
            }
            
            _adSettings.Clear();
            foreach (AdSettings settings in configuration.Settings)
            {
                if (!settings.Enabled) continue;
                BannerAdBase bannerAd = _bannerAds.Find(ad => ad.Provider == settings.Provider);
                if (bannerAd is null) continue;
                
                BannerAdSettings bannerAdSettings = new (bannerAd, settings.Retry, settings.Continue);
                _adSettings.Add(bannerAdSettings);
            }
            SetAdSettings();
        }
        
        public void LoadBannerAd()
        {
            if (!_enabled) return;
            if (!CheckAdNetworkInitialization())
            {
                _loadOnNetworkInitialize = true;
                return;
            }
            if (_bannerAd.CurrentState is not AdState.Null) return;
            if (_continueCount == _currentAdSettings.ContinueCount)
            {
                _bannerAd.Destroy();
                _adCounter = (_adCounter + 1) % _adSettings.Count;
                SetAdSettings();
            }
            
            _bannerAd.Load(this);
        }

        public void OnBannerAdLoadFail()
        {
            _retryCount++;
            // All the retries are not done, so retry.
            if (_retryCount != _currentAdSettings.RetryCount)
            {
                LoadBannerAd();
                return;
            }
            
            _adCounter++;
            // All the banners are used.
            if (_adCounter == _adSettings.Count)
            {
                _adCounter = 0;
                SetAdSettings();
                
                // In one cycle if once the banner ad is shown then repeat otherwise rest until next load call.
                if (!_shownOnce) return;
                _shownOnce = false;
                LoadBannerAd();
                return;
            }
            
            // Fetch the next banner and load.
            SetAdSettings();
            LoadBannerAd();
        }

        public void OnBannerAdLoadSuccess()
        {
            if (!_enabled)
            {
                DestroyBannerAd();
                return;
            }
            
            OnAdAvailable?.Invoke(_bannerAd.Provider);
            _continueCount++;
            _retryCount = 0;

            if (!_showOnLoad) return;
            _showOnLoad = false;
            ShowBannerAd();
        }

        public void ShowBannerAd()
        {
            if (!_enabled) return;
            if (!CheckAdNetworkInitialization())
            {
                _loadOnNetworkInitialize = true;
                _showOnLoad = true;
                return;
            }

            switch (_bannerAd.CurrentState)
            {
                case AdState.Null:
                    _showOnLoad = true;
                    LoadBannerAd();
                    break;
                
                case AdState.Loading:
                    _showOnLoad = true;
                    break;
                
                case AdState.Loaded:
                    _bannerAd.Show();
                    _shownOnce = true;
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }   
        }

        public void HideBannerAd()
        {
            if (!_enabled) return;
            if (!CheckAdNetworkInitialization())
            {
                _loadOnNetworkInitialize = false;
                _showOnLoad = false;
                return;
            }

            switch (_bannerAd.CurrentState)
            {
                case AdState.Null:
                case AdState.Loading:
                    _showOnLoad = false;
                    break;
                
                case AdState.Loaded:
                    _bannerAd.Hide();        
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public void DestroyBannerAd()
        {
            _loadOnNetworkInitialize = false;
            _showOnLoad = false;
            if (!CheckAdNetworkInitialization()) return;
            if (_bannerAd  is null || _bannerAd.CurrentState is AdState.Null) return;
            _bannerAd.Destroy();
        }
    }
    
    [Serializable]
    public class BannerAdSettings
    {
        [field: SerializeField] public BannerAdBase Ad { get; private set; }
        [field: SerializeField] public int RetryCount { get; private set; }
        [field: SerializeField] public int ContinueCount { get; private set; }

        public BannerAdSettings(BannerAdBase ad, int retryCount, int continueCount)
        {
            Ad = ad;
            RetryCount = retryCount;
            ContinueCount = continueCount;
        }
    }
}