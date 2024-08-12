using System;
using System.Collections.Generic;
using Better.Attributes.Runtime.Select;
using Samples.Interfaces;
using Samples.Models;
using Samples.OtherModels;
using UnityEngine;

namespace Samples
{
    public class Test2 : MonoBehaviour, ISomeInterface
    {
        [Dropdown(nameof(testArray))] [SerializeField]
        private string testStr;

        [Dropdown(nameof(testTuple))] [SerializeField]
        private int testInt;

        [Dropdown("r:SingletonTest.Instance.GetIDs()")] [SerializeField]
        private int testInt2;

        [Dropdown("r:SingletonTest.Instance.GetIDsProperty")] [SerializeField]
        private int testInt3;

        [Dropdown("r:SingletonTest.GetIDsPropertyStatic")] [SerializeField]
        private int testInt4;

        [Dropdown(nameof(testCollection))] [SerializeReference]
        private ISomeInterface testICollection;

        [SerializeField] private ScriptableObject[] gameObjects;

        [Dropdown(nameof(gameObjects))] [SerializeField]
        private ScriptableObject testObject;


        private string[] testArray = new[]
        {
            "Str1", "Str2", "Str3"
        };

        private List<Tuple<string, int>> testTuple = new List<Tuple<string, int>>()
        {
            new Tuple<string, int>("Int1", 1),
            new Tuple<string, int>("Int2", 2),
            new Tuple<string, int>("Int3", 3),
        };

        private Dictionary<string, ISomeInterface> testCollection = new Dictionary<string, ISomeInterface>()
        {
            { "test1", new SomeInterfaceImplementation1() },
            { "test2", new SomeInterfaceImplementation2() },
            { "test14", new SomeInterfaceImplementation14() }
        };
    }
}