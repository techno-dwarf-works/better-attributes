﻿using System;
using Better.Attributes.Runtime.Gizmo;
using Better.Commons.EditorAddons.Drawers;
using Better.Commons.EditorAddons.Drawers.HandlerBinding;
using Better.Commons.Runtime.Extensions;
using UnityEditor;
using UnityEngine;

namespace Better.Attributes.EditorAddons.Drawers.Gizmo
{
    [HandlerBinding(typeof(Vector3), typeof(GizmoAttribute))]
    public class Vector3Handler : GizmoHandler
    {
        private Vector3 _previousValue;

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
            _previousValue = property.vector3Value;
            base.SetProperty(property, fieldType);
        }
    }
}