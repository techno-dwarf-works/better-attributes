using System;
using Better.Commons.EditorAddons.Drawers.Utility;
using Better.Commons.EditorAddons.Extensions;
using Better.Commons.EditorAddons.Utility;
using Better.Commons.Runtime.Extensions;
using Better.Commons.Runtime.Utility;
using UnityEditor;
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

            if (_property.IsDisposed())
            {
                DebugUtility.LogException(new ObjectDisposedException(nameof(_property)));
                return;
            }

            if (_property.objectReferenceValue == null)
            {
                return;
            }

            var editor = Editor.CreateEditor(_property.objectReferenceValue);
            var editorElement = editor.CreateInspectorGUI();
            UpdateVisible(_rootElement);
            _rootElement.Add(editorElement);
        }

        public override void Deconstruct()
        {
            _property = null;
        }
    }
}