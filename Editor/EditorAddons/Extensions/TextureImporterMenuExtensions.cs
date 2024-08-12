using System;
using Better.Attributes.Runtime.Headers;
using UnityEditor;

namespace Better.Attributes.EditorAddons.Extensions
{
    internal static class TextureImporterMenuExtensions
    {
        [MenuItem("CONTEXT/TextureImporter/Convert To IconHeaderAttribute")]
        private static void CopyMaxSize(MenuCommand command)
        {
            var textureImporter = (TextureImporter)command.context;
            var guid = AssetDatabase.GUIDFromAssetPath(textureImporter.assetPath);

            var s = nameof(IconHeaderAttribute).Replace(nameof(Attribute), string.Empty);
            var systemCopyBuffer = $"[{s}(\"{guid.ToString()}\")]";
            EditorGUIUtility.systemCopyBuffer = systemCopyBuffer;
            EditorUtility.DisplayDialog($"{s} Copied!",
                "Code copied to clipboard, now you can paste it into your script.", "Ok");
        }
    }
}