using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Better.Attributes.Runtime;
using Better.Commons.EditorAddons.CustomEditors.Attributes;
using Better.Commons.EditorAddons.CustomEditors.Base;
using Better.Commons.EditorAddons.Extensions;
using Better.Commons.EditorAddons.Utility;
using Better.Commons.Runtime.Extensions;
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

        private Button DrawButton(MethodInfo methodInfo, EditorButtonAttribute attribute)
        {
            var button = new Button
            {
                text = attribute.GetDisplayName(methodInfo.PrettyMemberName()),
                name = methodInfo.PrettyMemberName()
            };
            button.style.FlexGrow(StyleDefinition.OneStyleFloat);
            button.RegisterCallback<ClickEvent, (MethodInfo, EditorButtonAttribute)>(OnClick, (methodInfo, attribute));
            return button;
        }

        private void OnClick(ClickEvent clickEvent, (MethodInfo methodInfo, EditorButtonAttribute attribute) data)
        {
            _serializedObject.Update();
            data.methodInfo.Invoke(_target, data.attribute.InvokeParams);
            EditorUtility.SetDirty(_target);
            _serializedObject.ApplyModifiedProperties();
        }


        private VisualElement DrawButtons(Dictionary<int, IEnumerable<KeyValuePair<MethodInfo, EditorButtonAttribute>>> buttons)
        {
            var container = new VisualElement();

            foreach (var button in buttons)
            {
                if (button.Key == -1)
                {
                    var grouped = button.Value.GroupBy(key => key.Key, pair => pair.Value,
                        (info, attributes) => new KeyValuePair<MethodInfo, IEnumerable<EditorButtonAttribute>>(info, attributes));
                    var verticalElement = VisualElementUtility.CreateVerticalGroup();
                    container.Add(verticalElement);

                    foreach (var group in grouped)
                    {
                        var horizontalElement = VisualElementUtility.CreateHorizontalGroup();
                        verticalElement.Add(horizontalElement);

                        foreach (var attribute in group.Value)
                        {
                            var buttonElement = DrawButton(group.Key, attribute);
                            horizontalElement.Add(buttonElement);
                        }
                    }
                }
                else
                {
                    var horizontalElement = VisualElementUtility.CreateHorizontalGroup();
                    container.Add(horizontalElement);
                    foreach (var (key, value) in button.Value)
                    {
                        var element = DrawButton(key, value);
                        horizontalElement.Add(element);
                    }
                }
            }

            return container;
        }

        public override VisualElement CreateInspectorGUI()
        {
            var buttons = DrawButtons(_methodButtonsAttributes);
            return buttons;
        }

        public override void OnChanged(SerializedObject serializedObject)
        {
        }
    }
}