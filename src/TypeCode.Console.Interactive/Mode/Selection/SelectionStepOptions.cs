﻿namespace TypeCode.Console.Interactive.Mode.Selection;

internal class SelectionStepOptions
{
    public SelectionStepOptions()
    {
        Selections = new List<string>();
    }
    
    public List<string> Selections { get; set; }
}