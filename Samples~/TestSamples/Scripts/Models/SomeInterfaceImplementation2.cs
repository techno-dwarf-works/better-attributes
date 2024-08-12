using System;
using Samples.Interfaces;
using UnityEngine;

namespace Samples.Models
{
    [Serializable]
    public class SomeInterfaceImplementation2 : ISomeInterface
    {
        [SerializeField] private bool boolField;
    }
}