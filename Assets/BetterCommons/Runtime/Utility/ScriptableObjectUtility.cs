using System.IO;
using UnityEditor;
using UnityEngine;

namespace Better.Commons.Runtime.Utility
{
    public static class ScriptableObjectUtility
    {
#if UNITY_EDITOR
        public static T CreateScriptableObjectAsset<T>(string path)
            where T : ScriptableObject
        {
            const string assetsPath = AssetDatabaseUtility.AssetsPath;
            const string assetExtension = AssetDatabaseUtility.AssetExtension;

            var lastFolder = Path.GetDirectoryName(path);
            lastFolder ??= string.Empty;
            if (lastFolder.StartsWith(assetsPath))
            {
                lastFolder = lastFolder.Remove(0, assetsPath.Length);
            }

            var absolutePath = Path.Combine(Application.dataPath, lastFolder);
            if (!Directory.Exists(absolutePath))
            {
                Directory.CreateDirectory(absolutePath);
                AssetDatabase.Refresh(ImportAssetOptions.Default);
            }

            if (!path.StartsWith(assetsPath))
            {
                path = Path.Combine(assetsPath, path);
            }

            if (!path.EndsWith(assetExtension))
            {
                var name = typeof(T).Name;
                path = Path.Combine(path, $"{name}{assetExtension}");
            }

            var asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, path);
            return asset;
        }

        public static T CreateScriptableObjectAsset<T>(params string[] pathParts)
            where T : ScriptableObject
        {
            var path = Path.Combine(pathParts);
            return CreateScriptableObjectAsset<T>(path: path);
        }
#endif
    }
}