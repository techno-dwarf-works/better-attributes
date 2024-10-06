using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Better.Attributes.EditorAddons.Drawers.Parameters
{
    internal static class FieldFactoryExtensions
    {
        public static void Add(this List<FieldFactory> self, Func<Type, bool> supportedFunc, Func<Parameter, VisualElement> createFunc)
        {
            var fieldFactory = new FieldFactory(supportedFunc, createFunc);
            self.Add(fieldFactory);
        }
    }
}