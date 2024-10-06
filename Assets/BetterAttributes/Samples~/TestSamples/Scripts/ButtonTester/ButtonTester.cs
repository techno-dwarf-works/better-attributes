using Better.Attributes.Runtime;
using UnityEngine;

namespace Samples.ButtonTester
{
    public class ButtonTester : MonoBehaviour
    {
        [EditorButton]
        private void Test()
        {
            Debug.Log(nameof(Test));
        }
        
        [EditorButton(CaptureGroup = 1)]
        private void TestInt(int value)
        {
            Debug.Log($"{nameof(TestInt)} {value}");
        }
        
        [EditorButton(CaptureGroup = 1)]
        private void TestString(string value, string stringValue = "test")
        {
            Debug.Log($"{nameof(TestString)} {value} {stringValue}");
        }
        
        [EditorButton(CaptureGroup = 1)]
        private void TestGameObject(GameObject value, double doubleValue = 3d)
        {
            Debug.Log($"{nameof(TestGameObject)} {value} {doubleValue}");
        }
        
        [EditorButton]
        private void TestInner(TestInner value)
        {
            Debug.Log($"{nameof(TestInner)} {value}");
        }
        
        [EditorButton]
        private void TestAnimationCurve(AnimationCurve value)
        {
            Debug.Log($"{nameof(TestAnimationCurve)} {value}");
        }
    }
}