using Better.Attributes.EditorAddons.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Better.Attributes.EditorAddons.Drawers.Preview
{
    public class TextureHandler : PreviewHandler
    {
        private protected override Texture GenerateTexture(Object drawnObject, float size)
        {
            var externalTexture = drawnObject switch
            {
                Texture2D texture2D => texture2D,
                Texture texture => Texture2D.CreateExternalTexture(
                    texture.width,
                    texture.height,
                    TextureFormat.RGB24,
                    false,
                    false,
                    texture.GetNativeTexturePtr()),
                _ => null
            };

            if (externalTexture == null)
            {
                return null;
            }

            CreateSpriteRenderer(externalTexture);
            return _previewScene.GenerateTexture(size);
        }

        private void CreateSpriteRenderer(Texture2D texture)
        {
            var spriteRenderer = new GameObject().AddComponent<SpriteRenderer>();
            var shader = Shader.Find(AttributesDefinitions.SpriteShaderName);
            spriteRenderer.material = new Material(shader);
            var textureSize = new Vector2(texture.width, texture.height);
            spriteRenderer.sprite = Sprite.Create(texture, new Rect(Vector2.zero, textureSize), Vector2.one / 2f);
            SceneManager.MoveGameObjectToScene(spriteRenderer.gameObject, _previewScene.PreviewScene);
        }

        private protected override void UpdateTexture()
        {
            _previewScene.Render();
        }
    }
}