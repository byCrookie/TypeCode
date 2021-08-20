using System;
using System.Collections.Generic;

namespace TypeCode.Business.Mode.MultipleTypes
{
    internal interface IMultipleTypesSelectionContext
    {
        List<Type> SelectedTypes { get; set; }
        Type SelectedType { get; set; }
    }
}