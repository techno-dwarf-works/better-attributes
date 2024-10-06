using System;
using System.Collections.Generic;

namespace Better.Attributes.Runtime.Collections
{
    public class DropdownCollection<TValue> : List<Tuple<string, TValue>>
    {
        public DropdownCollection<TValue> Add(string name, TValue value)
        {
            Add(new Tuple<string, TValue>(name, value));
            return this;
        }
        
        public DropdownCollection<TValue> Add(TValue value)
        {
            return Add(value.ToString(), value);
        }
    }
}