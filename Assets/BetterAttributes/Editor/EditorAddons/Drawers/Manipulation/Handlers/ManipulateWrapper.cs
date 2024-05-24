using System;
using Better.Attributes.Runtime.Manipulation;
using Better.Commons.EditorAddons.Drawers;
using Better.Commons.EditorAddons.Drawers.Utility;
using Better.Commons.Runtime.Extensions;
using Better.Commons.Runtime.Utility;
using UnityEditor;

namespace Better.Attributes.EditorAddons.Drawers.Manipulation
{
    public abstract class ManipulateWrapper : SerializedPropertyHandler
    {
        protected SerializedProperty _property;
        protected ManipulateAttribute _attribute;

        public override void Deconstruct()
        {
        }

        protected abstract bool IsConditionSatisfied();

        public virtual void SetProperty(SerializedProperty property, ManipulateAttribute attribute)
        {
            _property = property;
            _attribute = attribute;
        }

        public void PopulateContainer(ElementsContainer container)
        {
            UpdateState(container);
        }

        public void UpdateState(ElementsContainer container)
        {
            var satisfied = IsConditionSatisfied();
            switch (_attribute.ModeType)
            {
                case ManipulationMode.Show:
                    container.ContainerStyle.SetVisible(satisfied);
                    break;
                case ManipulationMode.Hide:
                    container.ContainerStyle.SetVisible(!satisfied);
                    break;
                case ManipulationMode.Disable:
                    container.SetEnabled(!satisfied);
                    break;
                case ManipulationMode.Enable:
                    container.SetEnabled(satisfied);
                    break;
                default:
                    DebugUtility.LogException<ArgumentOutOfRangeException>();
                    break;
            }
        }
    }
}