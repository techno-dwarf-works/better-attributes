using Better.Commons.EditorAddons.Drawers;
using Better.Commons.EditorAddons.Drawers.Utility;
using Better.Commons.EditorAddons.EditorPopups;
using Better.Commons.EditorAddons.Extensions;
using Better.Commons.EditorAddons.Utility;
using Better.Commons.Runtime.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Better.Attributes.EditorAddons.Drawers.Preview
{
    public abstract class PreviewHandler : SerializedPropertyHandler
    {
        private protected readonly PreviewSceneRenderer _previewScene;
        private EditorPopup _editorPopup;
        private bool _isPopupOpened;
        private float _previewSize;

        private const string EmptyFieldMessage = "Preview not available for empty field";
        private const string PropertyInvalidMessage = "Property is not valid";

        private protected abstract void UpdateTexture();

        protected PreviewHandler()
        {
            _previewScene = new PreviewSceneRenderer();
        }

        public override void Deconstruct()
        {
            _isPopupOpened = false;
            _editorPopup?.ClosePopup();
            _previewScene?.Deconstruct();
        }

        private protected abstract Texture GenerateTexture(Object drawnObject, float size);


        public virtual void OpenPreviewWindow(SerializedProperty property, Vector2 position, float size)
        {
            if (_isPopupOpened)
            {
                Deconstruct();
                return;
            }

            _previewSize = size;
            _previewScene.Construct();
            var texture = GenerateTexture(property.objectReferenceValue, _previewSize);
            if (texture == null) return;
            var screenPoint = GUIUtility.GUIToScreenPoint(position);
            _editorPopup = EditorPopup.Initialize(texture, new Rect(screenPoint, Vector2.one * size)).SetUpdateAction(UpdateTexture);
            _isPopupOpened = true;
        }

        public virtual void UpdatePreviewWindow(Vector2 position)
        {
            if (!_isPopupOpened) return;
            var screenPoint = GUIUtility.GUIToScreenPoint(position);

            _editorPopup.UpdatePosition(screenPoint);
        }

        public virtual void ClosePreviewWindow()
        {
            Deconstruct();
        }

        public bool ValidateObject(SerializedProperty property, ElementsContainer container)
        {
            var isPropertyValid = property != null && property.Verify();

            var element = container.GetOrAddHelpBox(PropertyInvalidMessage, nameof(PropertyInvalidMessage), HelpBoxMessageType.Error);
            element.RootStyle.SetVisible(!isPropertyValid);

            if (!isPropertyValid)
            {
                return false;
            }

            return ValidateObject(property.objectReferenceValue, container);
        }

        protected virtual bool ValidateObject(Object drawnObject, ElementsContainer container)
        {
            var value = drawnObject != null;

            var element = container.GetOrAddHelpBox(EmptyFieldMessage, nameof(EmptyFieldMessage), HelpBoxMessageType.Warning);
            element.RootStyle.SetVisible(!value);

            return value;
        }

        public virtual void UpdatePropertyPreviewWindow(SerializedProperty property)
        {
            if (!_isPopupOpened) return;
            _previewScene.Construct();
            var texture = GenerateTexture(property.objectReferenceValue, _previewSize);
            _editorPopup.SetTexture(texture);
        }
    }
}