#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace MyAdvertisement
{
    public abstract class AdNetworkBase: ScriptableObject
    {
        protected IAdNetworkInitializationCallback _initializationCallback;
        
        [field:SerializeField] public bool Testing { get; protected set; }
        
        [field: SerializeField] public string AndroidAppID { get; private set; }
        [field: SerializeField] public string IOSAppID { get; private set; }
        
        public virtual bool Initialized { get; protected set; }

        public abstract void Initialize(IAdNetworkInitializationCallback initializationCallback);
        
#if UNITY_EDITOR
        public void EnableTesting(bool value)
        {
            Testing = value;
            EditorUtility.SetDirty(this);
        }

        public virtual void ResetSettings()
        {
            Initialized = false;
            EditorUtility.SetDirty(this);
        }
#endif
    }
}