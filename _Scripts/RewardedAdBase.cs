namespace MyAdvertisement
{
    public abstract class RewardedAdBase: AdBase
    {
        protected IRewardAdCallbackListener _rewardAdCallback;
        
        public virtual AdsProvider Provider { get; protected set; }

        public abstract void Load(IRewardAdCallbackListener listener);
        public abstract void OnLoadFail();
        public abstract void OnLoadSuccess();
        public abstract void Show();
        public abstract void OnShowFail();
        public abstract void OnClose();
        public abstract void OnGiveReward();
        public abstract void Destroy();
    }
}