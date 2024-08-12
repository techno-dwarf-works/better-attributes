using Better.Attributes.Runtime.Headers;
using UnityEditor;
using UnityEngine;

namespace Better.Attributes.EditorAddons.Drawers.Decorators
{
    [CustomPropertyDrawer(typeof(IconHeaderAttribute))]
    internal sealed class IconHeaderDrawer : DecoratorDrawer
    {
        private Texture _loadedTexture;
        private float _height;

        public override void OnGUI(Rect position)
        {
            position.yMin += EditorGUIUtility.singleLineHeight * 0.5f;
            position = EditorGUI.IndentedRect(position);
            if (!(attribute is IconHeaderAttribute iconHeaderAttribute)) return;
            if (_loadedTexture == null)
            {
                var path = AssetDatabase.GUIDToAssetPath(iconHeaderAttribute.Guid);
                _loadedTexture = AssetDatabase.LoadAssetAtPath<Texture>(path);
            }

            var texture = (Texture)Texture2D.whiteTexture;
            if (_loadedTexture != null)
            {
                texture = _loadedTexture;
            }

            var imageAspect = texture.width / (float)texture.height;
            _height = EditorGUIUtility.currentViewWidth / imageAspect;
            GUI.DrawTexture(position, texture, ScaleMode.ScaleToFit, iconHeaderAttribute.UseTransparency, imageAspect);
        }

        public override float GetHeight()
        {
            return _height;
        }
    }
}