#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace MyAdvertisement
{
    public abstract class AdBase: ScriptableObject 
    {
        [SerializeField] protected AdNetworkBase AdNetwork;
        
        [SerializeField] protected string _androidAdId;
        [SerializeField] protected string _iOSAdId;
        
        protected virtual string AdID => Application.platform == RuntimePlatform.Android ? _androidAdId : _iOSAdId;

        public AdState CurrentState { get; protected set; } = AdState.Null;
        
#if UNITY_EDITOR
        public virtual void ResetSettings()
        {
            CurrentState = AdState.Null;
            EditorUtility.SetDirty(this);
        }
#endif
    }

    public enum AdState
    {
        Null,
        Loading, 
        Loaded
    }
}