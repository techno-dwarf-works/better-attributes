using Better.Attributes.Runtime.Gizmo;
using UnityEngine;

namespace Samples
{
    public class PreviewTest : MonoBehaviour
    {
        [SerializeField] private Sprite sprite;
        [Gizmo] [SerializeField] private Vector3 pos;
        [GizmoLocal] [SerializeField] private Quaternion rot;
        
    }
}