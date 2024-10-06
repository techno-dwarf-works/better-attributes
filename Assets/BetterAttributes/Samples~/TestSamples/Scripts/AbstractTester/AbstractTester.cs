using Better.Attributes.Runtime.Select;
using UnityEngine;

namespace Samples.MiscTest
{
    public class AbstractTester : MonoBehaviour
    {
        [Select]
        [SerializeReference] private AbstractTest _test;
    }
}