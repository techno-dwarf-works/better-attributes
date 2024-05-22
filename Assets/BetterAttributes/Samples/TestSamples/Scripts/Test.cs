using System;
using System.Collections.Generic;
using Better.Attributes.Runtime;
using Better.Attributes.Runtime.DrawInspector;
using Better.Attributes.Runtime.Gizmo;
using Better.Attributes.Runtime.Manipulation;
using Better.Attributes.Runtime.Misc;
using Better.Attributes.Runtime.Preview;
using Better.Attributes.Runtime.Rename;
using Better.Attributes.Runtime.Select;
using Better.Commons.Runtime.DataStructures.SerializedTypes;
using Samples.Interfaces;
using Samples.Models;
using UnityEngine;

namespace Samples
{
    [Flags]
    public enum MyFlagEnum
    {
        First = 1,
        Second = 2,
        Third = 4
    }

    public enum TestEnum
    {
        Option,
        Option1,
        Option2,
        Option3,
        Option4,
        Option5,
        Option6,
        Option7,
        Option8,
        Option9,
        Option10,
        Option11,
        Option12,
        Option13,
        Option14,
        Option15,
        Option16,
        Option17,
        Option18,
        Option19,
        Option20,
        Option21,
        Option22,
        Option23,
        Option24,
        Option25,
        Option26,
        Option27,
    }

    public class Test : MonoBehaviour
    {
        [HideLabel] [Select(typeof(ISomeInterface))] [SerializeField]
        private SerializedType serializedType;
        
        [Select] [SerializeField] private KeyCode keyCode;
        
        [DisableIf(nameof(keyCode), KeyCode.D)] [Select] [SerializeField]
        private MyFlagEnum myFlagEnumTest;
        
        [EnumButtons] [HideLabel] [SerializeField]
        private TestEnum myEnumTest;
        
        [ShowIf(nameof(keyCode), KeyCode.Backspace)] [Preview] [DrawInspector] [SerializeField]
        private PreviewTest component;
        
        [Preview] [CustomTooltip("Test tooltip")] [SerializeField]
        private Texture2D texture;
        
        [HelpBox("It's help box")] [DrawInspector] [SerializeField]
        private List<TestScriptableObject> scriptableObjectList;
        
        [DrawInspector] [SerializeField] private TestScriptableObject[] scriptableObjectArray;
        [DrawInspector] [SerializeField] private TestScriptableObject scriptableObject;
        
        [GizmoLocal] [SerializeField] private Vector3 vector3Local;
        
        [GizmoLocal] [RenameField("Quaternion Local Rename")] [SerializeField]
        private Quaternion quaternion;
        
        [GizmoLocal] [SerializeField] private SomeClass some;
        
        [HideLabel] [SerializeField] private SomeClass someClass;
        
        [ReadOnly] [SerializeField] private float someFloat;
        
        [Select(DisplayGrouping.GroupedFlat)] [SerializeReference]
        private ISomeInterface someInterface;
        
        [Select] [SerializeReference] private SomeAbstractClass someAbstractClass;
        
        [Select(typeof(SomeAbstractClass), DisplayName.Full)] [SerializeReference]
        private List<SomeAbstractClass> someAbstractClasses;
        
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
        
        [SerializeField] private bool boolField;
        
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
        [EditorButton("Some Cool Button")]
        private void SomeMethod3()
        {
            Debug.Log($"{nameof(SomeMethod3)}");
        }
    }
}