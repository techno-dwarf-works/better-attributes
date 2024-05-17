using System;
using Better.Commons.EditorAddons.Extensions;
using Better.Commons.Runtime.Extensions;
using UnityEditor;
using UnityEngine;

namespace Better.Attributes.EditorAddons.Drawers.Gizmo
{
    public class QuaternionLocalWrapper : GizmoWrapper
    {
        private const float Size = 1.1f;
        private Quaternion _quaternion;

        public override void Apply(SceneView sceneView)
        {
            if (!ShowInSceneView) return;
            if (_serializedProperty.IsTargetComponent(out var component))
            {
                var transform = component.transform;
                var rotation = transform.rotation;
                var position = transform.position;
                var worldRotation = (rotation * _quaternion);
                if (!worldRotation.IsNormalized())
                {
                    worldRotation = Quaternion.identity;
                }

                DrawLabel($"Local {GetName()}:\n{_quaternion.eulerAngles}", position, worldRotation, sceneView);
                var buffer = Quaternion.Inverse(rotation) * Handles.RotationHandle(worldRotation, position);

                Handles.ArrowHandleCap(GUIUtility.GetControlID(FocusType.Passive), position, worldRotation, Size * HandleUtility.GetHandleSize(position),
                    EventType.Repaint);

                if (!_quaternion.Approximately(buffer))
                {
                    _quaternion = buffer;
                    SetValueAndApply(_quaternion);
                }
            }
        }

        public override void SetProperty(SerializedProperty property, Type fieldType)
        {
            _quaternion = property.quaternionValue;
            base.SetProperty(property, fieldType);
        }

        private protected override Vector3 GetPosition(Vector3 position, Quaternion rotation, SceneView sceneView)
        {
            return (position + (rotation * Vector3.up) * HandleUtility.GetHandleSize(position) +
                    sceneView.camera.transform.right * 0.2f * HandleUtility.GetHandleSize(position));
        }
    }
}