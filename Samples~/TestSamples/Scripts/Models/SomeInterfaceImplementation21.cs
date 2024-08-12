using System;
using Samples.Interfaces;
using UnityEngine;

namespace Samples.Models
{
    [Serializable]
    public class SomeInterfaceImplementation21 : ISomeInterface
    {
        [SerializeField] private int intField;
    }
}