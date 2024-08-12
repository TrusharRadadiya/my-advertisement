using UnityEditor;
using UnityEngine;

namespace MyAdvertisement.Utilities.Editor
{
    public static class ScriptableObjectUtility
    {
        public static T FindAsset<T>() where T : ScriptableObject
        {
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(T)); 
            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            T obj = AssetDatabase.LoadAssetAtPath<T>(path);
            return obj;
        }
        
        public static T[] FindAssets<T>() where T : ScriptableObject
        {
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(T)}");
            T[] assets = new T[guids.Length];
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                assets[i] = AssetDatabase.LoadAssetAtPath<T>(path);
            }

            return assets;
        }
    }
}
