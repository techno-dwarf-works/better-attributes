using System.Collections.Generic;
using System.Reflection;
using Better.Attributes.EditorAddons.Helpers;
using Better.Attributes.Runtime;
using Better.Commons.EditorAddons.CustomEditors.Attributes;
using Better.Commons.EditorAddons.CustomEditors.Base;
using Better.Commons.EditorAddons.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Better.Attributes.EditorAddons.CustomEditors
{
    [MultiEditor(typeof(Object), true, Order = 999)]
    public class ButtonsEditor : ExtendedEditor
    {
        private Dictionary<int, IEnumerable<KeyValuePair<MethodInfo, EditorButtonAttribute>>> _methodButtonsAttributes =
            new Dictionary<int, IEnumerable<KeyValuePair<MethodInfo, EditorButtonAttribute>>>();

        public ButtonsEditor(Object target, SerializedObject serializedObject) : base(target, serializedObject)
        {
        }

        public override void OnDisable()
        {
        }

        public override void OnEnable()
        {
            var type = _target.GetType();
            _methodButtonsAttributes = EditorButtonUtility.GetSortedMethodAttributes(type);
        }


        public override VisualElement CreateInspectorGUI()
        {
            var container = new EditorButtonContainer(_serializedObject);
            container.CreateFromTarget(_target);
            return container;
        }

        public override void OnChanged(SerializedObject serializedObject)
        {
        }
    }
}