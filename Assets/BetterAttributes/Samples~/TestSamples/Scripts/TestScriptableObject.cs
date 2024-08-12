using System.Collections.Generic;
using Better.Attributes.Runtime;
using Better.Attributes.Runtime.Gizmo;
using Better.Attributes.Runtime.Manipulation;
using Better.Attributes.Runtime.Select;
using Samples.Interfaces;
using Samples.Models;
using UnityEngine;

namespace Samples
{
    [CreateAssetMenu(menuName = "Create TestScriptableObject", fileName = "TestScriptableObject", order = 0)]
    public class TestScriptableObject : ScriptableObject
    {
        [Select] [SerializeField] private KeyCode keyCode;
        [Select] [SerializeField] private KeyCode keyCode1;

        [Gizmo] [SerializeField] private Bounds bounds;

        [Gizmo] [SerializeField] private Vector3 vector3;

        [Gizmo] [SerializeField] private Quaternion quaternion;

        [ReadOnly] [SerializeField] private SomeClass someClass;

        [ReadOnly] [SerializeField] private float someFloat;

        [Select] [SerializeReference] private ISomeInterface someInterface;

        [Select] [SerializeReference] private SomeAbstractClass someAbstractClass;

        [Select(typeof(SomeAbstractClass))] [SerializeReference]
        private List<SomeAbstractClass> someAbstractClasses;

        [Select(typeof(ISomeInterface))] [SerializeReference]
        private List<ISomeInterface> someInterfaces;

        ///Default usage of attribute.
        [EditorButton]
        private void SomeMethod()
        {
            //Some code.
        }

        ///This button will call method with predefined parameters. 
        ///When invokeParams not specified will call with null.
        [EditorButton(InvokeParams = new object[] { 10f })]
        private void SomeMethod(float floatValue)
        {
            Debug.Log($"{nameof(SomeMethod)}({floatValue})");
        }

        ///This button will call method with predefined parameters. 
        ///When invokeParams not specified will call with null.
        [EditorButton(InvokeParams = new object[] { 10f, 10 })]
        private void SomeMethod(float floatValue, int intValue)
        {
            Debug.Log($"{nameof(SomeMethod)}({floatValue}, {intValue})");
        }

        /// This button will be in the same row with button for SomeMethod2.
        /// But will be in the second position.
        /// When captureGroup not specified each button placed in separate row.
        /// When priority not specified buttons in one row sorted by order in code.
        [EditorButton(CaptureGroup = 1, Priority = 2)]
        private void SomeMethod1()
        {
            Debug.Log($"{nameof(SomeMethod1)}");
        }

        [EditorButton(CaptureGroup = 1, Priority = 1)]
        private void SomeMethod2()
        {
            Debug.Log($"{nameof(SomeMethod2)}");
        }

        /// This button will have name "Some Cool Button".
        /// When displayName not specified or null/empty/whitespace button 
        /// will have name same as method.
        [EditorButton(displayName: "Some Cool Button")]
        private void SomeMethod3()
        {
            Debug.Log($"{nameof(SomeMethod3)}");
        }
    }
}