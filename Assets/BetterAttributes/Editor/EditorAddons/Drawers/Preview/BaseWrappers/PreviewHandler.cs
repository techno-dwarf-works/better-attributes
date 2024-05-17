using System.Threading;
using System.Threading.Tasks;
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
        private CancellationTokenSource _cancellation;
        private Vector2 _currentMousePosition;
        private Vector2 _currentScreenMousePosition;
        private protected readonly PreviewSceneRenderer _previewScene;
        
        private const string EmptyFieldMessage = "Preview not available for empty field";
        private const string PropertyInvalidMessage = "Property is not valid";

        private protected abstract void UpdateTexture();

        protected PreviewHandler()
        {
            _previewScene = new PreviewSceneRenderer();
        }
        
        public override void Deconstruct()
        {
            EditorPopup.CloseInstance();
            _previewScene?.Deconstruct();
            _cancellation?.Cancel(false);
            _cancellation = null;
        }

        private protected abstract Texture GenerateTexture(Object drawnObject, float size);
        

        public virtual void OpenPreviewWindow(Vector2 position, SerializedProperty serializedProperty, float size)
        {
            _previewScene.Construct();
            var texture = GenerateTexture(serializedProperty.objectReferenceValue, size);
            if (texture == null) return;
            var popup = EditorPopup.Initialize(texture,
                new Rect(_currentScreenMousePosition, Vector2.one * size), true);

            _cancellation = new CancellationTokenSource();
            UpdateTextureLoop(popup, _cancellation.Token);
            
            _currentScreenMousePosition = GUIUtility.GUIToScreenPoint(position);
        }

        public virtual void UpdatePreviewWindow(Vector2 position)
        {
            _currentScreenMousePosition = GUIUtility.GUIToScreenPoint(position);
        }

        public virtual void ClosePreviewWindow()
        {
            Deconstruct();
        }

        private async void UpdateTextureLoop(EditorPopup editorPopup, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                UpdateTexture();
                editorPopup.UpdatePosition(_currentScreenMousePosition);
                await Task.Yield();
                if (cancellationToken.IsCancellationRequested) break;
            }
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
    }
}