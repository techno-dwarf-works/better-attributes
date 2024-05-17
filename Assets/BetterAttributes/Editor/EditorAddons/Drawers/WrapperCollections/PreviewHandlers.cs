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
                wrapper.Wrapper.OpenPreviewWindow(position, property, previewSize);
            }
        }
        
        public void ClosePreviewWindow(SerializedProperty property)
        {
            if (TryGetValue(property, out var wrapper))
            {
                wrapper.Wrapper.ClosePreviewWindow();
            }
        }

        public void UpdatePreviewWindow(SerializedProperty property, Vector2 position)
        {
            if (TryGetValue(property, out var wrapper))
            {
                wrapper.Wrapper.UpdatePreviewWindow(position);
            }
        }

        public bool ValidateObject(SerializedProperty property, ElementsContainer container)
        {
            if (TryGetValue(property, out var wrapper))
            {
               return wrapper.Wrapper.ValidateObject(property, container);
            }

            return false;
        }
    }
}