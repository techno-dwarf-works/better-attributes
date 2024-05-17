using System;
using Better.Attributes.Runtime.Manipulation;
using Better.Commons.EditorAddons.Drawers.Caching;
using Better.Commons.EditorAddons.Drawers.Utility;
using Better.Commons.EditorAddons.Utility;
using Better.Commons.Runtime.Utility;
using UnityEditor;
using UnityEngine;

namespace Better.Attributes.EditorAddons.Drawers.Manipulation.Wrappers
{
    public abstract class ManipulateWrapper : SerializedPropertyHandler
    {
        protected SerializedProperty _property;
        protected ManipulateAttribute _attribute;
        private GUI.Scope _scope;
        private int _currentCount;

        public override void Deconstruct()
        {
            _scope?.Dispose();
        }

        protected abstract bool IsConditionSatisfied();
        
        public virtual bool PreDraw(ref Rect position)
        {
            var satisfied = IsConditionSatisfied();

            switch (_attribute.ModeType)
            {
                case ManipulationMode.Show:
                    _scope = new HideGroup(!satisfied);
                    if (!satisfied)
                    {
                        position = DisableClick();
                    }

                    break;
                case ManipulationMode.Hide:
                    _scope = new HideGroup(satisfied);
                    if (satisfied)
                    {
                        position = DisableClick();
                    }

                    break;
                case ManipulationMode.Disable:
                    _scope = new EditorGUI.DisabledGroupScope(satisfied);
                    break;
                case ManipulationMode.Enable:
                    _scope = new EditorGUI.DisabledGroupScope(!satisfied);
                    break;
                default:
                DebugUtility.LogException<ArgumentOutOfRangeException>();
                break;
            }

            return true;
        }

        private Rect DisableClick()
        {
            _currentCount = Event.current.clickCount;
            Event.current.clickCount = 0;
            return Rect.zero;
        }

        public virtual HeightCacheValue GetHeight()
        {
            var satisfied = IsConditionSatisfied();

            switch (_attribute.ModeType)
            {
                case ManipulationMode.Show:
                    var heightCacheValue = new HeightCacheValue(satisfied, 0);
                    return satisfied ? heightCacheValue : heightCacheValue.Force();
                case ManipulationMode.Hide:
                    var height = new HeightCacheValue(!satisfied, 0);
                    return !satisfied ? height : height.Force();
                case ManipulationMode.Disable:
                case ManipulationMode.Enable:
                    return HeightCacheValue.GetAdditive(0f);
                default:
                    DebugUtility.LogException<ArgumentOutOfRangeException>();
                    return HeightCacheValue.GetAdditive(0f);
            }
        }

        public virtual void SetProperty(SerializedProperty property, ManipulateAttribute attribute)
        {
            _property = property;
            _attribute = attribute;
        }

        public virtual void PostDraw()
        {
            _scope?.Dispose();
            if (_currentCount > 0)
            {
                Event.current.clickCount = _currentCount;
            }
        }
    }
}