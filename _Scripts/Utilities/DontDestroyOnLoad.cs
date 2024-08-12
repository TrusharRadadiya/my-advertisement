using UnityEngine;

namespace MyAdvertisement.Utilities
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        private void Awake() => DontDestroyOnLoad(gameObject);
    }
}