using Better.Commons.Runtime.Extensions;
using UnityEditor;

namespace Better.Attributes.EditorAddons.Drawers.Gizmo
{
    public class BoundsWrapper : BoundsBaseWrapper
    {
        public override void Apply(SceneView sceneView)
        {
            if (!ShowInSceneView) return;
            DrawLabel($"{GetName()}:\nCenter: {_bounds.center}\nSize: {_bounds.size}", _bounds.center, _defaultRotation, sceneView);
            var center = Handles.PositionHandle(_bounds.center, _defaultRotation);
            var size = DrawSize(_bounds.center);
            ValidateSize(size);
            Handles.DrawWireCube(center, size);
            
            if (!_bounds.center.Approximately(center) || !_bounds.size.Approximately(size))
            {
                _bounds.center = center;
                _bounds.size = size;
                SetValueAndApply(_bounds);
            }
        }
    }
}