using TMPro;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _adNetworkStatusText;
    [SerializeField] private GameObject _bannerAdButton;
    [SerializeField] private GameObject _interstitialAdButton;
    [SerializeField] private GameObject _rewardedAdButton;

    private void Awake()
    {
        _adNetworkStatusText.text = "Initializing ad networks!";
        _bannerAdButton.SetActive(false);
        _interstitialAdButton.SetActive(false);
        _rewardedAdButton.SetActive(false);
    }

    public void OnAdsNetworksInitialized()
    {
        _bannerAdButton.SetActive(true);
        _interstitialAdButton.SetActive(true);
        _rewardedAdButton.SetActive(true);
        _adNetworkStatusText.text = "Ad networks initialized!!";
    }
}