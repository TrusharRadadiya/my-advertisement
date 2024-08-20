using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyAdvertisement
{
    [RequireComponent(typeof(AdNetworksController))]
    public class InterstitialAdsController : MonoBehaviour, IInterstitialAdCallbackListener
    {
        private AdNetworksController _networksController;
        
        [SerializeField] private List<InterstitialAdSettings> _adSettings = new();
        
        private InterstitialAdSettings _currentAdSettings;
        private InterstitialAdBase _interstitialAd;
        private bool _loadOnNetworkInitialize;
        private bool _shownOnce;
        private int _adCounter;
        private int _retryCount;
        private int _continueCount;

        private Action _onClose;
        
        public bool AdAvailable => _interstitialAd?.CurrentState is AdState.Loaded;
        public event Action OnAdAvailable;
        
        private void Awake()
        {
            _networksController = GetComponent<AdNetworksController>();
            InterstitialAdsHandler.Initialize(this);
        }

        private void OnDestroy() => _interstitialAd?.Destroy();

        public void OnAdNetworksInitialization()
        {
            SetAdSettings();
            if (!_loadOnNetworkInitialize) return;
            _loadOnNetworkInitialize = false;
            LoadInterstitialAd();
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
            _interstitialAd = _currentAdSettings.Ad;
            _retryCount = 0;
            _continueCount = 0;
        }
        
        public void LoadInterstitialAd()
        {
            if (!CheckAdNetworkInitialization())
            {
                _loadOnNetworkInitialize = true;
                return;
            }
            
            if (_interstitialAd.CurrentState is not AdState.Null) return;
            if (_continueCount == _currentAdSettings.ContinueCount)
            {
                _interstitialAd.Destroy();
                _adCounter = (_adCounter + 1) % _adSettings.Count;
                SetAdSettings();
            }
            
            _interstitialAd.Load(this);
        }
        
        public void OnInterstitialAdLoadFail()
        {
            _retryCount++;
            // All the retries are not done, so retry.
            if (_retryCount != _currentAdSettings.RetryCount)
            {
                LoadInterstitialAd();
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
                LoadInterstitialAd();
                return;
            }
            
            // Fetch the next banner and load.
            SetAdSettings();
            LoadInterstitialAd();
        }

        public void OnInterstitialAdLoadSuccess() => OnAdAvailable?.Invoke();

        public void ShowInterstitialAd(Action onClose)
        {
            if (!CheckAdNetworkInitialization())
            {
                _loadOnNetworkInitialize = true;
                return;
            }

            switch (_interstitialAd.CurrentState)
            {
                case AdState.Null:
                    LoadInterstitialAd();
                    onClose?.Invoke();
                    break;
                
                case AdState.Loading:
                    onClose?.Invoke();
                    break;
                
                case AdState.Loaded:
                    _shownOnce = true;
                    _onClose = onClose;
                    _interstitialAd.Show();
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public void OnInterstitialAdShowFail()
        {
            _onClose?.Invoke();
            _onClose = null;
            OnInterstitialAdLoadFail();
        }

        public void OnInterstitialAdClose()
        {
            _continueCount++;
            _retryCount = 0;
            _onClose?.Invoke();
            _onClose = null;
            _interstitialAd.Destroy();
        }

        public void DestroyInterstitialAd()
        {
            if (_interstitialAd.CurrentState is AdState.Null) return;
            _interstitialAd.Destroy();
            _continueCount++;
            _retryCount = 0;
            _onClose = null;
        }
    }
    
    [Serializable]
    public class InterstitialAdSettings
    {
        [field: SerializeField] public InterstitialAdBase Ad { get; private set; }
        [field: SerializeField] public int RetryCount { get; private set; }
        [field: SerializeField] public int ContinueCount { get; private set; }
    }
}