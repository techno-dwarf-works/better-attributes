using System;
using Better.Attributes.Runtime.Validation;
using Better.Commons.EditorAddons.Drawers;
using Better.Commons.Runtime.Extensions;
using UnityEditor;

namespace Better.Attributes.EditorAddons.Drawers.Validation.Handlers
{
    [Serializable]
    [HandlerBinding(typeof(PrefabReferenceAttribute))]
    public class PrefabHandler : NotNullHandler
    {
        public override ValidationValue<string> Validate()
        {
            var baseResult = base.Validate();
            if (!baseResult.State)
            {
                return baseResult;
            }

            if (Property.propertyType != SerializedPropertyType.ObjectReference)
            {
                return GetClearValue();
            }
            
            var obj = Property.objectReferenceValue;
            if (!PrefabUtility.IsPartOfPrefabAsset(obj))
            {
                var str = $"\"{Property.displayName.FormatBoldItalic()}\"";
                if (!PrefabUtility.IsPartOfNonAssetPrefabInstance(obj))
                {
                    return GetNotValidValue($"Object in {str} field is not prefab");
                }

                return GetNotValidValue($"Object in {str} field is prefab instance in scene");
            }

            return GetClearValue();
        }
    }
}