namespace MyAdvertisement
{
    public abstract class BannerAdBase: AdBase
    {
        protected IBannerAdCallbackListener _bannerAdCallback;
        
        public abstract void Load(IBannerAdCallbackListener listener);
        public abstract void OnLoadFail();
        public abstract void OnLoadSuccess();
        public abstract void Show();
        public abstract void Hide();
        public abstract void Destroy();
    }
}