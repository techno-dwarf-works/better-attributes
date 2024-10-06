using System;
using Better.Attributes.Runtime;
using Better.Attributes.Runtime.Misc;
using Better.Attributes.Runtime.Select;
using Better.Attributes.Runtime.Validation;
using UnityEngine;

namespace Samples.Models
{
    [Serializable]
    public class SomeAbstractClassImplementation2 : SomeAbstractClass
    {
        [SerializeField] private bool boolField;
        
        [SerializeField] private protected int baseIntField;
        [Find] [SerializeField] private Test _test;

        public int BaseIntField => baseIntField;

        [EditorButton]
        private void Test()
        {
            Debug.Log($"[{nameof(SomeAbstractClassImplementation2)}] {nameof(Test)}");
        }
    }

    [Serializable]
    public class TestSomeAbstractClass
    {
        [Select] [Detailed(Nested = false)]
        [SerializeReference] private SomeAbstractClass _detailedAttributeNotNested;
    }
    
    [Serializable]
    public class SomeAbstractClassImplementation3 : SomeAbstractClass
    {
        [Select] 
        [SerializeReference] private SomeAbstractClass _noDetailedAttribute;
        [Select] [Detailed]
        [SerializeReference] private SomeAbstractClass _detailedAttribute;
        
        [SerializeField] private TestSomeAbstractClass _detailedAttributeNotNested;
    }
}