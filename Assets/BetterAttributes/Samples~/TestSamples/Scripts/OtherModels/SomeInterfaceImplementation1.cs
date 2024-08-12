using System;
using System.Collections.Generic;
using Better.Attributes.Runtime.Gizmo;
using Samples.Interfaces;
using UnityEngine;

namespace Samples.OtherModels
{
    [Serializable]
    public class SomeInterfaceImplementation1 : ISomeInterface
    {
        [SerializeField] private int intField;
        [Gizmo][SerializeField] private List<Vector3> gVector3;
    }
}