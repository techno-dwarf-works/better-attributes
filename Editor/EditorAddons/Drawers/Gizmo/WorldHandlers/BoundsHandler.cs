using Better.Commons.Runtime.Extensions;
using UnityEditor;

namespace Better.Attributes.EditorAddons.Drawers.Gizmo
{
    public class BoundsHandler : BoundsBaseHandler
    {
        public override void Apply(SceneView sceneView)
        {
            if (!ShowInSceneView) return;
            DrawLabel($"{GetName()}:\nCenter: {_previousValue.center}\nSize: {_previousValue.size}", _previousValue.center, _defaultRotation, sceneView);
            var center = Handles.PositionHandle(_previousValue.center, _defaultRotation);
            var size = DrawSize(_previousValue.center);
            ValidateSize(size);
            Handles.DrawWireCube(center, size);
            
            if (!_previousValue.center.Approximately(center) || !_previousValue.size.Approximately(size))
            {
                _previousValue.center = center;
                _previousValue.size = size;
                SetValueAndApply(_previousValue);
            }
        }
    }
}