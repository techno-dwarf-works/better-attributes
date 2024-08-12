using System;
using Samples.Interfaces;
using UnityEngine;

namespace Samples.Models
{
    [Serializable]
    public class SomeInterfaceImplementation3 : ISomeInterface
    {
        [SerializeField] private int intField;
    }
}