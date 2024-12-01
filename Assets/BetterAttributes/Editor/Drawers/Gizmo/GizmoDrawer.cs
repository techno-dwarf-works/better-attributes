using System.Collections.Generic;
using Better.Attributes.Runtime.Gizmo;
using Better.Commons.EditorAddons.Comparers;
using Better.Commons.EditorAddons.Drawers;
using Better.Commons.EditorAddons.Drawers.BehavioredElements;
using Better.Commons.EditorAddons.Drawers.Container;
using Better.Commons.EditorAddons.Extensions;
using Better.Commons.EditorAddons.Utility;
using UnityEditor;
using UnityEngine.UIElements;

namespace Better.Attributes.EditorAddons.Drawers.Gizmo
{
    [CustomPropertyDrawer(typeof(BaseGizmoAttribute), true)]
    public class GizmoDrawer : PropertyDrawer<GizmoHandler, BaseGizmoAttribute>
    {
        public const string Hide = "Hide";
        public const string Show = "Show";

        private Dictionary<SerializedProperty, BehavioredElement<Button>> _behavioredElements;

        public GizmoDrawer()
        {
            EditorApplication.delayCall += DelayCall;
            _behavioredElements = new Dictionary<SerializedProperty, BehavioredElement<Button>>(SerializedPropertyComparer.Instance);
        }

        private void DelayCall()
        {
            EditorApplication.delayCall -= DelayCall;
            SceneView.duringSceneGui += OnSceneGUIDelegate;
            SceneView.RepaintAll();
        }

        private void OnSceneGUIDelegate(SceneView sceneView)
        {
            if (sceneView.drawGizmos)
            {
                Handlers?.Revalidate();
                Apply(sceneView);
            }
        }

        private void Apply(SceneView sceneView)
        {
            Handlers.Revalidate();
            
            foreach (var gizmo in Handlers)
            {
                var valueWrapper = gizmo.Value.Handler;
                valueWrapper.Apply(sceneView);
            }
        }

        protected override void ContainerReleased(ElementsContainer container)
        {
            base.ContainerReleased(container);
            SceneView.duringSceneGui -= OnSceneGUIDelegate;
        }

        protected override void PopulateContainer(ElementsContainer container)
        {
            var fieldType = GetFieldOrElementType();
            var serializedProperty = container.SerializedProperty;

            if (!TypeHandlersBinder.IsSupported(fieldType))
            {
                container.AddNotSupportedBox(fieldType, Attribute.GetType());
                return;
            }

            var handler = GetHandler(serializedProperty);
            handler.SetProperty(serializedProperty, fieldType);

            
            if (!_behavioredElements.TryGetValue(serializedProperty, out var element))
            {
                element = CreateBehavioredElement(serializedProperty);
                _behavioredElements.Add(serializedProperty, element);
            }
            
            var text = handler.ShowInSceneView ? Hide : Show;
            element.SubElement.text = text;
            element.Attach(container.RootElement);
        }

        private void OnClicked(ClickEvent clickEvent, SerializedProperty property)
        {
            var handler = GetHandler(property);
            handler.SetMode(!handler.ShowInSceneView);
            if (!_behavioredElements.TryGetValue(property, out var element))
            {
                element = CreateBehavioredElement(property);
                _behavioredElements.Add(property, element);
            }
            
            var text = handler.ShowInSceneView ? Hide : Show;
            element.SubElement.text = text;
            SceneView.RepaintAll();
        }

        private BehavioredElement<Button> CreateBehavioredElement(SerializedProperty property)
        {
            var element = new BehavioredElement<Button>(new GizmoElementBehaviour());
            element.RegisterCallback<ClickEvent, SerializedProperty>(OnClicked, property);
            return element;
        }
    }
}