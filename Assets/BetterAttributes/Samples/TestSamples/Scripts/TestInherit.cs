using System.Collections.Generic;
using Better.Attributes.Runtime.Select;
using Samples.Interfaces;
using UnityEngine;

namespace Samples
{
    public class TestInherit : MonoBehaviour
    {
        [Select(typeof(ISomeInterface))] [SerializeReference]
        private List<ISomeInterface> testICollection;
    }
}