namespace MyAdvertisement
{
    public interface IInterstitialAdCallbackListener
    {
        void OnInterstitialAdLoadFail();
        void OnInterstitialAdLoadSuccess();
        void OnInterstitialAdShowFail();
        void OnInterstitialAdClose();
    }
}