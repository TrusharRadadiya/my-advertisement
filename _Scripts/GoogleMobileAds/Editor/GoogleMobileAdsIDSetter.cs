using MyAdvertisement.Utilities.Editor;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace MyAdvertisement.Editor
{
    public class GoogleMobileAdsIDSetter: IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;
        
        public void OnPreprocessBuild(BuildReport report)
        {
            // GoogleMobileAdNetwork googleMobileAdNetwork = ScriptableObjectUtility.FindAsset<GoogleMobileAdNetwork>(); 
            // GoogleMobileAdsSettings googleMobileAdsSettings = GoogleMobileAdsSettings.LoadInstance();
            // string androidAppID = googleMobileAdNetwork.Testing ? googleMobileAdNetwork.TestAppID : googleMobileAdNetwork.AndroidAppID;
            // string iOSAppID = googleMobileAdNetwork.Testing ? googleMobileAdNetwork.TestAppID : googleMobileAdNetwork.IOSAppID;
            // googleMobileAdsSettings.GoogleMobileAdsAndroidAppId = androidAppID;
            // googleMobileAdsSettings.GoogleMobileAdsIOSAppId = iOSAppID;
            // EditorUtility.SetDirty(googleMobileAdsSettings);
        }
    }
}