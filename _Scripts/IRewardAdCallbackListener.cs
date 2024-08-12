namespace MyAdvertisement
{
    public interface IRewardAdCallbackListener
    {
        void OnRewardedAdLoadFail();
        void OnRewardedAdLoadSuccess();
        void OnRewardedAdShowFail();
        void OnRewardedAdClose();
        void OnRewardedAdGiveReward();
    }
}