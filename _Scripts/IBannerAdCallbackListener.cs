namespace MyAdvertisement
{
    public interface IBannerAdCallbackListener
    {
        void OnBannerAdLoadFail();
        void OnBannerAdLoadSuccess();
    }
}