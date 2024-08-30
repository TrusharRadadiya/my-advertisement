using UnityEditor;
using UnityEngine;

namespace MyAdvertisement.Editor
{
    public static class PrefabInstantiator
    {
        [MenuItem("My Advertisement/Add Advertisement Prefab")]
        public static void InstantiatePrefab()
        {
            string[] GUIDs = AssetDatabase.FindAssets("t:prefab");
            foreach (string guid in GUIDs)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (go.name == "MyAdvertisementPrefab")
                {
                    GameObject prefab = Object.Instantiate(go);
                    prefab.name = "Advertisement";
                    return;
                }
            }
        }
    }
}