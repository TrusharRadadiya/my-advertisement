using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyAdvertisement
{
    [RequireComponent(typeof(AdNetworksController))]
    public class RewardedAdsController : MonoBehaviour, IRewardAdCallbackListener
    {
        private AdNetworksController _networksController;
        
        [SerializeField] private bool _enabled = true;
        [SerializeField] private List<RewardedAdBase> _rewardedAds = new();
        [SerializeField] private List<RewardedAdSettings> _adSettings = new();
        
        private RewardedAdSettings _currentAdSettings;
        private RewardedAdBase _rewardedAd;
        private bool _loadOnNetworkInitialize;
        private bool _shownOnce;
        private int _adCounter;
        private int _retryCount;
        private int _continueCount;

        private Action<AdsProvider> _onClose;
        private Action _onReward;

        public bool AdAvailable => _rewardedAd?.CurrentState is AdState.Loaded;
        public event Action OnAdAvailable;
        
        private void Awake()
        {
            _networksController = GetComponent<AdNetworksController>();
            RewardedAdsHandler.Initialize(this);
        }

        public void OnAdNetworksInitialization()
        {
            SetAdSettings();
            if (!_loadOnNetworkInitialize) return;
            _loadOnNetworkInitialize = false;
            LoadRewardedAd();
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
            _rewardedAd = _currentAdSettings.Ad;
            _retryCount = 0;
            _continueCount = 0;
        }

        public void SetRemoteConfig(Configuration configuration)
        {
            _enabled = configuration.Show;
            if (!_enabled)
            {
                DestroyRewardedAd();
                return;
            }
            
            _adSettings.Clear();
            foreach (AdSettings settings in configuration.Settings)
            {
                if (!settings.Enabled) continue;
                RewardedAdBase rewardedAd = _rewardedAds.Find(ad => ad.Provider == settings.Provider);
                if (rewardedAd is null) continue;
                
                RewardedAdSettings rewardedAdSettings = new (rewardedAd, settings.Retry, settings.Continue);
                _adSettings.Add(rewardedAdSettings);
            }
            SetAdSettings();
        }

        public void LoadRewardedAd()
        {
            if (!_enabled) return;
            if (!CheckAdNetworkInitialization())
            {
                _loadOnNetworkInitialize = true;
                return;
            }
            if (_rewardedAd.CurrentState is not AdState.Null) return;
            if (_continueCount == _currentAdSettings.ContinueCount)
            {
                _rewardedAd.Destroy();
                _adCounter = (_adCounter + 1) % _adSettings.Count;
                SetAdSettings();
            }
            
            _rewardedAd.Load(this);
        }
        
        public void OnRewardedAdLoadFail()
        {
            _retryCount++;
            // All the retries are not done, so retry.
            if (_retryCount != _currentAdSettings.RetryCount)
            {
                LoadRewardedAd();
                return;
            }
            
            _adCounter++;
            // All the interstitials are used.
            if (_adCounter == _adSettings.Count)
            {
                _adCounter = 0;
                SetAdSettings();
                // In once cycle if once the interstitial ad is shown that repeat otherwise rest until next load call.
                if (!_shownOnce) return;
                _shownOnce = false;
                LoadRewardedAd();
                return;
            }
            
            // Fetch the next banner and load.
            SetAdSettings();
            LoadRewardedAd();
        }

        public void OnRewardedAdLoadSuccess() => OnAdAvailable?.Invoke();
        
        public void ShowRewardedAd(Action<AdsProvider> onClose, Action onReward)
        {
            if (!_enabled)
            {
                DestroyRewardedAd();
                onClose?.Invoke(AdsProvider.Null);
                return;
            }
            
            if (!CheckAdNetworkInitialization())
            {
                _loadOnNetworkInitialize = true;
                return;
            }

            switch (_rewardedAd.CurrentState)
            {
                case AdState.Null:
                    LoadRewardedAd();
                    onClose?.Invoke(AdsProvider.Null);
                    break;
                
                case AdState.Loading:
                    onClose?.Invoke(AdsProvider.Null);
                    break;
                
                case AdState.Loaded:
                    _shownOnce = true;
                    _onClose = onClose;
                    _onReward = onReward;
                    _rewardedAd.Show();
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public void OnRewardedAdShowFail()
        {
            _onClose?.Invoke(AdsProvider.Null);
            _onClose = null;
            OnRewardedAdLoadFail();
        }

        public void OnRewardedAdClose()
        {
            _continueCount++;
            _retryCount = 0;
            _onClose?.Invoke(_rewardedAd.Provider);
            _onClose = null;
            _onReward = null;
            _rewardedAd.Destroy();
        }

        public void OnRewardedAdGiveReward() => _onReward?.Invoke();

        public void DestroyRewardedAd()
        {
            if (_rewardedAd is null || _rewardedAd.CurrentState is AdState.Null) return;
            _rewardedAd.Destroy();
            _continueCount++;
            _retryCount = 0;
            _onClose = null;
            _onReward = null;
        }
    }
    
    [Serializable]
    public class RewardedAdSettings
    {
        [field: SerializeField] public RewardedAdBase Ad { get; private set; }
        [field: SerializeField] public int RetryCount { get; private set; }
        [field: SerializeField] public int ContinueCount { get; private set; }

        public RewardedAdSettings(RewardedAdBase ad, int retryCount, int continueCount)
        {
            Ad = ad;
            RetryCount = retryCount;
            ContinueCount = continueCount;
        }
    }
}