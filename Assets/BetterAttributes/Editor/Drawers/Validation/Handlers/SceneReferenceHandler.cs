using System;
using Better.Attributes.Runtime.Validation;
using Better.Commons.EditorAddons.Drawers;
using Better.Commons.Runtime.Extensions;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Better.Attributes.EditorAddons.Drawers.Validation.Handlers
{
    [Serializable]
    [HandlerBinding(typeof(SceneReferenceAttribute))]
    public class SceneReferenceHandler : NotNullHandler
    {
        public override ValidationValue<string> Validate()
        {
            var baseResult = base.Validate();
            if (!baseResult.State)
            {
                return baseResult;
            }

            var target = Property.serializedObject.targetObject;

            var obj = Property.objectReferenceValue;

            return ValidateNotPrefabContext(obj, target);
        }

        private ValidationValue<string> ValidateNotPrefabContext(Object obj, Object target)
        {
            var isObjectInScene = IsObjectInScene(obj);
            var isTargetInScene = IsObjectInScene(target);

            if (isTargetInScene && !isObjectInScene)
            {
                var str = $"\"{Property.displayName.FormatBoldItalic()}\"";;
                return GetNotValidValue($"Object in {str} field is not scene object");
            }

            if (!isTargetInScene)
            {
                return ValueTuple(obj, target);
            }

            return GetClearValue();
        }

        private bool IsObjectInScene(Object obj)
        {
            if (obj is GameObject gameObject)
            {
                return gameObject.scene.IsValid();
            }

            if (obj is Component component)
            {
                return component.gameObject.scene.IsValid();
            }

            return false;
        }

        private ValidationValue<string> ValueTuple(Object obj, Object target)
        {
            var objRoot = GetOutermostPrefabInstanceRoot(obj);
            var targetRoot = GetOutermostPrefabInstanceRoot(target);
            var equals = objRoot == targetRoot;
            if (!equals)
            {
                return GetNotValidValue(
                    $"Object in \"{Property.displayName.FormatBoldItalic()}\" field is not part of \"{target.name.FormatBoldItalic()}\" prefab");
            }

            return GetClearValue();
        }

        private static Object GetOutermostPrefabInstanceRoot(Object obj)
        {
            var outermostPrefabInstanceRoot = PrefabUtility.GetNearestPrefabInstanceRoot(obj);
            if (outermostPrefabInstanceRoot.IsNullOrDestroyed())
            {
                if (obj is GameObject gameObject)
                    outermostPrefabInstanceRoot = gameObject.transform.root ? gameObject.transform.root.gameObject : gameObject;

                if (obj is Component component)
                    outermostPrefabInstanceRoot = component.gameObject;
            }

            return outermostPrefabInstanceRoot;
        }
    }
}