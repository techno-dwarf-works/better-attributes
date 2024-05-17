using Better.Attributes.EditorAddons.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Better.Attributes.EditorAddons.Drawers.Preview
{
    public class TextureHandler : PreviewHandler
    {
        private protected override Texture GenerateTexture(Object drawnObject, float size)
        {
            Texture2D externalTexture = null;
            if (drawnObject is Texture texture)
            {
                externalTexture = Texture2D.CreateExternalTexture(texture.width, texture.height, TextureFormat.RGB24,
                    false, false,
                    texture.GetNativeTexturePtr());
            }

            if (drawnObject is Texture2D texture2D)
            {
                externalTexture = texture2D;
            }

            if (externalTexture != null)
            {
                CreateSpriteRenderer(externalTexture);
                return _previewScene.GenerateTexture(size);
            }

            return null;
        }

        private void CreateSpriteRenderer(Texture2D texture)
        {
            var spriteRenderer = new GameObject().AddComponent<SpriteRenderer>();
            var shader = Shader.Find(Constants.SpriteShaderName);
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