using Better.Attributes.EditorAddons.Drawers.DrawInspector;
using Better.Commons.EditorAddons.Drawers.Base;
using UnityEditor;
using UnityEngine.UIElements;

namespace Better.Attributes.EditorAddons.Drawers.WrapperCollections
{
    public class DrawInspectorCollection : HandlerCollection<DrawInspectorHandler>
    {
        public void SetOpen(SerializedProperty serializedProperty, bool value)
        {
            if (TryGetValue(serializedProperty, out var collectionValue))
            {
                collectionValue.Wrapper.SetOpen(value);
            }
        }

        public bool IsOpen(SerializedProperty serializedProperty)
        {
            if (TryGetValue(serializedProperty, out var collectionValue))
            {
                return collectionValue.Wrapper.IsOpen();
            }

            return false;
        }

        public VisualElement GetInspectorContainer(SerializedProperty property)
        {
            if (TryGetValue(property, out var wrapper))
            {
                return wrapper.Wrapper.GetInspectorContainer(property);
            }

            return null;
        }

        public void SetupInspector(SerializedProperty property)
        {
            if (TryGetValue(property, out var wrapper))
            {
                wrapper.Wrapper.SetupInspector();
            }
        }
    }
}