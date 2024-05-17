using System.Reflection;
using Better.Attributes.EditorAddons.Drawers.Select.SetupStrategies;
using Better.Attributes.Runtime.Select;
using Better.Commons.EditorAddons.Drawers.Caching;
using Better.Commons.EditorAddons.Drawers.Utility;
using Better.Commons.EditorAddons.Enums;
using Better.Commons.EditorAddons.Extensions;
using Better.Commons.EditorAddons.Utility;
using Better.Commons.Runtime.Drawers.Attributes;
using Better.Commons.Runtime.Extensions;
using UnityEditor;

namespace Better.Attributes.EditorAddons.Drawers.Select.Wrappers
{
    public abstract class BaseSelectWrapper : SerializedPropertyHandler
    {
        protected SerializedProperty _property;
        protected FieldInfo _fieldInfo;
        protected MultiPropertyAttribute _attribute;
        protected SetupStrategy _setupStrategy;

        public virtual void Setup(SerializedProperty property, FieldInfo fieldInfo, MultiPropertyAttribute attribute, SetupStrategy setupStrategy)
        {
            _property = property;
            _fieldInfo = fieldInfo;
            _attribute = attribute;
            _setupStrategy = setupStrategy;
        }

        public virtual HeightCacheValue GetHeight()
        {
            var copy = _property.Copy();
            var type = _fieldInfo.FieldType;
            if (type.IsArrayOrList())
            {
                type = type.GetCollectionElementType();
            }

            var propertyHeight = GetPropertyHeight(copy);
            if (_setupStrategy == null || !_setupStrategy.CheckSupported())
            {
                var message = VisualElementUtility.NotSupportedMessage(copy.name, type, _attribute.GetType());
                propertyHeight += VisualElementUtility.GetHelpBoxHeight(EditorGUIUtility.currentViewWidth, message, IconType.ErrorMessage);
                propertyHeight += VisualElementUtility.SpaceHeight;
            }

            var full = HeightCacheValue.GetFull(propertyHeight);
            copy.Dispose();
            return full;
        }

        protected virtual float GetPropertyHeight(SerializedProperty copy)
        {
            var includeChildren = !_setupStrategy.SkipFieldDraw();
            var propertyHeight = 0f;
            if (includeChildren)
            {
                includeChildren = copy.isExpanded;
                propertyHeight = EditorGUI.GetPropertyHeight(copy, includeChildren);
            }

            return propertyHeight;
        }

        public abstract void Update(object value);

        public abstract object GetCurrentValue();

        public virtual bool Verify()
        {
            return _property != null && _property.Verify();
        }

        public override void Deconstruct()
        {
            _property = null;
        }
    }
}