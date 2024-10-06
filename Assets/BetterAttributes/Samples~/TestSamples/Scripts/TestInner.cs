using System;
using Better.Attributes.Runtime.Select;
using UnityEngine;

namespace Samples
{
    [Serializable]
    public class TestInner
    {
        [Dropdown("r:SingletonTest.Instance.GetIDs()")] [SerializeField]
        private int testIntLog;
    }
}