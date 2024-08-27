namespace MyAdvertisement
{
    public abstract class InterstitialAdBase: AdBase
    {
        protected IInterstitialAdCallbackListener _interstitialAdCallback;
        public virtual AdsProvider Provider { get; protected set; }
        
        public abstract void Load(IInterstitialAdCallbackListener listener);
        public abstract void OnLoadFail();
        public abstract void OnLoadSuccess();
        public abstract void Show();
        public abstract void OnShowFail();
        public abstract void OnClose();
        public abstract void Destroy();
    }
}