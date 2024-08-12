using Better.Attributes.EditorAddons.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Better.Attributes.EditorAddons.Drawers.Preview
{
    public class SpriteHandler : PreviewHandler
    {
        private protected override Texture GenerateTexture(Object drawnObject, float size)
        {
            if (drawnObject is Sprite sprite)
            {
                var spriteRenderer = new GameObject().AddComponent<SpriteRenderer>();
                var shader = Shader.Find(AttributesDefinitions.SpriteShaderName);
                spriteRenderer.material = new Material(shader);
                spriteRenderer.sprite = sprite;
                SceneManager.MoveGameObjectToScene(spriteRenderer.gameObject, _previewScene.PreviewScene);
                return _previewScene.GenerateTexture(size);
            }

            return null;
        }

        private protected override void UpdateTexture()
        {
            _previewScene.Render();
        }
    }
}