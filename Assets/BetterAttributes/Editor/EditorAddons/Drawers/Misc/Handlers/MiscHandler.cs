using System.Reflection;
using Better.Attributes.Runtime.Misc;
using Better.Commons.EditorAddons.Drawers;
using Better.Commons.EditorAddons.Drawers.Container;
using Better.Commons.EditorAddons.Drawers.Handlers;

namespace Better.Attributes.EditorAddons.Drawers.Misc
{
    public abstract class MiscHandler : SerializedPropertyHandler
    {
        protected ElementsContainer _container;
        protected MiscAttribute _attribute;
        protected FieldInfo _fieldInfo;

        public override void Deconstruct()
        {
        }

        public void SetupContainer(ElementsContainer container, FieldInfo fieldInfo, MiscAttribute attribute)
        {
            _container = container;
            _attribute = attribute;
            _fieldInfo = fieldInfo;
            OnSetupContainer();
        }

        protected abstract void OnSetupContainer();
    }
}