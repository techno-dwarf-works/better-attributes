using Better.Commons.EditorAddons.Extensions;
using Better.Commons.Runtime.Utility;
using UnityEditor;
using UnityEngine;

namespace Better.Attributes.EditorAddons.Drawers.Gizmo
{
    public class BoundsLocalWrapper : BoundsBaseWrapper
    {
        public override void Apply(SceneView sceneView)
        {
            if (!ShowInSceneView) return;
            if (_serializedProperty.IsTargetComponent(out var component))
            {
                var transform = component.transform;
                var worldPosition = transform.TransformPoint(_bounds.center);
                DrawLabel($"{GetName()}:\nLocal Center: {_bounds.center}\nSize: {_bounds.size}", worldPosition, _defaultRotation, sceneView);
                var center = transform.InverseTransformPoint(Handles.PositionHandle(worldPosition, Quaternion.identity));
                var size = DrawSize(worldPosition);
                ValidateSize(size);
                Handles.DrawWireCube(worldPosition, size);

                if (!Vector3Utility.Approximately(_bounds.center, center) || !Vector3Utility.Approximately(_bounds.size, size))
                {
                    _bounds.center = center;
                    _bounds.size = size;
                    SetValueAndApply(_bounds);
                }
            }
        }
    }
}