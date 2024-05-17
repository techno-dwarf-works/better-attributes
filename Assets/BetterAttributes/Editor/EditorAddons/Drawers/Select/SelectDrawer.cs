using System.Reflection;
using Better.Attributes.EditorAddons.Drawers.Select.Wrappers;
using Better.Attributes.EditorAddons.Drawers.WrapperCollections;
using Better.Attributes.Runtime.Select;
using Better.Commons.EditorAddons.Drawers.Attributes;
using Better.Commons.EditorAddons.Drawers.Base;
using Better.Commons.Runtime.Drawers.Attributes;
using UnityEditor;
using UnityEngine;

#pragma warning disable CS0618

namespace Better.Attributes.EditorAddons.Drawers.Select
{
    [MultiCustomPropertyDrawer(typeof(SelectAttribute))]
    [MultiCustomPropertyDrawer(typeof(SelectImplementationAttribute))]
    [MultiCustomPropertyDrawer(typeof(DropdownAttribute))]
    public class SelectDrawer : SelectDrawerBase<SelectAttributeBase>
    {
        protected override HandlerCollection<BaseSelectWrapper> GenerateCollection()
        {
            return new SelectHandlers();
        }
        
        protected override void DrawField(Rect position, SerializedProperty property, GUIContent label)
        {
            var rect = PreparePropertyRect(position);
            if (_setupStrategy.SkipFieldDraw())
            {
                // rect.height = value.Wrapper.GetHeight().Value;
                EditorGUI.LabelField(rect, label);
                return;
            }
            _setupStrategy.DrawField(rect, property, label);
        }

        public SelectDrawer(FieldInfo fieldInfo, MultiPropertyAttribute attribute) : base(fieldInfo, attribute)
        {
        }
    }
}