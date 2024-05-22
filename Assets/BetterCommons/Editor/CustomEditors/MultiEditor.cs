using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Better.Commons.EditorAddons.CustomEditors.Attributes;
using Better.Commons.EditorAddons.CustomEditors.Base;
using Better.Commons.EditorAddons.Extensions;
using Better.Commons.Runtime.Extensions;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Better.Commons.EditorAddons.CustomEditors
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Object), true)]
    internal sealed class MultiEditor : Editor
    {
        private List<ExtendedEditor> _preEditors = new List<ExtendedEditor>();
        private List<ExtendedEditor> _postEditors = new List<ExtendedEditor>();
        private bool _overrideDefault;

        private void OnEnable()
        {
            try
            {
                if (target.IsNullOrDestroyed() || serializedObject.IsDisposed())
                {
                    return;
                }
            }
            catch
            {
                return;
            }

            var targetType = target.GetType();

            var extensions = FindEditors(targetType);

            Iterate(extensions);
        }

        private static IReadOnlyList<(Type type, MultiEditorAttribute)> FindEditors(Type targetType)
        {
            bool WherePredicate((Type type, MultiEditorAttribute attribute) x)
            {
                var att = x.Item2;
                if (att == null)
                {
                    return false;
                }

                if (att.EditorForChildClasses)
                {
                    return att.EditorFor.IsAssignableFrom(targetType);
                }

                return att.EditorFor == targetType;
            }

            return typeof(ExtendedEditor).GetAllInheritedTypesWithoutUnityObject().Select(type => (type, type.GetCustomAttribute<MultiEditorAttribute>()))
                .Where(WherePredicate).OrderBy(x => x.Item2.Order).ToArray();
        }

        private void Iterate(IReadOnlyList<(Type type, MultiEditorAttribute)> extensions)
        {
            var paramArray = new object[2]
            {
                target, serializedObject
            };

            for (var index = 0; index < extensions.Count; index++)
            {
                var (type, betterEditorAttribute) = extensions[index];
                if (!_overrideDefault && betterEditorAttribute.OverrideDefaultEditor)
                {
                    _overrideDefault = true;
                }

                var extension = (ExtendedEditor)Activator.CreateInstance(type, paramArray);
                extension.OnEnable();
                if (betterEditorAttribute.Order < 0)
                {
                    _preEditors.Add(extension);
                }
                else
                {
                    _postEditors.Add(extension);
                }
            }
        }

        public override VisualElement CreateInspectorGUI()
        {
            var container = new VisualElement();

            for (var i = 0; i < _preEditors.Count; i++)
            {
                var element = _preEditors[i].CreateInspectorGUI();
                if (element != null)
                {
                    container.Add(element);
                }
            }

            if (!_overrideDefault)
            {
                InspectorElement.FillDefaultInspector(container, serializedObject, this);
            }

            for (var i = 0; i < _postEditors.Count; i++)
            {
                var element = _postEditors[i].CreateInspectorGUI();
                if (element != null)
                {
                    container.Add(element);
                }
            }

            container.TrackSerializedObjectValue(serializedObject, OnSerializedObjectTrack);

            return container;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
        }

        private void OnSerializedObjectTrack(SerializedObject serializedObject)
        {
            for (var i = 0; i < _preEditors.Count; i++)
            {
                _preEditors[i].OnChanged(serializedObject);
            }

            for (var i = 0; i < _postEditors.Count; i++)
            {
                _postEditors[i].OnChanged(serializedObject);
            }
        }

        private void OnDisable()
        {
            for (var i = 0; i < _preEditors.Count; i++)
            {
                _preEditors[i].OnDisable();
            }

            for (var i = 0; i < _postEditors.Count; i++)
            {
                _postEditors[i].OnDisable();
            }
        }
    }
}