using System;
using UnityEditor;
using UnityEngine;

namespace Better.Commons.EditorAddons.EditorPopups
{
    public class EditorPopup : EditorWindow
    {
        private Texture _texture;
        private bool _needUpdate = true;
        private bool _destroyTexture;
        private Action _updateAction;
        public event Action Closed;
        public event Action FocusLost;
        public event Action Destroyed;

        public static EditorPopup Initialize(Texture texture, Rect position, bool destroyTexture = false)
        {
            var window = HasOpenInstances<EditorPopup>() ? GetWindow<EditorPopup>() : CreateInstance<EditorPopup>();
            return Initialize(texture, position, destroyTexture, window);
        }

        private static EditorPopup Initialize(Texture texture, Rect position, bool destroyTexture, EditorPopup window)
        {
            window.position = position;
            window._texture = texture;
            window._needUpdate = false;
            window._destroyTexture = destroyTexture;
            window.ShowPopup();
            return window;
        }

        public static EditorPopup InitializeAsWindow(Texture texture, Rect position, bool destroyTexture = false)
        {
            var window = HasOpenInstances<EditorPopup>() ? GetWindow<EditorPopup>() : CreateInstance<EditorPopup>();
            window.position = position;
            window.SetTexture(texture);
            window._needUpdate = false;
            window._destroyTexture = destroyTexture;
            window.ShowUtility();
            return window;
        }

        public EditorPopup SetUpdateAction(Action action)
        {
            _updateAction = action;
            _needUpdate = true;
            return this;
        }

        private void Update()
        {
            if (_needUpdate)
            {
                _updateAction?.Invoke();
                Repaint();
            }
        }

        private void OnGUI()
        {
            if (_texture != null)
            {
                GUI.DrawTexture(new Rect(0, 0, position.width, position.height), _texture, ScaleMode.ScaleToFit, true);
            }
        }

        private void OnLostFocus()
        {
            FocusLost?.Invoke();
        }

        private void OnDestroy()
        {
            Destroyed?.Invoke();
        }

        public static void CloseInstance()
        {
            if (!HasOpenInstances<EditorPopup>()) return;
            var window = GetWindow<EditorPopup>();
            window.ClosePopup();
        }

        public void UpdatePosition(Vector2 newPosition)
        {
            var rect = position;
            rect.position = newPosition;
            position = rect;
        }

        public void SetTexture(Texture texture)
        {
            _texture = texture;
        }

        public void ClosePopup()
        {
            OnClosePopup();
            Close();
        }

        private void OnClosePopup()
        {
            Closed?.Invoke();
            if (_destroyTexture && _texture)
            {
                DestroyImmediate(_texture);
            }
        }
    }
}