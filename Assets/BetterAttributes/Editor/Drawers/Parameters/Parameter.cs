using System;
using System.Reflection;
using Better.Commons.Runtime.Extensions;

namespace Better.Attributes.EditorAddons.Drawers.Parameters
{
    internal class Parameter
    {
        public Type ParameterType { get; }
        public string Name { get; }
        public object Data { get; private set; }
        
        public Parameter(ParameterInfo info)
        {
            ParameterType = info.ParameterType;
            Name = info.Name;
            Data = info.HasDefaultValue ? info.DefaultValue : info.ParameterType.GetDefault();
        }

        public void SetData(object newData)
        {
            Data = newData;
        }
    }
}