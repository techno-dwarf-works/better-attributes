using System;
using Samples.Interfaces;
using UnityEngine;

namespace Samples.OtherModels
{
    [Serializable]
    public class SomeInterfaceImplementation15 : ISomeInterface
    {
        [SerializeField] private int intField;
    }
}