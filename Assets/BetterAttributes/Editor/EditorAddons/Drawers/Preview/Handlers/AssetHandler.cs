using Better.Commons.EditorAddons.Drawers;
using Better.Commons.EditorAddons.Enums;
using Better.Commons.EditorAddons.Utility;
using Better.Commons.Runtime.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Better.Attributes.EditorAddons.Drawers.Preview
{
    public class AssetHandler : PreviewHandler
    {
        private const string ObjectNotSupportedMessage = "Object is not Component";
        private const string NoRenderersMessage = "Preview not available for objects without {0} in children";

        protected override bool ValidateObject(Object drawnObject, ElementsContainer container)
        {
            var baseValid = base.ValidateObject(drawnObject, container);
            if (!baseValid)
            {
                return false;
            }

            var element = container.GetOrAddHelpBox(ObjectNotSupportedMessage, nameof(ObjectNotSupportedMessage), HelpBoxMessageType.Error);
            
            if (drawnObject is not Component component)
            {
                element.RootStyle.SetVisible(true);
                return false;
            }
            
            element.RootStyle.SetVisible(false);

            var hasRenderers = component.gameObject.GetComponentsInChildren<Renderer>().Length > 0;
            var renderersMessage = string.Format(NoRenderersMessage, VisualElementUtility.FormatBold($"{nameof(Renderer)}s"));
            
            element = container.GetOrAddHelpBox(renderersMessage, nameof(NoRenderersMessage), HelpBoxMessageType.Error);
            element.RootStyle.SetVisible(!hasRenderers);
            
            return hasRenderers;
        }

        private protected override Texture GenerateTexture(Object drawnObject, float size)
        {
            var gameObject = (drawnObject as Component)?.gameObject;
            if (PrefabUtility.IsPartOfPrefabAsset(drawnObject))
            {
                var path = AssetDatabase.GetAssetPath(gameObject);
                PrefabUtility.LoadPrefabContentsIntoPreviewScene(path, _previewScene.PreviewScene);
            }
            else
            {
                var copy = Object.Instantiate(gameObject);
                SceneManager.MoveGameObjectToScene(copy, _previewScene.PreviewScene);
            }

            return _previewScene.GenerateTexture(size);
        }

        private protected override void UpdateTexture()
        {
            _previewScene.Render();
        }
    }
}