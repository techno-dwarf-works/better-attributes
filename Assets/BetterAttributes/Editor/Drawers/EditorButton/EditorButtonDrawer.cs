using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Better.Attributes.EditorAddons.Drawers.Parameters;
using Better.Attributes.EditorAddons.Utilities;
using Better.Attributes.Runtime;
using Better.Commons.EditorAddons.Enums;
using Better.Commons.EditorAddons.Extensions;
using Better.Commons.EditorAddons.Utility;
using Better.Commons.Runtime.Extensions;
using Better.Commons.Runtime.UIElements;
using Better.Commons.Runtime.Utility;
using UnityEditor;
using UnityEngine.UIElements;

namespace Better.Attributes.EditorAddons.Drawers.EditorButton
{
    public class EditorButtonDrawer : VisualElement
    {
        private readonly SerializedObject _serializedObject;
        private Dictionary<int, IEnumerable<KeyValuePair<MethodInfo, EditorButtonAttribute>>> _buttons;
        private object _target;

        public EditorButtonDrawer(SerializedObject serializedObject)
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
            foreach (var (captureGroup, keyValuePairs) in methodAttributes)
            {
                if (captureGroup == -1)
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
                            var visualElement = CreateFromMethodInfo(group.methodInfo, attribute);
                            horizontalElement.Add(visualElement);
                        }
                    }
                }
                else
                {
                    var horizontalElement = VisualElementUtility.CreateHorizontalGroup();
                    Add(horizontalElement);
                    foreach (var (key, value) in keyValuePairs)
                    {
                        var visualElement = CreateFromMethodInfo(key, value);
                        horizontalElement.Add(visualElement);
                    }
                }
            }
        }

        private VisualElement CreateFromMethodInfo(MethodInfo methodInfo, EditorButtonAttribute attribute)
        {
            var parameters = methodInfo.GetParameters();
            foreach (var parameterInfo in parameters)
            {
                if (!ParameterFieldProvider.IsSupported(parameterInfo.ParameterType) || parameterInfo.IsOut)
                {
                    return CreateNotSupportedHelpBox(methodInfo, parameterInfo);
                }
            }

            return CreateButton(methodInfo, attribute);
        }

        private static HelpBox CreateNotSupportedHelpBox(MethodInfo methodInfo, ParameterInfo parameterInfo)
        {
            string message;
            if (parameterInfo.IsOut)
            {
                message = $"Parameter({parameterInfo.Name}) with \"out\" modificator in {methodInfo.Name} not supported";
            }
            else
            {
                message = $"Parameter({parameterInfo.Name}) with type {parameterInfo.ParameterType} in {methodInfo.Name} not supported";
            }

            var helpBox = VisualElementUtility.HelpBox(message, HelpBoxMessageType.Error);
            helpBox.style.FlexGrow(StyleDefinition.OneStyleFloat);
            return helpBox;
        }

        private VisualElement CreateButton(MethodInfo methodInfo, EditorButtonAttribute attribute)
        {
            var prettyMemberName = methodInfo.PrettyMemberName();

            var verticalGroup = VisualElementUtility.CreateVerticalGroup();
            verticalGroup.name = $"{prettyMemberName}__{nameof(verticalGroup)}";
            verticalGroup.style
                .FlexGrow(StyleDefinition.OneStyleFloat)
                .FlexBasis(0.5f);

            var horizontalGroup = VisualElementUtility.CreateHorizontalGroup();
            horizontalGroup.name = $"{prettyMemberName}__{nameof(horizontalGroup)}";
            horizontalGroup.style
                .FlexGrow(StyleDefinition.OneStyleFloat)
                .MaxHeight(StyleDefinition.ButtonHeight);

            verticalGroup.Add(horizontalGroup);

            var button = new Button
            {
                text = attribute.GetDisplayName(prettyMemberName),
                name = prettyMemberName
            };

            var parameters = methodInfo.GetParameters();
            var datas = parameters.Select(info => new Parameter(info)).ToArray();

            var parametersElement = new ParametersElementDrawer(datas);
            verticalGroup.Add(parametersElement);

            if (!datas.IsNullOrEmpty())
            {
                var toggle = new ToggleButton(value => parametersElement.style.SetVisible(value));
                toggle.AddIcon(IconType.GrayDropdown);
                toggle.style.Padding(1f);
                
                toggle.text = string.Empty;
                horizontalGroup.Add(toggle);
            }
            else
            {
                parametersElement.style.SetVisible(false);
            }

            horizontalGroup.Add(button);
            button.style.FlexGrow(StyleDefinition.OneStyleFloat);
            button.RegisterCallback<ClickEvent, MethodInfo, ParametersElementDrawer>(OnClick, methodInfo, parametersElement);
            return verticalGroup;
        }

        private void OnClick(ClickEvent clickEvent, (MethodInfo methodInfo, ParametersElementDrawer parameters) data)
        {
            _serializedObject.Update();
            var parameters = data.parameters;

            //TODO: Validate parameters count and types
            data.methodInfo.Invoke(_target, parameters.GetData());
            EditorUtility.SetDirty(_serializedObject.targetObject);
            _serializedObject.ApplyModifiedProperties();
        }
    }
}