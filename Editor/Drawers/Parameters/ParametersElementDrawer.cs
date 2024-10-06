using System.Linq;
using Better.Commons.Runtime.Extensions;
using UnityEngine.UIElements;

namespace Better.Attributes.EditorAddons.Drawers.Parameters
{
    internal class ParametersElementDrawer : VisualElement
    {
        private readonly Parameter[] _parameters;

        public ParametersElementDrawer(Parameter[] parameters)
        {
            _parameters = parameters;

            foreach (var parameter in _parameters)
            {
                var element = ParameterFieldProvider.CreateParameterField(parameter);
                Add(element);
            }
            
            AddToClassList(HelpBox.ussClassName);
            style.PaddingRight(5f)
                .FlexDirection(FlexDirection.Column);
        }

        public object[] GetData()
        {
            return _parameters.Select(parameter => parameter.Data).ToArray();
        }
    }
}