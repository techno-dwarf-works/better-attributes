using System.Reflection;
using Better.Attributes.EditorAddons.Drawers.WrapperCollections;
using Better.Attributes.Runtime.Gizmo;
using Better.Commons.EditorAddons.Drawers;
using Better.Commons.EditorAddons.Drawers.Attributes;
using Better.Commons.EditorAddons.Drawers.Base;
using Better.Commons.EditorAddons.Utility;
using Better.Commons.Runtime.Drawers.Attributes;
using Better.Commons.Runtime.Extensions;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

#if UNITY_2022_1_OR_NEWER
using GizmoUtility = Better.Attributes.EditorAddons.Drawers.Utility.GizmoUtility;
#else
using Better.Attributes.EditorAddons.Drawers.Utility;
#endif

namespace Better.Attributes.EditorAddons.Drawers.Gizmo
{
    [MultiCustomPropertyDrawer(typeof(GizmoAttribute))]
    [MultiCustomPropertyDrawer(typeof(GizmoLocalAttribute))]
    public class GizmoDrawer : MultiFieldDrawer<GizmoWrapper>
    {
        public const string Hide = "Hide";
        public const string Show = "Show";

        public GizmoDrawer(FieldInfo fieldInfo, MultiPropertyAttribute attribute) : base(fieldInfo, attribute)
        {
        }

        public override void Initialize(FieldDrawer drawer)
        {
            base.Initialize(drawer);
            EditorApplication.delayCall += DelayCall;
        }

        private void DelayCall()
        {
            EditorApplication.delayCall -= DelayCall;
            SceneView.duringSceneGui += OnSceneGUIDelegate;
            SceneView.RepaintAll();
        }

        private GizmoHandlers Collection
        {
            get
            {
                _handlers ??= GenerateCollection();

                return _handlers as GizmoHandlers;
            }
        }

        private void OnSceneGUIDelegate(SceneView sceneView)
        {
            if (sceneView.drawGizmos)
            {
                if (_handlers == null)
                {
                    _handlers = GenerateCollection();
                }

                GizmoUtility.Instance.ValidateCachedProperties(_handlers);
                Collection?.Apply(sceneView);
            }
        }

        protected override void Deconstruct()
        {
            SceneView.duringSceneGui -= OnSceneGUIDelegate;
            _handlers?.Deconstruct();
        }

        protected override void PopulateContainer(ElementsContainer container)
        {
            var fieldType = GetFieldOrElementType();
            var property = container.Property;

            if (!GizmoUtility.Instance.IsSupported(fieldType))
            {
                container.AddNotSupportedBox(fieldType, _attribute.GetType());
                return;
            }

            var cache = ValidateCachedProperties(property, GizmoUtility.Instance);
            if (!cache.IsValid)
            {
                Collection.SetProperty(property, fieldType);
            }

            if (!container.TryGetByTag(property, out var propertyElement))
            {
                return;
            }

            if (!propertyElement.Elements.TryFind(out PropertyField propertyField))
            {
                return;
            }

            var button = new Button();
            UpdateButtonText(button, property);
            button.RegisterCallback<ClickEvent, (SerializedProperty, Button)>(OnClicked, (property, button));
            propertyField.Add(button);
        }

        private void UpdateButtonText(Button button, SerializedProperty property)
        {
            button.text = Collection.ShowInSceneView(property) ? Hide : Show;
        }

        private void OnClicked(ClickEvent clickEvent, (SerializedProperty property, Button button) data)
        {
            Collection.SetMode(data.property, !Collection.ShowInSceneView(data.property));
            UpdateButtonText(data.button, data.property);
        }

        protected override HandlerCollection<GizmoWrapper> GenerateCollection()
        {
            return new GizmoHandlers();
        }
    }
}