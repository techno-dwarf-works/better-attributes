﻿using System;
using System.Diagnostics;
using Better.Internal.Core.Runtime;

namespace Better.Attributes.Runtime.Misc
{
    [Conditional(Defines.Editor)]
    [AttributeUsage(AttributeTargets.Field)]
    public class DetailedAttribute : MiscAttribute
    {
        public bool Nested { get; set; }

        public DetailedAttribute()
        {
            Nested = true;
        }
    }
}