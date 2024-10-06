using System;
using System.Collections.Generic;
using Better.Attributes.Runtime;
using Better.Attributes.Runtime.DrawInspector;
using Better.Attributes.Runtime.Gizmo;
using Better.Attributes.Runtime.Manipulation;
using Better.Attributes.Runtime.Misc;
using Better.Attributes.Runtime.Preview;
using Better.Attributes.Runtime.Select;
using Better.Attributes.Runtime.Validation;
using Better.Commons.Runtime.DataStructures.SerializedTypes;
using Samples.Interfaces;
using Samples.Models;
using UnityEngine;

namespace Samples
{
    [Serializable]
    public class TestSerializableType<T> : ITestSerializableType
    {
        protected TestSerializableType()
        {
        }
    }

    public class Test : MonoBehaviour, ISomeInterface
    {
        [HideLabel] [Select(typeof(ISomeInterface))] [SerializeField]
        private SerializedType serializedType;

        [Select] [NotNull] [SerializeReference]
        private List<ITestSerializableType> testInheritedList;

        [Select(typeof(ISomeInterface))] [SerializeField]
        private SerializedType serializedType2;

        [Select] [SerializeField] private KeyCode keyCode;


        [DisableIf(nameof(keyCode), KeyCode.D)] [Select] [SerializeField]
        private MyFlagEnum myFlagEnumTest;

        [EnumButtons] [HideLabel] [SerializeField]
        private TestEnum myEnumTest;

        //
        [ShowIf(nameof(keyCode), KeyCode.Backspace)] [Preview] [DrawInspector] [SerializeField]
        private PreviewTest component;

        [NotNull] [Preview] [CustomTooltip("Test tooltip")] [SerializeField]
        private Texture2D texture;

        [HelpBox("It's help box")] [DrawInspector] [SerializeField]
        private List<TestScriptableObject> scriptableObjectList;

        [DrawInspector] [SerializeField] private TestScriptableObject[] scriptableObjectArray;
        [DrawInspector] [SerializeField] private TestScriptableObject scriptableObject;

        [GizmoLocal] [SerializeField] private Vector3 vector3Local;
        [SerializeField] private Vector3 vector3Local2;

        [GizmoLocal] [RenameField("Quaternion Local Rename")] [SerializeField]
        private Quaternion quaternion;

        [GizmoLocal] [SerializeField] private SomeClass some;

        [HideLabel] [SerializeField] private SomeClass someClass;

        [Clamp01] [ReadOnly] [SerializeField]
        private float someFloat;

        [HideLabel] [Select(DisplayGrouping.GroupedFlat)] [SerializeReference]
        private ISomeInterface someInterface;

        [Detailed] [Select] [SerializeReference]
        private SomeAbstractClass someAbstractClass;

        [Detailed] [Select(typeof(SomeAbstractClass), DisplayName.Full)] [SerializeReference]
        private List<SomeAbstractClass> someAbstractClasses;

        [Dropdown("r:SingletonTest.Instance.GetIDs()")] [SerializeField]
        private int testInt2;

        [HideLabel] [SerializeField] private TestInner _testInner;


        [Select(typeof(ISomeInterface), DisplayGrouping.Grouped)] [SerializeReference]
        private List<ISomeInterface> someInterfaces;

        [GizmoLocal] [SerializeField] private Bounds bounds;

        [DisableInEditorMode] [SerializeField] private List<Vector3> _vector3s;

        [ShowInEditorMode] [SerializeField] private int showInEditorMode;

        [HideInEditorMode] [SerializeField] private int hideInEditorMode;

        [HideInPlayMode] [SerializeField] private int hideInPlayMode;

        [ShowInPlayMode] [SerializeField] private int showInPlayMode;

        [EnableInPlayMode] [SerializeField] private int enableInPlayMode;

        [DisableInEditorMode] [SerializeField] private int disableInEditorMode;

        [EnableInEditorMode] [SerializeField] private int enableInEditorMode;

        [DisableInPlayMode] [SerializeField] private int disableInPlayMode;

        [HelpBox("TestHelpBox()")] [SerializeField]
        private bool boolField;

        [ShowIf(nameof(boolField), true)] [SerializeField]
        private int showIfBool;

        [HideIf(nameof(boolField), true)] [SerializeField]
        private int hideIfBool;

        [DisableIf(nameof(boolField), true)] [SerializeField]
        private int disableIfBool;

        [EnableIf(nameof(boolField), true)] [SerializeField]
        private int enableIfBool;


        ///Default usage of attribute.
        [EditorButton]
        private void SomeMethod()
        {
            //Some code.
        }

        private string TestHelpBox()
        {
            return "This is help with dynamic method";
        }

        ///This button will call method with predefined parameters. 
        ///When invokeParams not specified will call with null.
        [EditorButton()]
        private void SomeMethod(float floatValue)
        {
            Debug.Log($"{nameof(SomeMethod)}({floatValue})");
        }

        ///This button will call method with predefined parameters. 
        ///When invokeParams not specified will call with null.
        [EditorButton()]
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
        [EditorButton("Some Cool Button")]
        private void SomeMethod3()
        {
            Debug.Log($"{nameof(SomeMethod3)}");
        }
    }
}