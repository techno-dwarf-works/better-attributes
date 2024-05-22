using System.Collections.Generic;

namespace Samples
{
    public class SingletonTest
    {
        private static SingletonTest _instance;

        private List<int> GetIDs()
        {
            return new List<int>()
            {
                1, 2, 3, 4
            };
        }
        
        private List<int> GetIDsProperty => new List<int>()
        {
            5, 6, 4, 8
        };
        
        private static List<int> GetIDsPropertyStatic => new List<int>()
        {
            65, 09, 56, 756
        };
        

        public static SingletonTest Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SingletonTest();
                }

                return _instance;
            }
        }
    }
}