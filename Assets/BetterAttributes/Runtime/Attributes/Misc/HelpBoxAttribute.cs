﻿using System;
using System.Diagnostics;
using Better.Internal.Core.Runtime;

namespace Better.Attributes.Runtime.Misc
{
    [Conditional(Defines.Editor)]
    [AttributeUsage(AttributeTargets.Field)]
    public class HelpBoxAttribute : MiscAttribute
    {
        public HelpBoxAttribute(string textOrSelector)
        {
            TextOrSelector = textOrSelector;
        }

        public string TextOrSelector { get; }
    }
}