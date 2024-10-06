using System.Collections.Generic;

namespace Better.Attributes.EditorAddons.Drawers.Select
{
    public class NoneCollection : IDataCollection
    {
        private const string None = "Selector not found";
        
        public string FindName(object obj)
        {
            return None;
        }

        public List<object> GetValues()
        {
            return new List<object>();
        }
    }
}