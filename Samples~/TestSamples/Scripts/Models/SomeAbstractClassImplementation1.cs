using System;
using Better.Attributes.Runtime.Gizmo;
using UnityEngine;

namespace Samples.Models
{
    [Serializable]
    public class SomeAbstractClassImplementation1 : SomeAbstractClass
    {
        [SerializeField] private float floatField;
        [Gizmo][SerializeField] private Vector3 vector3;

        public SomeAbstractClassImplementation1(float floatFieldValue)
        {
            floatField = floatFieldValue;
        }
    }
}