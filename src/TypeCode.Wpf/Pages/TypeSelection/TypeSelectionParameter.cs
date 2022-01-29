using System;
using System.Collections.Generic;

namespace TypeCode.Wpf.Pages.TypeSelection
{
    public class TypeSelectionParameter
    {
        public bool AllowMultiSelection { get; set; }
        public IEnumerable<Type> Types { get; set; }
    }
}