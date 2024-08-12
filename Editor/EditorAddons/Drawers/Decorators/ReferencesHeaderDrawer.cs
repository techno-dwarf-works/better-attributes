using Better.Attributes.Runtime.Headers;
using UnityEditor;
using UnityEngine;

namespace Better.Attributes.EditorAddons.Drawers.Decorators
{
    [CustomPropertyDrawer(typeof(ReferencesHeaderAttribute))]
    internal sealed class ReferencesHeaderDrawer : DecoratorDrawer
    {
        public override void OnGUI(Rect position)
        {
            position.yMin += EditorGUIUtility.singleLineHeight * 0.5f;
            position = EditorGUI.IndentedRect(position);
            GUI.Label(position, (attribute as HeaderAttribute)?.header, EditorStyles.boldLabel);
        }

        public override float GetHeight()
        {
            return EditorGUIUtility.singleLineHeight * 1.5f;
        }
    }
}