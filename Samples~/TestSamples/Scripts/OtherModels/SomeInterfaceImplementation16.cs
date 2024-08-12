using System;
using Samples.Interfaces;
using UnityEngine;

namespace Samples.OtherModels
{
    [Serializable]
    public class SomeInterfaceImplementation16 : ISomeInterface
    {
        [SerializeField] private int intField;
    }
}