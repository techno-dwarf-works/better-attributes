using System.Collections.Generic;

namespace Better.Attributes.EditorAddons.Drawers.Select
{
    public interface IDataCollection
    {
        public string FindName(object obj);
        public List<object> GetValues();
    }
}