using System;
using Better.Commons.Runtime.Extensions;
using UnityEditor;
using UnityEngine;

namespace Better.Attributes.EditorAddons.Drawers.Gizmo
{
    public class QuaternionHandler : GizmoHandler
    {
        private const float Size = 1.1f;
        private Quaternion _previousValue;

        public override void Apply(SceneView sceneView)
        {
            if (!ShowInSceneView) return;
            DrawLabel($"{GetName()}:\n{_previousValue.eulerAngles}", _defaultPosition, _previousValue, sceneView);
            if (!_previousValue.IsNormalized())
            {
                _previousValue = Quaternion.identity;
            }

            var buffer = Handles.RotationHandle(_previousValue, _defaultPosition);

            Handles.ArrowHandleCap(GUIUtility.GetControlID(FocusType.Passive), _defaultPosition, buffer, Size * HandleUtility.GetHandleSize(_defaultPosition),
                EventType.Repaint);

            if (!_previousValue.Approximately(buffer))
            {
                _previousValue = buffer;
                SetValueAndApply(_previousValue);
            }
        }

        public override void SetProperty(SerializedProperty property, Type fieldType)
        {
            _previousValue = property.quaternionValue;

            var num = Mathf.Sqrt(Quaternion.Dot(_previousValue, _previousValue));
            if (num < (double)Mathf.Epsilon)
            {
                _previousValue = _defaultRotation;
            }

            base.SetProperty(property, fieldType);
        }
    }
}