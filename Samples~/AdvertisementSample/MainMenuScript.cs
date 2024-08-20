using TMPro;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _adNetworkStatusText;
    [SerializeField] private GameObject _bannerAdButton;
    [SerializeField] private GameObject _interstitialAdButton;

    private void Awake()
    {
        _adNetworkStatusText.text = "Initializing ad networks!";
        _bannerAdButton.SetActive(false);
        _interstitialAdButton.SetActive(false);
    }

    public void OnAdsNetworksInitialized()
    {
        _bannerAdButton.SetActive(true);
        _interstitialAdButton.SetActive(false);
        _adNetworkStatusText.text = "Ad networks initialized!!";
    }
}