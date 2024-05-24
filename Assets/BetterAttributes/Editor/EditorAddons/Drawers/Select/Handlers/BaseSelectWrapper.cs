using System.Reflection;
using Better.Attributes.EditorAddons.Drawers.Select.SetupStrategies;
using Better.Commons.EditorAddons.Drawers.Utility;
using Better.Commons.EditorAddons.Extensions;
using Better.Commons.Runtime.Drawers.Attributes;
using UnityEditor;

namespace Better.Attributes.EditorAddons.Drawers.Select
{
    public abstract class BaseSelectWrapper : SerializedPropertyHandler
    {
        protected SerializedProperty _property;
        protected FieldInfo _fieldInfo;
        protected MultiPropertyAttribute _attribute;
        protected SetupStrategy _setupStrategy;

        public void Setup(SerializedProperty property, FieldInfo fieldInfo, MultiPropertyAttribute attribute, SetupStrategy setupStrategy)
        {
            _property = property;
            _fieldInfo = fieldInfo;
            _attribute = attribute;
            _setupStrategy = setupStrategy;
            OnSetup();
        }

        protected abstract void OnSetup();
        
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