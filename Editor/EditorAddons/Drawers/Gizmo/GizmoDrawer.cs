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
    public class GizmoDrawer : BasePropertyDrawer<GizmoHandler, BaseGizmoAttribute>
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
                ValidationUtility.ValidateCachedProperties(Handlers);
                Apply(sceneView);
            }
        }

        private void Apply(SceneView sceneView)
        {
            List<SerializedProperty> keysToRemove = null;
            foreach (var gizmo in Handlers)
            {
                var valueWrapper = gizmo.Value.Handler;
                if (valueWrapper.Validate())
                {
                    valueWrapper.Apply(sceneView);
                }
                else
                {
                    if (keysToRemove == null)
                    {
                        keysToRemove = new List<SerializedProperty>();
                    }

                    keysToRemove.Add(gizmo.Key);
                }
            }

            if (keysToRemove != null)
            {
                foreach (var property in keysToRemove)
                {
                    Handlers.Remove(property);
                }
            }
        }

        protected override void Deconstruct()
        {
            base.Deconstruct();
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