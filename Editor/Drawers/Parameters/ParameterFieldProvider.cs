using System;
using System.Collections.Generic;
using System.Linq;
using Better.Commons.EditorAddons.Utility;
using Better.Commons.Runtime.Extensions;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Better.Attributes.EditorAddons.Drawers.Parameters
{
    internal static class ParameterFieldProvider
    {
        private const string ObjectFieldDisplayClassName = "unity-object-field-display";
        private static List<FieldFactory> _factories;

        static ParameterFieldProvider()
        {
            _factories = new List<FieldFactory>();

            _factories.Add(SupportDirectType<int>, ConfigureIntegerField);

            _factories.Add(SupportDirectType<long>, ConfigureField<LongField, long>);

            _factories.Add(SupportDirectType<bool>, ConfigureField<Toggle, bool>);

            _factories.Add(SupportDirectType<float>, ConfigureField<FloatField, float>);

            _factories.Add(SupportDirectType<double>, ConfigureField<DoubleField, double>);

            _factories.Add(SupportDirectType<string>, ConfigureStringField);

            _factories.Add(SupportDirectType<char>, ConfigureCharField);

            _factories.Add(type => SupportDirectType<Color>(type) || SupportDirectType<Color32>(type), ConfigureField<ColorField, Color>);

            _factories.Add(SupportDirectType<LayerMask>, ConfigureField<LayerMaskField, int>);

            _factories.Add(type => type.IsSubclassOf<Enum>(), ConfigureEnumField);

            _factories.Add(SupportDirectType<Vector2>, ConfigureField<Vector2Field, Vector2>);

            _factories.Add(SupportDirectType<Vector3>, ConfigureField<Vector3Field, Vector3>);

            _factories.Add(SupportDirectType<Vector4>, ConfigureField<Vector4Field, Vector4>);

            _factories.Add(SupportDirectType<Rect>, ConfigureField<RectField, Rect>);

            _factories.Add(SupportDirectType<AnimationCurve>, ConfigureField<CurveField, AnimationCurve>);

            _factories.Add(SupportDirectType<Bounds>, ConfigureField<BoundsField, Bounds>);

            _factories.Add(SupportDirectType<Gradient>, ConfigureField<GradientField, Gradient>);

            _factories.Add(SupportDirectType<Vector2Int>, ConfigureField<Vector2IntField, Vector2Int>);

            _factories.Add(SupportDirectType<Vector3Int>, ConfigureField<Vector3IntField, Vector3Int>);

            _factories.Add(SupportDirectType<RectInt>, ConfigureField<RectIntField, RectInt>);

            _factories.Add(SupportDirectType<BoundsInt>, ConfigureField<BoundsIntField, BoundsInt>);

            _factories.Add(SupportDirectType<Hash128>, ConfigureField<Hash128Field, Hash128>);

            _factories.Add(type => type.IsSubclassOf<Object>(), ConfigureObjectField);
        }

        private static bool SupportDirectType<T>(Type type)
        {
            return type == typeof(T);
        }

        private static VisualElement ConfigureEnumField(Parameter parameter)
        {
            var enumType = parameter.ParameterType;
            var enumValues = enumType.GetEnumValues().Cast<object>().ToList();
            var enumDisplayNames = enumValues.Select(enumValue => ObjectNames.NicifyVariableName(enumValue.ToString())).ToList();
            var enumValueIndex = enumValues.IndexOf(parameter.Data);
            if (enumType.IsDefined(typeof(FlagsAttribute), false))
            {
                var enumField = ConfigureField<EnumFlagsField, Enum>(parameter);
                enumField.choices = enumDisplayNames;
                enumField.value = (Enum)Enum.ToObject(enumType, enumValueIndex);
                return enumField;
            }


            var propertyFieldIndex = enumValueIndex < 0 || enumValueIndex >= enumDisplayNames.Count ? -1 : enumValueIndex;

            var popupField = ConfigureField<PopupField<string>, string>(parameter);

            popupField.choices = enumDisplayNames;
            popupField.index = propertyFieldIndex;

            return popupField;
        }

        private static VisualElement ConfigureStringField(Parameter parameter)
        {
            var textField = ConfigureField<TextField, string>(parameter);
            textField.maxLength = -1;
            return textField;
        }

        private static VisualElement ConfigureCharField(Parameter parameter)
        {
            var textField = ConfigureField<TextField, string>(parameter);
            textField.maxLength = -1;
            return textField;
        }

        private static VisualElement ConfigureObjectField(Parameter parameter)
        {
            var objectField = ConfigureField<ObjectField, Object>(parameter);
            objectField.objectType = parameter.ParameterType;
            objectField.allowSceneObjects = true;
            
            var visualElement = objectField.Q<VisualElement>(ObjectFieldDisplayClassName);
            visualElement?.style.Width(StyleDefinition.OneStyleLength);
            
            return objectField;
        }

        private static VisualElement ConfigureIntegerField(Parameter parameter)
        {
            var integerField = ConfigureField<IntegerField, int>(parameter);
            integerField.isDelayed = false;
            return integerField;
        }

        private static TField ConfigureField<TField, TValue>(
            Parameter parameter)
            where TField : BaseField<TValue>, new()
        {
            var field = new TField();
            field.RegisterValueChangedCallback(evt => { parameter.SetData(evt.newValue); });
            field.value = (TValue)parameter.Data;
            field.label = ObjectNames.NicifyVariableName(parameter.Name);
            field.style.FlexGrow(StyleDefinition.OneStyleFloat);
            field.labelElement.style.MinWidth(StyleKeyword.Auto);
            return field;
        }

        public static bool IsSupported(Type type)
        {
            foreach (var factory in _factories)
            {
                if (factory.SupportedFunc.Invoke(type))
                {
                    return true;
                }
            }

            return false;
        }

        public static VisualElement CreateParameterField(Parameter parameter)
        {
            foreach (var factory in _factories)
            {
                if (factory.SupportedFunc.Invoke(parameter.ParameterType))
                {
                    return factory.CreateFunc.Invoke(parameter);
                }
            }

            return null;
        }
    }
}