using System;
using System.Reflection;
using MyAdvertisement.Utilities.Editor;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using Object = UnityEngine.Object;

namespace MyAdvertisement.Editor
{
    public class GoogleMobileAdsSettingsSetter: IPreprocessBuildWithReport
    {
        public int callbackOrder => -1;
        
        public void OnPreprocessBuild(BuildReport report)
        {
            GoogleMobileAdNetwork googleMobileAdNetwork = ScriptableObjectUtility.FindAsset<GoogleMobileAdNetwork>();
            if (googleMobileAdNetwork is null) return;
            
            string androidAppID = googleMobileAdNetwork.Testing ? googleMobileAdNetwork.TestAppID : googleMobileAdNetwork.AndroidAppID;
            string iOSAppID = googleMobileAdNetwork.Testing ? googleMobileAdNetwork.TestAppID : googleMobileAdNetwork.IOSAppID;
            
            Assembly assembly = Assembly.Load("GoogleMobileAds.Editor");
            Type type = assembly.GetType("GoogleMobileAds.Editor.GoogleMobileAdsSettings");
            MethodInfo loadInstanceMethodInfo = type.GetMethod("LoadInstance", BindingFlags.NonPublic | BindingFlags.Static);
            object obj = loadInstanceMethodInfo.Invoke(null, null);

            PropertyInfo androidAdIdInfo = type.GetProperty("GoogleMobileAdsAndroidAppId");
            PropertyInfo iOSAdIdInfo = type.GetProperty("GoogleMobileAdsIOSAppId");
            PropertyInfo userTrackingDesInfo = type.GetProperty("UserTrackingUsageDescription");
            
            androidAdIdInfo.SetValue(obj, androidAppID);
            iOSAdIdInfo.SetValue(obj, iOSAppID);
            userTrackingDesInfo.SetValue(obj, "Your data will be used to provide you a better and personalized ad experience.");
            EditorUtility.SetDirty(obj as Object);
        }
    }
}