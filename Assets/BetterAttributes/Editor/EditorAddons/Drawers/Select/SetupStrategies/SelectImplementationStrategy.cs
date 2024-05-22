using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Better.Attributes.Runtime.Select;
using Better.Commons.EditorAddons.Enums;
using Better.Commons.EditorAddons.Extensions;
using Better.Commons.Runtime.Extensions;
using UnityEngine;

namespace Better.Attributes.EditorAddons.Drawers.Select.SetupStrategies
{
    public class SelectImplementationStrategy : SelectSerializedTypeStrategy
    {
        public SelectImplementationStrategy(FieldInfo fieldInfo, object container, SelectAttributeBase selectAttributeBase) : base(fieldInfo, container,
            selectAttributeBase)
        {
        }
        
        public override List<object> Setup()
        {
            var selectionObjects = GetFieldOrElementType().GetAllInheritedTypesWithoutUnityObject().Cast<object>().ToList();
            selectionObjects.Insert(0, null);
            return selectionObjects;
        }
        
        public override bool IsSkipingFieldDraw()
        {
            return false;
        }

        public override bool CheckSupported()
        {
            var baseType = GetFieldOrElementType();
            return baseType.IsAbstract || baseType.IsInterface;
        }

        public override GUIContent[] ResolveGroupedName(object value, DisplayGrouping grouping)
        {
            if (value is Type type)
            {
                if (!ValidateInternal(type))
                {
                    var resolveName = GUIContent(type);
                    return new GUIContent[] { resolveName };
                }
            }

            return base.ResolveGroupedName(value, grouping);
        }

        public override GUIContent ResolveName(object value, DisplayName displayName)
        {
            if (value is Type type)
            {
                if (!ValidateInternal(type))
                {
                    var resolveName = GUIContent(type);
                    return resolveName;
                }
            }

            return base.ResolveName(value, displayName);
        }

        private static GUIContent GUIContent(Type type)
        {
            var resolveName = IconType.ErrorMessage.GetIconGUIContent();
            resolveName.text = $"{type.Name}";
            resolveName.tooltip = "Type has not parameterless constructor!";
            return resolveName;
        }
    }
}