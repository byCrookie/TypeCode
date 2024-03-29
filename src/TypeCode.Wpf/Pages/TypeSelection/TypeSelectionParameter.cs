﻿namespace TypeCode.Wpf.Pages.TypeSelection;

public sealed class TypeSelectionParameter
{
    public TypeSelectionParameter()
    {
        Types = new List<Type>();
    }
    
    public bool AllowMultiSelection { get; set; }
    public IEnumerable<Type> Types { get; set; }
}