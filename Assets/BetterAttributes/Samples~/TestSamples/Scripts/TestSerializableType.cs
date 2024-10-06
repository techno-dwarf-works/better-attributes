using System;
using System.Collections.Generic;
using Better.Attributes.Runtime.Select;
using Better.Commons.Runtime.DataStructures.SerializedTypes;
using Samples.Interfaces;
using UnityEngine;

namespace Samples
{
    [Serializable]
    public class TestSerializableType : ITestSerializableType
    {
        [SerializeField] private bool t;

        [Select(typeof(ISomeInterface))] [SerializeField]
        private List<SerializedType> serializedTypes;

        protected TestSerializableType()
        {
        }
    }
}