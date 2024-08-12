using System;
using Better.Attributes.Runtime.Misc;
using UnityEngine;

namespace Samples.Models
{
    [Serializable]
    public class SomeClass
    {
        [SerializeField] private int intField;
        [SerializeField] private int intField1;
        [HideLabel]
        [SerializeField] private int intField2;
        [SerializeField] private int intField3;
        [SerializeField] private int intField4;
    }
}