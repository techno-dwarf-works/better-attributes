using System;
using UnityEditor;
using UnityEngine;

namespace Better.Attributes.EditorAddons.Drawers.Gizmo
{
    public abstract class BoundsBaseHandler : GizmoHandler
    {
        private protected Bounds _previousValue;

        private protected Vector3 DrawSize(Vector3 position)
        {
            return Handles.ScaleHandle(_previousValue.size, position, Quaternion.identity,
                HandleUtility.GetHandleSize(position) * 0.7f);
        }

        private protected void ValidateSize(Vector3 size)
        {
            for (int i = 0; i < 3; i++)
            {
                if (size[i] <= 0)
                {
                    var boundsSize = size;
                    boundsSize[i] = 0.01f;
                    size = boundsSize;
                }
            }
        }

        public override void SetProperty(SerializedProperty property, Type fieldType)
        {
            _previousValue = property.boundsValue;
            base.SetProperty(property, fieldType);

            if (_previousValue.size == Vector3.zero)
            {
                _previousValue.size = Vector3.one / 10f;
                SetValueAndApply(_previousValue);
            }
        }
    }
}