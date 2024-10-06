using System;
using System.Collections.Generic;
using Better.Attributes.Runtime.Select;
using Samples.Interfaces;
using UnityEngine;

namespace Samples.DropdownTest
{
    public interface IDropdownTestSelect
    {
        
    }

    [Serializable]
    public class DropdownTestSelect : IDropdownTestSelect
    {
        [Select] [SerializeReference] private IDropdownTestSelect _testSelect;
        
        [Dropdown(SingletonTest.Selector)] 
        [SerializeField]
        private string m_TestInner1;
        
        [Dropdown(nameof(Dropdown4))] 
        [SerializeField]
        private string m_TestInner4;
        
        private IEnumerable<string> Dropdown4()
        {
            return new[] { "Empty1", "Empty2", "Empty3" };
        }
    }
    
    public class DropdownTester : MonoBehaviour, ISomeInterface
    {
        [Select] 
        [SerializeReference] private IDropdownTestSelect _testSelect;

        [SerializeField] private PrivateInnerClass m_PrivateInner;
        
        [Dropdown(nameof(Dropdown1))] 
        [SerializeField]
        private string m_Test1;

        [Dropdown(nameof(Dropdown2))] 
        [SerializeField]
        private string m_Test2;

        [Dropdown(nameof(Dropdown3))] 
        [SerializeField]
        private string m_Test3;

        [Dropdown(nameof(Dropdown4))] 
        [SerializeField]
        private string m_Test4;
        
        [Dropdown(nameof(Dropdown5))] 
        [SerializeField]
        private string m_Test5;
        
        [Serializable]
        private class PrivateInnerClass
        {
            [Dropdown(nameof(DropdownPrivateInnerClass))] 
            [SerializeField]
            private string m_TestPrivateInnerClass;
            
            private IEnumerable<string> DropdownPrivateInnerClass()
            {
                return new[] { "PrivateInnerEmpty1", "PrivateInnerEmpty2", "PrivateInnerEmpty3" };
            }
            
        }

        private IEnumerable<string> Dropdown1(GameObject go)
        {
            return new[] { go.name };
        }

        private IEnumerable<string> Dropdown2(ISomeInterface someInterface)
        {
            return new[] { someInterface.ToString() };
        }

        private IEnumerable<string> Dropdown3(DropdownTester dropdownTester)
        {
            return new[] { dropdownTester.ToString(), nameof(DropdownTester), nameof(MonoBehaviour) };
        }

        private IEnumerable<string> Dropdown4()
        {
            return new[] { "Empty1", "Empty2", "Empty3" };
        }
        
        private IEnumerable<string> Dropdown5(Test dropdownTester)
        {
            return new[] { dropdownTester.ToString(), nameof(DropdownTester), nameof(MonoBehaviour) };
        }
    }
}