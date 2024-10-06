using System;
using Better.Commons.Runtime.Extensions;
using UnityEditor;
using UnityEngine;

namespace Better.Attributes.EditorAddons.Drawers.Gizmo
{
    public class Vector2Handler : GizmoHandler
    {
        private Vector2 _previousValue;

        public override void Apply(SceneView sceneView)
        {
            if (!ShowInSceneView) return;
            DrawLabel($"{GetName()}:\n{_previousValue}", _previousValue, _defaultRotation, sceneView);
            var buffer = Handles.PositionHandle(_previousValue, _defaultRotation);

            if (!_previousValue.Approximately(buffer))
            {
                _previousValue = buffer;
                SetValueAndApply(_previousValue);
            }
        }

        public override void SetProperty(SerializedProperty property, Type fieldType)
        {
            _previousValue = property.vector2Value;
            base.SetProperty(property, fieldType);
        }
    }
}