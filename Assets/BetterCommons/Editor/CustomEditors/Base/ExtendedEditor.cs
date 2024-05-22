using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Better.Commons.EditorAddons.CustomEditors.Base
{
    /// <summary>
    /// Class represents base types for <see cref="MultiEditor"/>
    /// <remarks>Use <see cref="_target"/> and <see cref="_serializedObject"/> as cached fields</remarks>
    /// </summary>
    public abstract class ExtendedEditor
    {
        protected readonly Object _target;
        protected readonly SerializedObject _serializedObject;

        protected ExtendedEditor(Object target, SerializedObject serializedObject)
        {
            _target = target;
            _serializedObject = serializedObject;
        }

        /// <summary>
        /// This method called just right after instance created
        /// </summary>
        public abstract void OnEnable();

        /// <summary>
        /// Use to draw your inspector
        /// </summary>
        public abstract VisualElement CreateInspectorGUI();

        /// <summary>
        /// This method called than editor disables
        /// </summary>
        public abstract void OnDisable();

        /// <summary>
        /// Called when object data in editor is changed
        /// </summary>
        /// <param name="serializedObject"></param>
        public abstract void OnChanged(SerializedObject serializedObject);
    }
}