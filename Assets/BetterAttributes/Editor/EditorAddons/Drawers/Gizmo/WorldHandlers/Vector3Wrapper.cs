using System;
using Better.Commons.Runtime.Extensions;
using UnityEditor;
using UnityEngine;

namespace Better.Attributes.EditorAddons.Drawers.Gizmo
{
    public class Vector3Wrapper : GizmoWrapper
    {
        private Vector3 _vector3;

        public override void Apply(SceneView sceneView)
        {
            if (!ShowInSceneView) return;
            DrawLabel($"{GetName()}:\n{_vector3}", _vector3, _defaultRotation, sceneView);
            var buffer = Handles.PositionHandle(_vector3, _defaultRotation);
            
            if (!_vector3.Approximately(buffer))
            {
                _vector3 = buffer;
                SetValueAndApply(_vector3);
            }
        }

        public override void SetProperty(SerializedProperty property, Type fieldType)
        {
            _vector3 = property.vector3Value;
            base.SetProperty(property, fieldType);
        }
    }
}