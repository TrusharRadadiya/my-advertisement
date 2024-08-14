using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MyAdvertisement
{
    public class AdNetworksController : MonoBehaviour, IAdNetworkInitializationCallback
    {
        #region -: Editor settings :-
#if UNITY_EDITOR
        [SerializeField] public bool _testing;
        
        private void OnValidate()
        {
            foreach (AdNetworkBase adsNetwork in _adNetworks) adsNetwork?.EnableTesting(_testing);
        }

        private void OnDestroy()
        {
            foreach (AdNetworkBase adNetwork in _adNetworks) adNetwork?.ResetSettings();
        }
#endif
        #endregion

        [SerializeField] private List<AdNetworkBase> _adNetworks = new ();
        
        public bool Initialized => _adNetworks.Find(adNetwork => !adNetwork.Initialized) is null;

        public UnityEvent OnInitializationComplete;
        
        private void Awake() => InitializeAdNetworks();

        public void InitializeAdNetworks()
        {
            foreach (AdNetworkBase adsNetwork in _adNetworks) adsNetwork.Initialize(this);
        }

        public void OnInitialization()
        {
            if (Initialized) OnInitializationComplete?.Invoke();
        }
    }
}