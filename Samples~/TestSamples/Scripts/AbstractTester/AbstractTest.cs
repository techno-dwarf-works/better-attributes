using System;
using Better.Attributes.Runtime.Manipulation;
using Better.Attributes.Runtime.Select;
using UnityEngine;

namespace Samples.MiscTest
{
    [Serializable]
    public abstract class AbstractTest
    {
        [SerializeField] private bool show;
        [ShowIf(nameof(show), true)]
        [SerializeField] private bool showField;

        [Dropdown(SingletonTest.Selector)]
        [SerializeField] private string _string;
    }
}