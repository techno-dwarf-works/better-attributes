using Better.Attributes.Runtime;
using Better.Attributes.Runtime.Select;
using Samples.Interfaces;
using UnityEngine;

namespace Samples
{
    public class RemovableTest : MonoBehaviour
    {
        [Select] [SerializeReference]
        private ISomeInterface someInterface;

        [EditorButton]
        public void Test()
        {
        
        }
    }
}
