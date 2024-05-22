using System.Reflection;
using Better.Attributes.Runtime.Rename;
using Better.Commons.EditorAddons.Drawers;
using Better.Commons.EditorAddons.Drawers.Attributes;
using Better.Commons.EditorAddons.Drawers.Base;
using Better.Commons.EditorAddons.Utility;
using Better.Commons.Runtime.Drawers.Attributes;
using Better.Commons.Runtime.Extensions;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

namespace Better.Attributes.EditorAddons.Drawers.Rename
{
    [MultiCustomPropertyDrawer(typeof(RenameFieldAttribute))]
    public class RenameFieldDrawer : FieldDrawer
    {
        protected override void Deconstruct()
        {
        }

        protected override void PopulateContainer(ElementsContainer container)
        {
            if (!container.TryGetPropertyField(out var propertyElement))
            {
                return;
            }
            var newName = (_attribute as RenameFieldAttribute)?.Name;
            propertyElement.label = newName;
        }

        public RenameFieldDrawer(FieldInfo fieldInfo, MultiPropertyAttribute attribute) : base(fieldInfo, attribute)
        {
        }
    }
}