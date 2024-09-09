using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Better.Attributes.Runtime;
using Better.Commons.EditorAddons.Utility;
using Better.Commons.Runtime.Extensions;
using Better.Commons.Runtime.Utility;
using UnityEditor;
using UnityEngine.UIElements;

namespace Better.Attributes.EditorAddons.Helpers
{
    public class EditorButtonContainer : VisualElement
    {
        private readonly SerializedObject _serializedObject;
        private Dictionary<int, IEnumerable<KeyValuePair<MethodInfo, EditorButtonAttribute>>> _buttons;
        private object _target;

        public EditorButtonContainer(SerializedObject serializedObject)
        {
            _serializedObject = serializedObject;
        }

        public void CreateFromTarget(object target)
        {
            if (target == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(target));
                return;
            }

            _target = target;
            Clear();
            var type = _target.GetType();
            Add(type);
        }

        private void Add(Type targetType)
        {
            var methodAttributes = EditorButtonUtility.GetSortedMethodAttributes(targetType);
            foreach (var (priority, keyValuePairs) in methodAttributes)
            {
                if (priority == -1)
                {
                    var grouped = keyValuePairs.GroupBy(key => key.Key, pair => pair.Value, (methodInfo, attributes) => (methodInfo, attributes));
                    var verticalElement = VisualElementUtility.CreateVerticalGroup();
                    Add(verticalElement);

                    foreach (var group in grouped)
                    {
                        var horizontalElement = VisualElementUtility.CreateHorizontalGroup();
                        verticalElement.Add(horizontalElement);

                        foreach (var attribute in group.attributes)
                        {
                            var buttonElement = DrawButton(group.methodInfo, attribute);
                            horizontalElement.Add(buttonElement);
                        }
                    }
                }
                else
                {
                    var horizontalElement = VisualElementUtility.CreateHorizontalGroup();
                    Add(horizontalElement);
                    foreach (var (key, value) in keyValuePairs)
                    {
                        var element = DrawButton(key, value);
                        horizontalElement.Add(element);
                    }
                }
            }
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
            EditorUtility.SetDirty(_serializedObject.targetObject);
            _serializedObject.ApplyModifiedProperties();
        }
    }
}