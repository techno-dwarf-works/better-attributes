using System;
using Better.Commons.EditorAddons.Extensions;
using Better.Commons.Runtime.Extensions;
using UnityEditor;
using UnityEngine;

namespace Better.Attributes.EditorAddons.Drawers.Gizmo
{
    public class Vector2LocalWrapper : GizmoWrapper
    {
        private Vector2 _vector2;

        public override void Apply(SceneView sceneView)
        {
            if (!ShowInSceneView) return;
            if (_serializedProperty.IsTargetComponent(out var component))
            {
                var transform = component.transform;
                var worldPosition = transform.TransformPoint(_vector2);
                DrawLabel($"Local {GetName()}:\n{_vector2}", _vector2, _defaultRotation, sceneView);
                var buffer = transform.InverseTransformPoint(Handles.PositionHandle(worldPosition, _defaultRotation));
                
                if (!_vector2.Approximately(buffer))
                {
                    _vector2 = buffer;
                    SetValueAndApply(_vector2);
                }
            }
        }

        public override void SetProperty(SerializedProperty property, Type fieldType)
        {
            _vector2 = property.vector2Value;
            base.SetProperty(property, fieldType);
        }
    }
}