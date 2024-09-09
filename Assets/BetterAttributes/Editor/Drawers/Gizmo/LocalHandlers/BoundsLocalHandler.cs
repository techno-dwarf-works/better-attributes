using Better.Commons.EditorAddons.Extensions;
using Better.Commons.Runtime.Utility;
using UnityEditor;
using UnityEngine;

namespace Better.Attributes.EditorAddons.Drawers.Gizmo
{
    public class BoundsLocalHandler : BoundsBaseHandler
    {
        public override void Apply(SceneView sceneView)
        {
            if (!ShowInSceneView) return;
            if (_serializedProperty.IsTargetComponent(out var component))
            {
                var transform = component.transform;
                var worldPosition = transform.TransformPoint(_previousValue.center);
                DrawLabel($"{GetName()}:\nLocal Center: {_previousValue.center}\nSize: {_previousValue.size}", worldPosition, _defaultRotation, sceneView);
                var center = transform.InverseTransformPoint(Handles.PositionHandle(worldPosition, Quaternion.identity));
                var size = DrawSize(worldPosition);
                ValidateSize(size);
                Handles.DrawWireCube(worldPosition, size);

                if (!Vector3Utility.Approximately(_previousValue.center, center) || !Vector3Utility.Approximately(_previousValue.size, size))
                {
                    _previousValue.center = center;
                    _previousValue.size = size;
                    SetValueAndApply(_previousValue);
                }
            }
        }
    }
}