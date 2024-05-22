using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Better.Commons.Runtime.Utility
{
    public static class AssetDatabaseUtility
    {
#if UNITY_EDITOR
        public const string AssetsPath = "Assets";
        public const string AssetExtension = ".asset";

        private const string PrefabFilter = "t:prefab";

        public static T[] FindPrefabsOfType<T>()
        {
            var prefabs = new List<T>();
            var gameObjects = FindAssetsOfType<GameObject>(PrefabFilter);

            foreach (var gameObject in gameObjects)
            {
                var components = gameObject.GetComponents<Component>();
                foreach (var component in components)
                {
                    if (component is not T item) continue;

                    prefabs.Add(item);
                    break;
                }
            }

            return prefabs.ToArray();
        }

        public static T[] FindAssetsOfType<T>(string filter) where T : Object
        {
            var assets = new List<T>();
            var guids = AssetDatabase.FindAssets(filter);

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<T>(path);

                if (asset != null)
                {
                    assets.Add(asset);
                }
            }

            return assets.ToArray();
        }

        public static T[] FindAssetsOfType<T>() where T : Object
        {
            var typeName = typeof(T).Name;
            var filter = $"t:{typeName}";
            return FindAssetsOfType<T>(filter);
        }
#endif
    }
}