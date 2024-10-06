using System;
using Better.Commons.EditorAddons.Drawers.Handlers;
using Better.Commons.EditorAddons.Extensions;
using Better.Commons.EditorAddons.Utility;
using Better.Commons.Runtime.Extensions;
using Better.Commons.Runtime.Utility;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Better.Attributes.EditorAddons.Drawers.DrawInspector
{
    [Serializable]
    public class DrawInspectorHandler : SerializedPropertyHandler
    {
        private SerializedProperty _property;
        private bool _isOpen = false;
        private VisualElement _rootElement;

        public DrawInspectorHandler()
        {
            _rootElement = new VisualElement();
        }

        private void UpdateVisible(VisualElement editorElement)
        {
            editorElement.style.SetVisible(_isOpen);
        }

        public void SetOpen(bool value)
        {
            _isOpen = value;
            UpdateVisible(_rootElement);
            ReorderableListUtility.RepaintAllInspectors(_property);
        }

        public bool IsOpen()
        {
            return _isOpen;
        }

        public VisualElement GetInspectorContainer(SerializedProperty property)
        {
            if (property == null)
            {
                DebugUtility.LogException<ArgumentException>(nameof(property));
                return null;
            }

            _property = property;
            return _rootElement;
        }

        public void SetupInspector()
        {
            _rootElement.Clear();

            if (_property == null || _property.IsDisposed())
            {
                DebugUtility.LogException(new ObjectDisposedException(nameof(_property)));
                return;
            }

            var referenceValue = _property.objectReferenceValue;
            if (referenceValue == null)
            {
                return;
            }

            var inspectorElement = new InspectorElement(_property.objectReferenceValue);
            UpdateVisible(_rootElement);
            _rootElement.Add(inspectorElement);
            _rootElement.style.PaddingLeft(StyleDefinition.IndentLevelPadding);
        }

        public bool CanOpen()
        {
            if (_property == null || _property.IsDisposed())
            {
                DebugUtility.LogException(new ObjectDisposedException(nameof(_property)));
                return false;
            }

            var referenceValue = _property.objectReferenceValue;
            if (referenceValue == null)
            {
                return false;
            }
            
            return true;
        }

        public override void Deconstruct()
        {
            _property = null;
        }
    }
}