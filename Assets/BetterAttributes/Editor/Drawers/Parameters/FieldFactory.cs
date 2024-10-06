using System;
using UnityEngine.UIElements;

namespace Better.Attributes.EditorAddons.Drawers.Parameters
{
    internal class FieldFactory
    {
        public Func<Type, bool> SupportedFunc { get; }
        public Func<Parameter, VisualElement> CreateFunc { get; }

        public FieldFactory(Func<Type, bool> supportedFunc, Func<Parameter, VisualElement> createFunc)
        {
            SupportedFunc = supportedFunc;
            CreateFunc = createFunc;
        }
    }
}