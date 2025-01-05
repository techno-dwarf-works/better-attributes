using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Better.Commons.EditorAddons.Drawers.Proxies;
using Better.Attributes.EditorAddons.Utilities;
using Better.Attributes.Runtime;
using Better.Commons.EditorAddons.Utility;
using Better.Commons.Runtime.Extensions;
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
                if (!ProxyProvider.IsSupported(parameterInfo.ParameterType) || parameterInfo.IsOut)
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

            var button = new Button
            {
                text = attribute.GetDisplayName(prettyMemberName),
                name = prettyMemberName
            };

            button.style.FlexGrow(StyleDefinition.OneStyleFloat);

            var parameters = methodInfo.GetParameters();

            if (parameters.IsNullOrEmpty())
            {
                button.RegisterCallback<ClickEvent, MethodInfo>(OnClick, methodInfo);
                return button;
            }

            var datas = parameters.Select(info => new ParameterProxy(info));

            var foldout = SetupFoldout(prettyMemberName);

            var views = CreateViews(datas, foldout);
            
            SetupFoldoutToggle(foldout, button);

            var group = views.Select(view => view.style).AsGroup();
            
            foldout.RegisterCallback<ChangeEvent<bool>, IStyle>(OnFoldout, group);
            button.RegisterCallback<ClickEvent, MethodInfo, List<ProxyView<object>>>(OnClick, methodInfo, views);
            
            return foldout;
        }

        private Foldout SetupFoldout(string prettyMemberName)
        {
            var foldout = new Foldout();
            foldout.name = $"{prettyMemberName}__{nameof(foldout)}";
            foldout.style
                .FlexGrow(StyleDefinition.OneStyleFloat)
                .FlexBasis(0.5f)
                .FlexDirection(FlexDirection.Column);
            
            foldout.contentContainer.AddToClassList(HelpBox.ussClassName);
            foldout.contentContainer.style
                .FlexDirection(FlexDirection.Column)
                .AlignItems(Align.Stretch);
            return foldout;
        }

        private List<ProxyView<object>> CreateViews(IEnumerable<ParameterProxy> datas, Foldout foldout)
        {
            var views = new List<ProxyView<object>>();
            foreach (var parameterProxy in datas)
            {
                var parametersElement = new ProxyView<ParameterProxy, object>(parameterProxy);
                parametersElement.style.FlexGrow(StyleDefinition.OneStyleFloat);
                views.Add(parametersElement);
                foldout.contentContainer.Add(parametersElement);
            }

            return views;
        }

        private void SetupFoldoutToggle(Foldout foldout, Button button)
        {
            var foldoutToggle = foldout.Q<Toggle>();
            foldoutToggle.Add(button);
            foldoutToggle.style.Margin(StyleDefinition.ZeroStyleLength);
            
            var toggleCheckmark = foldout.Q<VisualElement>(className: Toggle.inputUssClassName);
            toggleCheckmark.AddToClassList(Button.ussClassName);
            toggleCheckmark.style.FlexGrow(StyleDefinition.ZeroStyleFloat);
            toggleCheckmark.Children().Select(child => child.style).AsGroup().Margin(StyleDefinition.OneStyleLength);
        }

        private void OnFoldout(ChangeEvent<bool> evt, IStyle styleGroup)
        {
            var value = evt.newValue;
            styleGroup.SetVisible(value);
        }

        private void OnClick(ClickEvent clickEvent, (MethodInfo methodInfo, List<ProxyView<object>> views) data)
        {
            _serializedObject.Update();

            var parameters = data.views.Select(view => view.value).ToArray();
            data.methodInfo.Invoke(_target, parameters);
            EditorUtility.SetDirty(_serializedObject.targetObject);
            _serializedObject.ApplyModifiedProperties();
        }

        private void OnClick(ClickEvent clickEvent, MethodInfo methodInfo)
        {
            _serializedObject.Update();

            methodInfo.Invoke(_target, null);
            EditorUtility.SetDirty(_serializedObject.targetObject);
            _serializedObject.ApplyModifiedProperties();
        }
    }
}