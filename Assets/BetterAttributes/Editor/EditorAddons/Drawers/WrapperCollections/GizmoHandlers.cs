using System;
using System.Collections.Generic;
using Better.Attributes.EditorAddons.Drawers.Gizmo;
using Better.Commons.EditorAddons.Drawers.Base;
using UnityEditor;

namespace Better.Attributes.EditorAddons.Drawers.WrapperCollections
{
    public class GizmoHandlers : HandlerCollection<GizmoWrapper>
    {
        public void Apply(SceneView sceneView)
        {
            List<SerializedProperty> keysToRemove = null;
            foreach (var gizmo in this)
            {
                var valueWrapper = gizmo.Value.Wrapper;
                if (valueWrapper.Validate())
                {
                    valueWrapper.Apply(sceneView);
                }
                else
                {
                    if (keysToRemove == null)
                    {
                        keysToRemove = new List<SerializedProperty>();
                    }

                    keysToRemove.Add(gizmo.Key);
                }
            }

            if (keysToRemove != null)
            {
                foreach (var property in keysToRemove)
                {
                    Remove(property);
                }
            }
        }

        public void SetProperty(SerializedProperty property, Type fieldType)
        {
            if (TryGetValue(property, out var gizmoWrapper))
            {
                gizmoWrapper.Wrapper.SetProperty(property, fieldType);
            }
        }

        public bool ShowInSceneView(SerializedProperty property)
        {
            if (TryGetValue(property, out var gizmoWrapper))
            {
                return gizmoWrapper.Wrapper.ShowInSceneView;
            }

            return false;
        }

        public void SetMode(SerializedProperty property, bool value)
        {
            if (TryGetValue(property, out var gizmoWrapper))
            {
                gizmoWrapper.Wrapper.SetMode(value);
            }
        }

        public bool IsValid(SerializedProperty property)
        {
            if (TryGetValue(property, out var gizmoWrapper))
            {
                return gizmoWrapper.Wrapper.Validate();
            }

            return false;
        }
    }
}