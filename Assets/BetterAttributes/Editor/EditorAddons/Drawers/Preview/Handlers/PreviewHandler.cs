using Better.Commons.EditorAddons.Drawers;
using Better.Commons.EditorAddons.Drawers.Container;
using Better.Commons.EditorAddons.Drawers.Handlers;
using Better.Commons.EditorAddons.EditorPopups;
using Better.Commons.EditorAddons.Extensions;
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
        private bool _isValid;

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

            if (!_isValid)
            {
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
            if (!_isValid)
            {
                return;
            }

            if (!_isPopupOpened) return;
            var screenPoint = GUIUtility.GUIToScreenPoint(position);

            _editorPopup.UpdatePosition(screenPoint);
        }

        public virtual void ClosePreviewWindow()
        {
            Deconstruct();
        }

        public void ProcessElementsContainer(ElementsContainer container)
        {
            var property = container.SerializedProperty;
            var isPropertyValid = property != null && property.Verify();

            var element = container.GetOrAddHelpBox(PropertyInvalidMessage, nameof(PropertyInvalidMessage), HelpBoxMessageType.Error);
            element.style.SetVisible(!isPropertyValid);

            if (!isPropertyValid)
            {
                _isValid = false;
                return;
            }

            _isValid = ProcessElementsContainer(property.objectReferenceValue, container);
        }

        protected virtual bool ProcessElementsContainer(Object drawnObject, ElementsContainer container)
        {
            var value = drawnObject != null;

            var element = container.GetOrAddHelpBox(EmptyFieldMessage, nameof(EmptyFieldMessage), HelpBoxMessageType.Warning);
            element.style.SetVisible(!value);

            return value;
        }

        public virtual void UpdatePropertyPreviewWindow(ElementsContainer container)
        {
            ProcessElementsContainer(container);
            
            if (!_isValid)
            {
                return;
            }

            if (!_isPopupOpened)
            {
                return;
            }
            var property = container.SerializedProperty;
            _previewScene.Construct();
            var texture = GenerateTexture(property.objectReferenceValue, _previewSize);
            _editorPopup.SetTexture(texture);
        }
    }
}