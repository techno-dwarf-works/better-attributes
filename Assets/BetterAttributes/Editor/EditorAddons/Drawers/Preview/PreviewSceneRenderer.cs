using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Better.Attributes.EditorAddons.Drawers.Preview
{
    public class PreviewSceneRenderer
    {
        private Camera _renderCamera;
        private float _cameraDistanceOffset = 0.7f;
        private float _cameraHeightOffset = 0.3f;
        private float _lightIntensity = 0.5f;
        private float _cameraAspect = 1f;
        private float _angleStep = 20f;
        private Vector3 _boundsCenter;
        private GameObject _root;
        private Texture _texture;
        private Scene _previewScene;

        public Scene PreviewScene => _previewScene;

        public void Deconstruct()
        {
            EditorSceneManager.ClosePreviewScene(_previewScene);
            Object.DestroyImmediate(_texture);
        }

        public void Construct()
        {
            _previewScene = EditorSceneManager.NewPreviewScene();
        }

        public Texture GenerateTexture(float size)
        {
            var intSize = Mathf.RoundToInt(size);
            _texture = new Texture2D(intSize, intSize, TextureFormat.RGBA32, false);

            _root = _previewScene.GetRootGameObjects().First();
            _renderCamera = CreateCamera(_previewScene, size);
            var objectBounds = GetSceneBound(_root);
            ConfigureLight(_previewScene, objectBounds);
            PositionCamera(_renderCamera, objectBounds);
            _boundsCenter = objectBounds.center;
            return _texture;
        }

        private void ConfigureLight(Scene scene, Bounds objectBounds)
        {
            var light = new GameObject().AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = _lightIntensity;
            light.color = Color.white;
            var max = objectBounds.max;
            light.transform.forward = (objectBounds.center - max).normalized;
            SceneManager.MoveGameObjectToScene(light.gameObject, scene);
        }

        private void PositionCamera(Camera camera, Bounds objectBounds)
        {
            var min = objectBounds.min;
            var max = objectBounds.max;
            var topCorner = new Vector3(min.x, max.y, max.z);
            var botCorner = new Vector3(min.x, min.y, max.z);
            var corner = topCorner - (topCorner - botCorner) * _cameraHeightOffset;
            var transform = camera.transform;
            var cameraDirection = (objectBounds.center - corner).normalized;
            var sizeMagnitude = (min - max).magnitude;
            transform.position = corner + (-cameraDirection * _cameraDistanceOffset * sizeMagnitude);
            transform.forward = cameraDirection;
        }

        private Bounds GetSceneBound(GameObject root)
        {
            var bounds = new Bounds();

            var renderers = root.GetComponentsInChildren<Renderer>(false);
            var center = renderers.Aggregate(Vector3.zero, (current, transform) => current + transform.bounds.center);
            bounds.center = center / renderers.Length;
            foreach (var renderer in renderers)
            {
                bounds.Encapsulate(renderer.bounds);
            }

            return bounds;
        }

        private Camera CreateCamera(Scene scene, float size)
        {
            var camera = new GameObject().AddComponent<Camera>();
            var intSize = Mathf.RoundToInt(size);
            camera.nearClipPlane = 0.1f;
            camera.aspect = _cameraAspect;
            camera.orthographicSize = size / 2f;
            camera.backgroundColor = Color.gray;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.targetTexture = new RenderTexture(intSize, intSize, 16);
            camera.cameraType = CameraType.Preview;
            camera.scene = scene;
            SceneManager.MoveGameObjectToScene(camera.gameObject, scene);
            return camera;
        }

        public void Render()
        {
            _renderCamera.Render();
            Graphics.CopyTexture(_renderCamera.targetTexture, _texture);
            _root.transform.RotateAround(_boundsCenter, Vector3.up, _angleStep * Time.deltaTime);
        }
    }
}