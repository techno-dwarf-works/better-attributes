using System;
using Samples.Interfaces;
using UnityEngine;

namespace Samples.Models
{
    [Serializable]
    public class SomeInterfaceImplementation17 : ISomeInterface
    {
        [SerializeField] private int intField;
    }
}