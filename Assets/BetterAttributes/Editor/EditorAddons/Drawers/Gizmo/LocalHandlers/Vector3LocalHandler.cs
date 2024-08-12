using System;
using Better.Commons.EditorAddons.Extensions;
using Better.Commons.Runtime.Extensions;
using UnityEditor;
using UnityEngine;

namespace Better.Attributes.EditorAddons.Drawers.Gizmo
{
    public class Vector3LocalHandler : GizmoHandler
    {
        private Vector3 _previousValue;

        public override void Apply(SceneView sceneView)
        {
            if (!ShowInSceneView) return;
            if (_serializedProperty.IsTargetComponent(out var component))
            {
                var transform = component.transform;
                var worldPosition = transform.TransformPoint(_previousValue);
                DrawLabel($"Local {GetName()}:\n{_previousValue}", worldPosition, _defaultRotation, sceneView);
                var buffer = transform.InverseTransformPoint(Handles.PositionHandle(worldPosition, _defaultRotation));
                
                if (!_previousValue.Approximately(buffer))
                {
                    _previousValue = buffer;
                    SetValueAndApply(_previousValue);
                }
            }
        }

        public override void SetProperty(SerializedProperty property, Type fieldType)
        {
            _previousValue = property.vector3Value;
            base.SetProperty(property, fieldType);
        }
    }
}