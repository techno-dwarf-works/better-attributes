using System;
using Better.Attributes.Runtime.Validation;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Better.Attributes.EditorAddons.Drawers.Validation.Handlers
{
    [Serializable]
    public class MissingComponentHandler : ValidationHandler
    {
        private Object _target;

        public MissingComponentHandler(Object target)
        {
            _target = target;
        }

        public override ValidationType Type => ValidationType.Error;

        public override ValidationValue<string> Validate()
        {
            if (!_target) return GetClearValue();
            if (_target is GameObject gameObject)
            {
                var components = gameObject.GetComponents<Component>();
                for (var index = components.Length - 1; index >= 0; index--)
                {
                    var obj = components[index];
                    if (!obj) return GetNotValidValue($"Missing Component on GameObject: {_target.name}");
                }
            }

            return GetClearValue();
        }

        public override bool IsSupported()
        {
            return true;
        }

        public override void Deconstruct()
        {
        }
    }
}