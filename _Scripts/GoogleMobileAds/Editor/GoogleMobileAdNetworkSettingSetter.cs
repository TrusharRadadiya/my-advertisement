using MyAdvertisement.Utilities.Editor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace MyAdvertisement.Editor
{
    public class GoogleMobileAdNetworkSettingSetter: IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;
        
        public void OnPreprocessBuild(BuildReport report)
        {
            GoogleMobileAdNetwork googleMobileAdNetwork = ScriptableObjectUtility.FindAsset<GoogleMobileAdNetwork>(); 
            googleMobileAdNetwork.ResetSettings();
        }
    }
}