using Better.Attributes.EditorAddons.Drawers.Preview;
using Better.Commons.EditorAddons.Drawers;
using Better.Commons.EditorAddons.Drawers.Base;
using UnityEditor;
using UnityEngine;

namespace Better.Attributes.EditorAddons.Drawers.WrapperCollections
{
    public class PreviewHandlers : HandlerCollection<PreviewHandler>
    {
        public void OpenPreviewWindow(Vector2 position, SerializedProperty property, float previewSize)
        {
            if (TryGetValue(property, out var wrapper))
            {
                wrapper.Wrapper.OpenPreviewWindow(property, position, previewSize);
            }
        }
        
        public void ClosePreviewWindow(SerializedProperty property)
        {
            if (TryGetValue(property, out var wrapper))
            {
                wrapper.Wrapper.ClosePreviewWindow();
            }
        }
        
        public void UpdatePropertyPreviewWindow(SerializedProperty property)
        {
            if (TryGetValue(property, out var wrapper))
            {
                wrapper.Wrapper.UpdatePropertyPreviewWindow(property);
            }
        }

        public void UpdatePreviewWindow(SerializedProperty property, Vector2 position)
        {
            if (TryGetValue(property, out var wrapper))
            {
                wrapper.Wrapper.UpdatePreviewWindow(position);
            }
        }

        public bool __ValidateObject(SerializedProperty property, ElementsContainer container)
        {
            if (TryGetValue(property, out var wrapper))
            {
               return wrapper.Wrapper.__ValidateObject(property, container);
            }

            return false;
        }
    }
}